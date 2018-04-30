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

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        public virtual Category Categorys { get; set; }

        [Display(Name = "Город")]
        public virtual City Citys { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<Complaint> Complaints { get; set; }

        public virtual ICollection<ImageAds> ImageAds { get; set; }
    }
}