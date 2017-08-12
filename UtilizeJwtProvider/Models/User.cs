
using Microsoft.EntityFrameworkCore;

namespace UtilizeJwtProvider.Models
{
    public class User
    {
        public string UserId{ get; set; }

        public string Password { get; set; }
        
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }
        
        public string Email { get; set; }
        
        public bool IsActive { get; set; }
        
       
        
    }
}