using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace SchedulingPlugIn.Models.Entity
{
    public class PageRequest
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string PageCode { get; set; }
        public string UrlPage
        {
            get
            {
                Uri requestUrl = HttpContext.Current.Request.Url;
                var protocol = Uri.UriSchemeHttp;
                var hostName = requestUrl.Host;
                return string.Format("{0}://{1}/Prospect/Index/{2}", protocol, hostName, PageCode);
            }
        }

        public virtual ICollection<PageQuestion> PageQuestionList { get; set; }
        public virtual ICollection<Calendar> CalendarList { get; set; }
    }
}