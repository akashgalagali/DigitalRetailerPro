using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DigitalRetailerPro.Models
{
    public partial class TblUsers
    {
        public TblUsers()
        {
            TblLaptopCid = new HashSet<TblLaptop>();
            TblLaptopSid = new HashSet<TblLaptop>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public string Mobile { get; set; }

        public virtual ICollection<TblLaptop> TblLaptopCid { get; set; }
        public virtual ICollection<TblLaptop> TblLaptopSid { get; set; }
    }
}
