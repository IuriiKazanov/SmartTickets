using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SmartTickets.Models
{
    [Table("Categories")]
    public class Category
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; } // ID 

        [Required(ErrorMessage = "Пожалуйста введите название категории")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Категория")]
        public string Name { get; set; } 
        public IEnumerable<Event> Events { get; set; }
    }
}