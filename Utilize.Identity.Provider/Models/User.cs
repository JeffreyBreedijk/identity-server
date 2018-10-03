using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilize.Identity.Provider.Models
{
    [Table(name:"users")]
    public class User
    {
        [Key]
        public string Id { get; set; }
        
        public string Tenant { get; set; }
        
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