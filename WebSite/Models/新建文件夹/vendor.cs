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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class vendor
    {
        public vendor()
        {
            this.members = new HashSet<member>();
        }
        [Key, ForeignKey("user")]
        public int vendorId { get; set; }
    
        public virtual ICollection<member> members { get; set; }
        public virtual user user { get; set; }
    }
}
