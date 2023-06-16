﻿using CleanArchAdvertisementApi.Core.Entities;
using WebAdvertisementApi.Models;

namespace CleanArchAdvertisementApi.Application.Interfaces
{
    public interface IAdvertisementRepository : IRepository<Advertisement>
    {        
        Task<Advertisement> ConvertEditAdvertisementAsync(EditAdvertisement entity, string img);
        Task<Advertisement> ConvertAddAdvertisementAsync(AddAdvertisement entity, string img);
        Task<int> CheckCountAdvertisementAsync(Guid id);

    }
}
