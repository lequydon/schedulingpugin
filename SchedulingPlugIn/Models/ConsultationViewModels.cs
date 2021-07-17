using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models
{
    public class ConsultationViewModels
    {
        public string ID { get; set; }
        public string IDCustomer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Customer Customer { get; set; }
        public string PageName { get; set; }
        public string IDPage { get; set; }
        public PageRequest PageRequest { get; set; }
        public DateTime Date { get; set; }
        public DateTime datecreated { get; set; }
        public int HourFrom { get; set; }
        public int MinuteFrom { get; set; }
        public int Duration{ get; set; }
        public CalendarStatus Status { get; set; }
        public string Time
        {
            get
            {
                //var timeFrom = Date.Date;
                //timeFrom = timeFrom.AddHours(HourFrom);
                //timeFrom = timeFrom.AddMinutes(MinuteFrom);
                var timeTo = Date.AddMinutes(Duration);
                var time_end = timeTo.ToString("t");
                var time_start = Date.ToString("t");
                return time_start + "-" + time_end;

            }
        }
        public string DateFomart
        {
            get
            {
                //var timeFrom = Date.Date;
                //timeFrom = timeFrom.AddHours(HourFrom);
                //timeFrom = timeFrom.AddMinutes(MinuteFrom);
                return Date.ToString("d");

            }
        }
        //public static List<ConsultationViewModels> GetList()
        //{
        //    List<ConsultationViewModels> ls = new List<ConsultationViewModels>();
        //    ls.Add(new ConsultationViewModels { ID="1", FirstName="Ana",LastName= " Trujillo", Email= "AnaTrujillo@gmail.com", Date=DateTime.Now, TimeFrom=13, TimeTo=14 ,PageName="MGI Solutions",Phome="012345678"});
        //    ls.Add(new ConsultationViewModels { ID = "2", FirstName = "Maria", LastName = " Anders", Email = "Maria@gmail.com", Date = DateTime.Now, TimeFrom = 13, TimeTo = 14, PageName = "MGI Solutions" });
        //    ls.Add(new ConsultationViewModels { ID = "3", FirstName = "Antonio ", LastName = " Moreno", Email = "Antonio@gmail.com", Date = DateTime.Now, TimeFrom = 13, TimeTo = 14, PageName = "MGI Solutions" });
        //    ls.Add(new ConsultationViewModels { ID = "4", FirstName = "Thomas ", LastName = " Hardy", Email = "Thomas@gmail.com", Date = DateTime.Now, TimeFrom = 13, TimeTo = 14, PageName = "MGI Solutions" });
        //    ls.Add(new ConsultationViewModels { ID = "5", FirstName = "Christina", LastName = "Berglund", Email = "Christina@gmail.com", Date = DateTime.Now, TimeFrom = 13, TimeTo = 14, PageName = "MGI Solutions" });
        //    ls.Add(new ConsultationViewModels { ID = "6", FirstName = "Hanna ", LastName = " Moos", Email = "Hanna@gmail.com", Date = DateTime.Now, TimeFrom = 13, TimeTo = 14, PageName = "MGI Solutions" });

        //    return ls;
        //}
    }
}