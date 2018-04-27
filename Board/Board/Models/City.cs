using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class City
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название города")]
        public string Name { get; set; }

        public virtual ICollection<Ads> Ads { get; set; }
    }
}