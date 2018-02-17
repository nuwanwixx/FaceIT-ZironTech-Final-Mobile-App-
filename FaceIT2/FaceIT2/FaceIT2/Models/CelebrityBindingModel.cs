using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceIT2.Models
{
    public class CelebrityBindingModel
    {
        public int UserId { get; set; }
        public int CelebrityId { get; set; }
        public string CelebSuggestion { get; set; }
    }
}
