using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceIT2.Models
{
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
        public int ResetCode { get; set; }
        public string Password { get; set; }
    }
}
