using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace SmartTickets.Models
{
    [Table("Change")]
    public class Change
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Range(0, 200000, ErrorMessage = "Недопустимая цена")]
        [Display(Name = "Минимальная цена билета")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 6, ErrorMessage = "Недопустимое количество")]
        [Display(Name = "Количество")]
        public int Count { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

    }
}