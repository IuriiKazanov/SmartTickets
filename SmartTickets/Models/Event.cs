using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace SmartTickets.Models
{
    [Table("Event")]
    public class Event
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите название мероприятия")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Название мероприятия")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Артист")]
        public string Artist { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
        [Display(Name = "Дата и время")]
        public string Date { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 50 символов")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 250 символов")]
        [Display(Name = "Место")]
        public string Place { get; set; }

        [Required]
        [Range(0, 200000, ErrorMessage = "Недопустимая цена")]
        [Display(Name = "Минимальная цена билета")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 500000, ErrorMessage = "Недопустимое количество")]
        [Display(Name = "Количество")]
        public int Count { get; set; }

        [Display(Name = "Категория")]
        public Category Category { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageUrl { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? CategoryId { get; set; }
    }
}