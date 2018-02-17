using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace FaceIT2.ViewModels
{
    public class RegsterViewModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfimPassword { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public Boolean LocationPermission { get; set; }
        public string MobileNumber { get; set; }
        public string Message { get; set; }

        
    }
}
