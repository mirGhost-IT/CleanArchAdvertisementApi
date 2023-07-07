using CleanArchAdvertisementApi.Application.Interfaces;
using CleanArchAdvertisementApi.Core.Entities;
using CleanArchAdvertisementApi.Sql.Context;
using CleanArchAdvertisementApi.Sql.Queries;
using Microsoft.EntityFrameworkCore;
using System;


namespace CleanArchAdvertisementApi.Infrastructure.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        #region ===[ Private Members ]=============================================================

        private readonly AdvertisementContext _db;

        #endregion

        #region ===[ Constructor ]=================================================================

        public AdvertisementRepository(AdvertisementContext db)
        {
            _db = db;
        }

        #endregion

        #region ===[ IAdvertisement Methods ]==================================================
        public async Task<string> AddAsync(Advertisement entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return "Add successfully";
        }

        public async Task<string> DeleteAsync(Guid id)
        {     
            _db.Advertisements.Remove(await GetByIdAsync(id));
            await _db.SaveChangesAsync();

            return "Delete successfully\n";
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            var res = await _db.Advertisements
                .Include(a => a.User)
                .PaginationAdv(page: 1, count: 10)
                .AdvertisementAll();

            return res;
        }
        public async Task<List<Advertisement>> MultiSortAsync(string? search, string? orderByQueryString, DateTime? startDate, DateTime? endDate)
        {
            var res = await _db.Advertisements
                .Include(a => a.User)
                .PaginationAdv(page: 1, count: 10)
                .AdvertisementMultiSort(search, orderByQueryString, startDate, endDate);

            return res;
        }

        public async Task<Advertisement> GetByIdAsync(Guid id)
        {
            var res = await _db.Advertisements
                .Include(a => a.User)
                .AdvertisementById(id);

            return res; 
        }       

        public async Task<string> UpdateAsync(Advertisement entity)
        {           
            await _db.SaveChangesAsync();

            return "Update successfully";
        }      

        public async Task<Advertisement> ConvertEditAdvertisementAsync(EditAdvertisement entity, string img)
        {

            DateTime dateTimeUtc = entity.Created.ToUniversalTime();
            entity.Created = dateTimeUtc;

            dateTimeUtc = entity.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            entity.ExpirationDate = expirationDate;

            Advertisement advertisement = await GetByIdAsync(entity.Id);

            advertisement.Text = entity.Text;
            advertisement.ImageUrl = img;
            advertisement.Rating = entity.Rating;
            advertisement.ExpirationDate = entity.ExpirationDate;

            return advertisement;
        }
       

        public async Task<Advertisement> ConvertAddAdvertisementAsync(AddAdvertisement entity, string img)
        {
            var user = await _db.Users.UserById(entity.UserId);

            if (await CheckCountAdvertisementAsync(entity.UserId) > 5)
            {
                throw new AdvertisementException("The limit of advertisements for the user has been exceeded.");
            }

            Advertisement advertisement = new Advertisement
            {
                Text = entity.Text,
                Number = entity.Number,
                Rating = entity.Rating,
                UserId = entity.UserId,
                ImageUrl = img,
                User = user
            };
            DateTime dateTimeUnspecified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            DateTime dateTimeUtc = dateTimeUnspecified.ToUniversalTime();
            advertisement.Created = dateTimeUtc;

            dateTimeUtc = entity.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            advertisement.ExpirationDate = expirationDate;

            return advertisement;
        }

        public async Task<int> CheckCountAdvertisementAsync(Guid id)
        {
            return await _db.Advertisements.Include(i=>i.User).Where(i=>i.UserId == id).CountAsync();
        }

        #endregion
    }
}
