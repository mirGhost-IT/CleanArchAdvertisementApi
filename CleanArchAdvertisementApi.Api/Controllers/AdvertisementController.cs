using CleanArch.Api.Models;
using CleanArchAdvertisementApi.Application.Interfaces;
using CleanArchAdvertisementApi.Core.Entities;
using CleanArchAdvertisementApi.Infrastructure.Repository;
using CleanArchAdvertisementApi.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebAdvertisementApi.Models;

namespace CleanArchAdvertisementApi.Api.Controllers
{
    public class AdvertisementController : BaseApiController
    {
        #region ===[ Private Members ]=============================================================

        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IImage _image;

        #endregion

        #region ===[ Constructor ]=================================================================

        public AdvertisementController(IAdvertisementRepository advertisementRepository, IWebHostEnvironment environment, IImage image)
        {
            this._advertisementRepository = advertisementRepository;
            this._environment = environment;
            this._image = image;
        }

        #endregion

        #region ===[ AdvertisementController Methods ]==================================================
        /// <summary>
        /// Получить список объявлений
        /// </summary>
        /// <returns>Возвращет все объявления из бд</returns>
        [HttpGet("GetAdvertisements")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<List<Advertisement>>> GetAll()
        {
            var apiResponse = new ApiResponse<List<Advertisement>>();

            try
            {
                var data = await _advertisementRepository.GetAllAsync();
                apiResponse.Success = true;
                apiResponse.Result = data.ToList();
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

            return apiResponse;
        }

        /// <summary>
        /// Получить список объявлений мультисортировкой 
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="orderByQueryString">Список сортировок</param>
        /// <param name="startDate">Дата создания</param>
        /// <param name="endDate">Дата конца</param>
        /// <returns>Возврашает отсартированный список объявлений </returns>
        [HttpGet("GetMultiSort")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<List<Advertisement>>> MultiSort(string? search, string? orderByQueryString, DateTime? startDate, DateTime? endDate)
        {
            var apiResponse = new ApiResponse<List<Advertisement>>();

            try
            {
                var data = await _advertisementRepository.MultiSortAsync(search, orderByQueryString, startDate, endDate);
                apiResponse.Success = true;
                apiResponse.Result = data.ToList();
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

            return apiResponse;
        }

        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpGet("GetByIdAdvertisement")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<Advertisement>> GetById(Guid id)
        {

            var apiResponse = new ApiResponse<Advertisement>();

            try
            {
                var data = await _advertisementRepository.GetByIdAsync(id);
                apiResponse.Success = true;
                apiResponse.Result = data;
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

            return apiResponse;
        }

        /// <summary>
        /// Добовляет новое объявление
        /// </summary>
        /// <param name="addAdvertisement">Модель объявления для добавления в формате FromForm</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<string>> Add([FromForm] AddAdvertisement advertisement)
        {
            var apiResponse = new ApiResponse<string>();

            try
            {
                var img = await _image.AddImgAsync(advertisement, _environment.WebRootPath);
                var adv = await _advertisementRepository.ConvertAddAdvertisementAsync(advertisement, img);
                var data = await _advertisementRepository.AddAsync(adv);
                apiResponse.Success = true;
                apiResponse.Result = data;
            }
            catch (SqlException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("SQL Exception:", ex);
            }
            catch (AdvertisementException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("Add Exception:", ex);
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("Exception:", ex);
            }
            

            return apiResponse;
        }

        /// <summary>
        /// Изменение объявления
        /// </summary>
        /// <param name="editAdvertisement">Модель объявления для изменения в формате FromForm</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPut("Edit")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<string>> Update([FromForm] EditAdvertisement advertisement)
        {
            var apiResponse = new ApiResponse<string>();

            try
            {
                var img = await _image.EditImgAsync(advertisement, _environment.WebRootPath);
                var adv = await _advertisementRepository.ConvertEditAdvertisementAsync(advertisement, img);
                var data = await _advertisementRepository.UpdateAsync(adv);
                apiResponse.Success = true;
                apiResponse.Result = data;
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

            return apiResponse;
        }

        /// <summary>
        /// Удаляет объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Строку подтверждения успеха</returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse<string>> Delete(Guid id)
        {
            var apiResponse = new ApiResponse<string>();

            try
            {                
                var data = await _advertisementRepository.DeleteAsync(id);
                data += _image.DeleteImgAsync(id, _environment.WebRootPath);
                apiResponse.Success = true;
                apiResponse.Result = data;
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

            return apiResponse;
        }
        #endregion
    }
}
