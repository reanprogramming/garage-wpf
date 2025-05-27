using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_Management_System.Class
{
    class arr_data_issue
    {
        public string Issue_ID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Issue { get; set; }
        public string Responder { get; set; }
    }
    class arr_plateNumber
    {
        public string Plate_ID { get; set; }
        public string Branchcode { get; set; }
        public string Type_ID { get; set; }
        public string Titile { get; set; }
        public string inputter { get; set; }
        public string LastUpdate { get; set; }
    }


    public static class search
    {
        public static string tran_id { get; set; }
        public static string search_action { get; set; }
    }
}
