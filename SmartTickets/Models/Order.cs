using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace SmartTickets.Models
{
    [Table("Orders")]
    public class Order
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Некорректное значение")]
        [Display(Name = "Количество")]
        public int Count { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Number { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime Date { get; set; }
    }
}