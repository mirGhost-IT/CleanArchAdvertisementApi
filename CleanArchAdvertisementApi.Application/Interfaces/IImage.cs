using CleanArchAdvertisementApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdvertisementApi.Models;

namespace CleanArchAdvertisementApi.Application.Interfaces
{
    public interface IImage
    {
        Task<string> DeleteImgAsync(Guid id, string webRootPath);
        Task<string> EditImgAsync(EditAdvertisement entity, string webRootPath);
        Task<string> AddImgAsync(AddAdvertisement entity, string webRootPath);
        MemoryStream ImageResize(string imagePhysicalPath, int width, int height);
    }
}
