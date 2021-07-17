using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.RepositoryLayer;
using SchedulingPlugIn.RepositoryLayer.Interface;
using SchedulingPlugIn.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Servicex
{
    public class PageRequestService : IPageRequestService
    {
        private IDALContext context;
        
        public PageRequestService(IDALContext dal)
        {
            context = dal;
        }
        public PageRequestService()
        {
            context = new DALContext(false);
        }
        public PageRequest Create(string pageID, string pageName, string pageCode)
        {
            var pageRequest = new PageRequest { ID = pageID, Name = pageName, PageCode = pageCode };
            context.PageRequests.Create(pageRequest);
            context.SaveChanges();
            return pageRequest;
        }

        public List<PageRequest> GetList()
        {
            var listPage= context.PageRequests.All().ToList();
            foreach (var item in listPage)
            {
                item.ID = HttpUtility.HtmlDecode(item.ID);
                item.Name= HttpUtility.HtmlDecode(item.Name);
            }
            return listPage;
        }
        public PageRequest Get(string id)
        {
            var page = context.PageRequests.Filter(e => e.ID == id).FirstOrDefault();
            return page;
        }

        public Service.Interface.CODE_RESULT_RETURN Create(PageRequest t)
        {
            t.PageCode = Helper.Util.GenerateCode(10);
            var page = context.PageRequests.Filter(e => e.ID == t.ID).FirstOrDefault();
            if (page != null)
                return CODE_RESULT_RETURN.MaTrung;
            var pageName = context.PageRequests.Filter(e => e.Name == t.Name).FirstOrDefault();
            if (pageName != null)
                return CODE_RESULT_RETURN.TenTrung;
            context.PageRequests.Create(t);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }

        public Service.Interface.CODE_RESULT_RETURN Update(PageRequest t)
        {
            var page = context.PageRequests.Filter(e => e.ID != t.ID && e.Name == t.Name).FirstOrDefault();
            if (page != null)
                return CODE_RESULT_RETURN.TenTrung;
            page = context.PageRequests.Find(t.ID);
            page.Name = t.Name;
            context.SaveChanges();
            return Service.Interface.CODE_RESULT_RETURN.ThanhCong;
        }

        public CODE_RESULT_RETURN Delete(PageRequest t)
        {
            //var page = context.PageRequests.Filter(e => e.ID == t.ID).FirstOrDefault();
            context.PageRequests.Delete(e=>e.ID==t.ID);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
            
        }       

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        CODE_RESULT_RETURN IService<PageRequest>.Create(PageRequest t)
        {
            throw new NotImplementedException();
        }

        CODE_RESULT_RETURN IService<PageRequest>.Update(PageRequest t)
        {
            throw new NotImplementedException();
        }
    }
}