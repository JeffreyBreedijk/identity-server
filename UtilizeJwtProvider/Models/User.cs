using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace UtilizeJwtProvider.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        
        public Tenant Tenant { get; set; }
        
        public string LoginCode { get; set;}
        
        public string Hash { get; set; }
        
        public string Salt { get; set; }
        
        public string Firstname { get; set;}
        
        public string Lastname { get; set; }
        
        public string Email { get; set;}
        
        public bool IsActive { get; set;}
        
        public bool IsDeleted { get; set;}
        
        public string DebtorId { get; set;}
        
      
    }
}