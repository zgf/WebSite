//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace WebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class company
    {
        public company()
        {
            this.news = new HashSet<news>();
            this.purchases = new HashSet<purchase>();
        }
    
        public int companyId { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^\W[\W\d_]{5-19}$", ErrorMessage = "Please enter valid name.")]
        [Remote("Verify", "CheckCompanyNameRegister", ErrorMessage = "user name is registered")]

        public string company_name { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter valid phone.")]
        public string company_telephone { get; set; }
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage ="Please enter valid email")]
        public string company_mail { get; set; }
        [Required(ErrorMessage = "*")]

        public string company_address { get; set; }

        public string company_introduction { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(20,MinimumLength = 6,ErrorMessage ="Please enter valid password")]
        public string company_password { get; set; }
    
        public virtual ICollection<news> news { get; set; }
        public virtual ICollection<purchase> purchases { get; set; }
    }
}
