using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchAdvertisementApi.Infrastructure.Repository
{
    public class AdvertisementException : Exception
    {
        public AdvertisementException(string message) : base(message)
        {
        }
    }
}
