using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using SchedulingPlugIn.Servicex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public class QuestionController : BaseController
    {
        PageQuestionService pagequestionService = new PageQuestionService();
        PageRequestService PageRequestService = new PageRequestService();
        // GET: Question
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Questionlist()
        {
            var list = pagequestionService.GetList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getlistdata()
        {
            var datalist = pagequestionService.GetListpage();
            return Json(datalist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getlisttap(string PageID)
        {
            var pagelist = pagequestionService.Listquetion(PageID).OrderBy(e => e.OrderQuestion).ToList();
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListIDPage(string IDlist)
        {
            var listID = pagequestionService.ListPage(IDlist);
            return Json(listID, JsonRequestBehavior.AllowGet);
        }
        public int UpdateQuestions(PageQuestion up)
        {
            //up.Question = HttpUtility.HtmlEncsode(up.Question);
            var update = pagequestionService.Update(up);
            return (int)update;
        }
        public int Delete(string deleteid)
        {
            var del = pagequestionService.Delete(deleteid);
            return (int)del;
        }
        public int Create(string contentQuestion, string pageID)
        {
            var create = pagequestionService.Create(contentQuestion, pageID);
            return (int)create;
        }
        public int Drag(string oldindex, string newindex, string PageRequestid1)
        {
            var drag = pagequestionService.ListDrap(oldindex, newindex, PageRequestid1);
            return 1;
        }
    }
}