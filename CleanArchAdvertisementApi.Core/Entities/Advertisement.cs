using System.ComponentModel.DataAnnotations;

namespace CleanArchAdvertisementApi.Core.Entities
{
    public class Advertisement
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле Number обязательно для заполнения")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение поля Number должно быть положительным")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Поле UserId обязательно для заполнения")]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required(ErrorMessage = "Поле Text обязательно для заполнения")]
        [StringLength(1000, ErrorMessage = "Длина поля Text не должна превышать 1000 символов")]
        public string Text { get; set; }

        public string ImageUrl { get; set; }

        [Range(0, 10, ErrorMessage = "Значение поля Rating должно быть в диапазоне от 0 до 10")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Поле Created обязательно для заполнения")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Поле ExpirationDate обязательно для заполнения")]
        public DateTime ExpirationDate { get; set; }
    }
}
