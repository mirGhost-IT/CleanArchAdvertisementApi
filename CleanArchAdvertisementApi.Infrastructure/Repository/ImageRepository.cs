using CleanArchAdvertisementApi.Application.Interfaces;
using CleanArchAdvertisementApi.Core.Entities;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace CleanArchAdvertisementApi.Infrastructure.Repository
{
    public class ImageRepository : IImageRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly IAdvertisementRepository _advertisementRepository;

        #endregion

        #region ===[ Constructor ]=================================================================

        public ImageRepository(IAdvertisementRepository advertisementRepository)
        {
            this._advertisementRepository = advertisementRepository;
        }

        #endregion

        #region ===[ ImageRepository Methods ]==================================================
        public MemoryStream ImageResize(string imagePhysicalPath, int width, int height)
        {
            using (var image = Image.Load(imagePhysicalPath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return ms;
                }
            }
        }
        public async Task<string> DeleteImgAsync(Guid id, string webRootPath)
        {
            var adv = await _advertisementRepository.GetByIdAsync(id);
            var imagePath = adv.ImageUrl;
            var imagePhysicalPath = Path.Combine(webRootPath, imagePath.TrimStart('/'));
            File.Delete(imagePhysicalPath);

            return "Delete img successfully";
        }
        public async Task<string> EditImgAsync(EditAdvertisement entity, string webRootPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(entity.ImageUrl.FileName);
            string filePath = Path.Combine(webRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await entity.ImageUrl.CopyToAsync(stream);
            }

            string imageUrl = "/images/" + fileName;
            DeleteImgAsync(entity.Id, webRootPath);

            return imageUrl;
        }
        public async Task<string> AddImgAsync(AddAdvertisement entity, string webRootPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(entity.ImageUrl.FileName);
            string filePath = Path.Combine(webRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await entity.ImageUrl.CopyToAsync(stream);
            }

            string imageUrl = "/images/" + fileName;

            return imageUrl;
        }
        #endregion
    }
}
