using CleanArchAdvertisementApi.Core.Entities;

namespace CleanArchAdvertisementApi.Application.Interfaces
{
    public interface IImageRepository
    {
        Task<string> DeleteImgAsync(Guid id, string webRootPath);
        Task<string> EditImgAsync(EditAdvertisement entity, string webRootPath);
        Task<string> AddImgAsync(AddAdvertisement entity, string webRootPath);
        MemoryStream ImageResize(string imagePhysicalPath, int width, int height);
    }
}
