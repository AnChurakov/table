using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*Жалобы на объявления*/

namespace Board.Models
{
    public class Complaint
    {
        public Guid Id { get; set; }

        public virtual Ads Ads { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}