using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Board.Models
{
    public class Ads
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название объвления")]
        public string Name { get; set; }

        public DateTime? DateCreation { get; set; }

        public virtual Category Categorys { get; set; }

        public virtual City Citys { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}