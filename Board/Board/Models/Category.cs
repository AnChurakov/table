using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название категории")]
        public string Name { get; set; }

        public virtual ICollection<Ads> Ads { get; set; }
    }
}