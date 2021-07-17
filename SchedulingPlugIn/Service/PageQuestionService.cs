using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.RepositoryLayer;
using SchedulingPlugIn.RepositoryLayer.Interface;
using SchedulingPlugIn.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Service
{
    public class PageQuestionService : IPageQuestionService
    {
        private IDALContext context;

        public PageQuestionService(IDALContext dalQ)
        {
            context = dalQ;
        }
        public PageQuestionService()
        {
            context = new DALContext();
        }
        public CODE_RESULT_RETURN Create(string contentQuestion, string pageID)
        {
            int maxOrderQuestion = 0;
            var maxQuestion = context.PageQuestions.Filter(e => e.PageRequestID == pageID).OrderByDescending(e => e.OrderQuestion).FirstOrDefault();
            if (maxQuestion != null)
                maxOrderQuestion = maxQuestion.OrderQuestion;
            PageRequest pageTab = context.PageRequests.Filter(e => e.ID == pageID).FirstOrDefault();
            if (pageTab != null)
            {
                var question = new PageQuestion { OrderQuestion = maxOrderQuestion + 1, ID = Guid.NewGuid().ToString(), Question = contentQuestion, PageRequestID = pageTab.ID };
                context.PageQuestions.Create(question);
                context.SaveChanges();
                return CODE_RESULT_RETURN.ThanhCong;
            }
            else
                return CODE_RESULT_RETURN.ThatBai;
        }

        public CODE_RESULT_RETURN Delete(string t)
        {
            var obj = context.PageQuestions.Filter(e => e.ID == t).FirstOrDefault();
            return Delete(obj);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public PageQuestion Get(string id)
        {
            var Question = context.PageQuestions.Filter(e => e.ID == id).FirstOrDefault();
            return Question;

        }

        public List<PageQuestion> GetList()
        {
            return context.PageQuestions.All().ToList();
        }

        public CODE_RESULT_RETURN Update(PageQuestion q)
        {
            if (q.Question == null)
            {
                return CODE_RESULT_RETURN.ThatBai;
            }
            else
            {
                PageQuestion updateQuestions = context.PageQuestions.Find(q.ID);
                updateQuestions.Question = q.Question;
                updateQuestions.OrderQuestion = q.OrderQuestion;
                context.SaveChanges();
                return Interface.CODE_RESULT_RETURN.ThanhCong;
            }
            

        }
        public List<QuestionViewModels> GetListpage()
        {
            var pageN = context.PageRequests.All().Select(e => new QuestionViewModels
            {
                Name = e.Name,
                PageID = e.ID

            });
            return pageN.ToList();
        }
        public List<QuestionViewModels> ListPage(string list)
        {
            var pagelistID = context.PageRequests.All().Select(a => new QuestionViewModels
            {
                PageID = a.ID,
                Name = a.Name

            });
            return pagelistID.ToList();
        }
        //Drag and Drop
        public CODE_RESULT_RETURN ListDrap(string oldindex, string newindex, string PageRequestid1)
        {
            var newindextemp = Int32.Parse(newindex);
            var oldindextemp = Int32.Parse(oldindex);
            var datatemp = 0;
            var Flag = 0;
            var Draplist = context.PageQuestions.Filter(e => e.PageRequestID == PageRequestid1).OrderBy(e => e.OrderQuestion).ToList();
            if (newindextemp < oldindextemp)
                for (var i = 0; i <= Draplist.Count - 1; i++)
                {
                    if (newindextemp == i)
                    {
                        if (Flag == 0)
                        {
                            Flag = 1;
                            datatemp = Draplist[i].OrderQuestion;
                        }
                        Draplist[i].OrderQuestion = Draplist[i + 1].OrderQuestion;
                    }
                    if (oldindextemp != i && newindextemp != i && Flag == 1)
                        Draplist[i].OrderQuestion = Draplist[i + 1].OrderQuestion;
                    if (oldindextemp == i)
                    {
                        if (Flag == 1)
                        {
                            Draplist[i].OrderQuestion = datatemp;
                        }
                        break;
                    }


                }
            var flag1 = 0;
            if (newindextemp > oldindextemp)
                for (var i = Draplist.Count - 1; i >= 0; i--)
                {

                    if (newindextemp == i)
                    {
                        if (flag1 == 0)
                        {
                            flag1 = 1;
                            datatemp = Draplist[i].OrderQuestion;
                        }
                        Draplist[i].OrderQuestion = Draplist[i - 1].OrderQuestion;
                    }
                    if (oldindextemp != i && newindextemp != i && flag1 == 1)
                        Draplist[i].OrderQuestion = Draplist[i - 1].OrderQuestion;
                    if (oldindextemp == i)
                    {
                        if (flag1 == 1)
                        {
                            Draplist[i].OrderQuestion = datatemp;
                        }
                        break;
                    }

                }
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        public List<QuestionViewModels> Listquetion(string PageID)
        {
            var datagrid = context.PageQuestions.Filter(e => e.PageRequestID == PageID).Select(e => new QuestionViewModels
            {
                ID = e.ID,
                Question = e.Question,
                OrderQuestion = e.OrderQuestion,
                pagerequest = e.PageRequestID

            }).ToList();
            foreach (var item in datagrid)
            {
                item.Question = HttpUtility.HtmlDecode(item.Question);
            }
            return datagrid;
        }
        public List<QuestionViewModels> GetListQuestion(string pageCode)
        {

            var PageRequest = context.PageRequests.Filter(e => e.PageCode == pageCode).FirstOrDefault();
            var result = context.PageQuestions.Filter(e => e.PageRequestID == PageRequest.ID).Select(e => new QuestionViewModels
            {
                ID = e.ID,
                Question = e.Question,
                OrderQuestion = e.OrderQuestion
            }).OrderBy(e => e.OrderQuestion);
            return result.ToList();
        }

        public CODE_RESULT_RETURN Delete(PageQuestion t)
        {

            var obj = context.PageQuestions.Filter(e => e.ID == t.ID).FirstOrDefault();
            var obj1 = context.CalendarPoolings.Filter(e => e.IDPageQuestion == obj.ID);
            foreach(var i in obj1)
            {
                i.IDPageQuestion = null;
            }
            context.SaveChanges();
            if (obj == null)
                return CODE_RESULT_RETURN.ThatBai;
            else
                context.PageQuestions.Delete(obj);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }

        public CODE_RESULT_RETURN Create(PageQuestion t)
        {
            throw new NotImplementedException();
        }
        public string getIDPageRequest(string pageCode)
        {
            var PageRequest = context.PageRequests.Filter(e => e.PageCode == pageCode).FirstOrDefault();
            return PageRequest.ID;
        }
        //Drag grid 
        //    public string Getdrag(string dataitem,int newindex)
        //    {
        //        var Drag = context.PageQuestions.Filter(e => e.ID==PageRe).FirstOrDefault();
        //        return Drag.ID;
        //}
    }
}