using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using SchedulingPlugIn.Service.Interface;
using SchedulingPlugIn.Servicex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public class PageController : BaseController
    {
        PageRequestService pageRequestService = new PageRequestService();
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListPage()
        {
            var listPage = pageRequestService.GetList();
            return Json(listPage, JsonRequestBehavior.AllowGet);
        }
        public int Create(PageRequest pagets)
        {
            var rs = pageRequestService.Create(pagets);
            return (int) rs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Delete(PageRequest pagedelete)
        {
            
             var delete =   pageRequestService.Delete(pagedelete);
                return (int) delete;           
          
        }
        public int Update(PageRequest up)
        {
           var update= pageRequestService.Update(up);
            return (int) update;
        }
    }
}