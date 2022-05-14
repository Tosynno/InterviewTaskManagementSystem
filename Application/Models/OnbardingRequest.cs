using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class OnbardingRequest
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
