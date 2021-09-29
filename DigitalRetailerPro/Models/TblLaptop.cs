using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DigitalRetailerPro.Models
{
    public partial class TblLaptop
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Configuration { get; set; }
        public string PaymentMode { get; set; }
        public double Cost { get; set; }
        public int? SidId { get; set; }
        public int? CidId { get; set; }
        public bool? Available { get; set; }

        public virtual TblUsers Cid { get; set; }
        public virtual TblUsers Sid { get; set; }
    }
}
