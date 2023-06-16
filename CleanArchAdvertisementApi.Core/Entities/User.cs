using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CleanArchAdvertisementApi.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле Text обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Длина поля Name не должна превышать 50 символов")]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        [NotMapped]
        public List<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
    }
}
