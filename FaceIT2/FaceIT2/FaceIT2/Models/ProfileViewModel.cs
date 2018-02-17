using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceIT2.ViewModels
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool LocationPermission { get; set; }
        public bool BlockStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> ResetCode { get; set; }
        public int PhotoID { get; set; }
        public string Link { get; set; }
        public bool DeleteStatus { get; set; }
        public int UploaderID { get; set; }
    }
}
