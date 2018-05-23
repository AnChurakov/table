using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Board.Models
{
    public class Banner
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название баннера")]
        public string Name { get; set; }

        [Display(Name = "Текстовый блок баннера")]
        public string Description { get; set; }

        [Display(Name = "На странице отдельного объявления")]
        public bool SinglePage { get; set; }

        public virtual ICollection<ImageBanner> ImageBanner { get; set; }
    }
}