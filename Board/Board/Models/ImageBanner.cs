using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Board.Models;

namespace Board.Models
{
    public class ImageBanner
    {
        public Guid Id { get; set; }

        public string Path { get; set; }

        public DateTime? DateCreate { get; set; }

        public virtual Banner Banners { get; set; }
    }
}