using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceIT2.Models
{
    public class ReportUserBindingModel
    {
        public int UserId { get; set; }
        public int ReportUserId { get; set; }
        public string ReportDetail { get; set; }
    }
}
