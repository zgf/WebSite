//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public user()
        {
            this.companies = new HashSet<company>();
            this.experts = new HashSet<expert>();
            this.vendors = new HashSet<vendor>();
        }
    
        public int userId { get; set; }
        public string user_type { get; set; }
        public string user_telephone { get; set; }
        public string user_mail { get; set; }
        public string user_name { get; set; }
        public string user_address { get; set; }
        public string user_introduction { get; set; }
        public string user_password { get; set; }
    
        public virtual ICollection<company> companies { get; set; }
        public virtual ICollection<expert> experts { get; set; }
        public virtual ICollection<vendor> vendors { get; set; }
    }
}