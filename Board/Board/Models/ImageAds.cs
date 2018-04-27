using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Board.Models
{
    public class ImageAds
    {
        public Guid Id { get; set; }

        public string Path { get; set; }

        public virtual Ads Ads { get; set; }
    }
}