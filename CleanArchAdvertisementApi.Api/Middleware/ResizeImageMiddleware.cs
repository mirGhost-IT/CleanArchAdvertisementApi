using CleanArch.Api.Models;
using CleanArchAdvertisementApi.Application.Interfaces;
using CleanArchAdvertisementApi.Core.Entities;
using CleanArchAdvertisementApi.Logging;
using System.Data.SqlClient;
using System.Net;
using System.Runtime;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CleanArchAdvertisementApi.Api.Middleware
{
    public class ResizeImageMiddleware : IMiddleware
    {
        #region ===[ Private Members ]=============================================================
        private readonly IImage _image;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IWebHostEnvironment _environment;

        #endregion

        #region ===[ Constructor ]=================================================================
        public ResizeImageMiddleware(IImage image, IWebHostEnvironment environment, IAdvertisementRepository advertisementRepository)
        {
            _environment = environment;
            _image = image;
            _advertisementRepository = advertisementRepository;
        }
#endregion

        #region ===[ ImageRepository Methods ]==================================================
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            var apiResponse = new ApiResponse<string>();

            try
            {
                Guid id = Guid.Parse(context.Request.Query["id"]);
                int height = int.Parse(context.Request.Query["height"]);
                int width = int.Parse(context.Request.Query["width"]);

                var advertisement = await _advertisementRepository.GetByIdAsync(id);
                var imagePath = advertisement.ImageUrl;
                var imagePhysicalPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));

                var ms = _image.ImageResize(imagePhysicalPath, width, height);
                context.Response.ContentType = "image/jpeg";
                await ms.CopyToAsync(context.Response.Body);

                apiResponse.Success = true;

                await next(context);
            }
            catch (SqlException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("SQL Exception:", ex);
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("Exception:", ex);
            }
        }
        #endregion
    }
}
