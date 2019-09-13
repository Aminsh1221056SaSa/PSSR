using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Configuration
{
    public class ApplicationSettings
    {
        public string ApplicationTitle { get; set; }
        public string ClientId { get; set; }
        public string Authority { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPAccount { get; set; }
        public string SMTPPassword { get; set; }
        public string OilApiAddress{ get; set; }
        public string OilApiName { get; set; }
    }

    public class SqlConnectionHelper
    {
        public string DefaultConnection { get; set; }
    }
}
