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

    public partial class audit
    {
        [Required(ErrorMessage = "*")]

        public int bidId { get; set; }
        [Required(ErrorMessage = "*")]

        public int expertId { get; set; }
        public int auditId { get; set; }
        [Required(ErrorMessage = "*")]

        public string audit_content { get; set; }
        public DateTime audit_time { get; set; }

        public virtual bid bid { get; set; }
        public virtual expert expert { get; set; }
    }
}