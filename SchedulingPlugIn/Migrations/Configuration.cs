namespace SchedulingPlugIn.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SchedulingPlugIn.Models;
    using SchedulingPlugIn.Models.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SchedulingPlugIn.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            
        }

        protected override void Seed(SchedulingPlugIn.Models.ApplicationDbContext context)
        {

            #region Seed Data for OWin

            //var passwordHash = new PasswordHasher();
            //string password = passwordHash.HashPassword("12345");
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "1",
            //        UserName = "donle2044@4gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "12345678911",
            //        Email = "donle2044@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "2",
            //        UserName = "canhle2044@gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "12345678911",
            //        Email = "canhle2044@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "3",
            //        UserName = "dinhlan77@gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "696969",
            //        Email = "dinhlan77@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.SaveChanges();
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "4",
            //        UserName = "thuhuyennguyen9723@gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "696969",
            //        Email = "thuhuyennguyen9723@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.SaveChanges();
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "5",
            //        UserName = "ndtthinh18281992@gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "696969",
            //        Email = "ndtthinh18281992@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.SaveChanges();
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "6",
            //        UserName = "nguyenngoctri1902@gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "696969",
            //        Email = "nguyenngoctri1902@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.SaveChanges();
            //context.Users.AddOrUpdate(u => u.Id,
            //    new ApplicationUser
            //    {
            //        Id = "7",
            //        UserName = "ndnghia69@gmail.com",
            //        PasswordHash = password,
            //        PhoneNumber = "696969",
            //        Email = "ndnghia69@gmail.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString("D")
            //    });
            //context.SaveChanges();
            //context.Users.AddOrUpdate(u => u.Id,
            //   new ApplicationUser
            //   {
            //       Id = "8",
            //       UserName = "ngocbo91@gmail.com",
            //       PasswordHash = password,
            //       PhoneNumber = "696969",
            //       Email = "ngocbo91@gmail.com",
            //       EmailConfirmed = true,
            //       PhoneNumberConfirmed = true,
            //       SecurityStamp = Guid.NewGuid().ToString("D")
            //   });
            //context.SaveChanges();
            //var lsRole = new List<IdentityRole>
            //{
            //    new IdentityRole{Id ="1", Name = RoleInPage.Administrator },
            //    new IdentityRole{Id ="2", Name = RoleInPage.User},

            //};
            //lsRole.ForEach(t => context.Roles.AddOrUpdate(e => e.Name, t));
            //context.SaveChanges();

            //var users1 = context.Users.Where(e => e.UserName == "ndnghia69@gmail.com").ToList();
            //foreach (var user in users1)
            //{
            //    user.Roles.Add(new IdentityUserRole { RoleId = "1", UserId = user.Id });
            //    context.SaveChanges();
            //}
            //var users2 = context.Users.Where(e => e.UserName == "nguyenngoctri1902@gmail.com").ToList();
            //foreach (var user in users2)
            //{
            //    user.Roles.Add(new IdentityUserRole { RoleId = "1", UserId = user.Id });
            //    context.SaveChanges();
            //}
            //var users3 = context.Users.Where(e => e.UserName == "thuhuyennguyen9723@gmail.com").ToList();
            //foreach (var user in users3)
            //{
            //    user.Roles.Add(new IdentityUserRole { RoleId = "1", UserId = user.Id });
            //    context.SaveChanges();
            //}
            //var users4 = context.Users.Where(e => e.UserName == "dinhlan77@gmail.com").ToList();
            //foreach (var user in users4)
            //{
            //    user.Roles.Add(new IdentityUserRole { RoleId = "1", UserId = user.Id });
            //    context.SaveChanges();
            //}
            //var users = context.Users.Where(e => e.UserName == "ndtthinh18281992@gmail.com").ToList();
            //foreach (var user in users)
            //{
            //    user.Roles.Add(new IdentityUserRole { RoleId = "1", UserId = user.Id });
            //    context.SaveChanges();
            //}

            #endregion

            #region Create data for PageRequest
            var lsPage = new List<PageRequest>
            {
                new PageRequest{ ID= "MGISolutions", Name= "MGI Solutions",PageCode=Helper.Util.GenerateCode(10)},
                new PageRequest{ ID = "JouleBroker", Name= "Joule Broker",PageCode=Helper.Util.GenerateCode(9)}
            };
            lsPage.ForEach(t => context.PageRequestDbSet.AddOrUpdate(e => e.Name, t));
            context.SaveChanges();
            #endregion

            #region Create data for PageQuestion
            var lsQuestion = new List<PageQuestion>
            {
                new PageQuestion{ID="1", Question="Lorem Ipsum is simply dummy text of the printing and typesetting industry?", OrderQuestion= 1, PageRequestID="MGISolutions" },
                new PageQuestion{ID="2", Question="Lorem Ipsum has been the industry's standard dummy text ever since the 1500s?", OrderQuestion= 2, PageRequestID="MGISolutions" },
                new PageQuestion{ID="3", Question="It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages?", OrderQuestion= 2, PageRequestID="JouleBroker" },
                new PageQuestion{ID="4", Question="Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC", OrderQuestion= 3, PageRequestID="JouleBroker" },
                new PageQuestion{ID="5", Question="Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur?", OrderQuestion= 4, PageRequestID="JouleBroker" },
                new PageQuestion{ID="6", Question="The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested?", OrderQuestion= 5, PageRequestID="JouleBroker" },
                new PageQuestion{ID="7", Question="There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form?", OrderQuestion= 6, PageRequestID="JouleBroker" },
                new PageQuestion{ID="8", Question="All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet?", OrderQuestion= 7, PageRequestID="JouleBroker" },
                new PageQuestion{ID="9", Question="It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable", OrderQuestion= 8, PageRequestID="JouleBroker" }
            };
            lsQuestion.ForEach(t => context.PageQuestionsDbSet.AddOrUpdate(a => a.ID, t));
            context.SaveChanges();
            #endregion

            #region Create data for Customer
            var lsCustomer = new List<Customer>
            {
                new Customer{ ID = "1", FirstName= "Maria", LastName="Anders", Email="MariaAnders@gmail.com",Phone="0123456"},
                new Customer{ ID= "2", FirstName= "Dany", LastName="Anders", Email="DanyAnders@gmail.com",Phone="12345678"},
            };
            lsCustomer.ForEach(t => context.CustomerDbSet.AddOrUpdate(e => e.ID, t));
            context.SaveChanges();
            #endregion
            #region Create data for SessionTimeSlot
            var sessionTimeSlot = new List<SessionTimeSlot>
            {
                new SessionTimeSlot  {ID = "1", SessionStart = new DateTime(2019, 9, 1), SessionEnd = new DateTime(2019,9,18), Duration = 90},
                new SessionTimeSlot { ID = "2", SessionStart = new DateTime(2019, 9, 10), SessionEnd = new DateTime(2019, 9, 16), Duration = 45 },
                new SessionTimeSlot { ID = "3", SessionStart = new DateTime(2019, 9, 21), SessionEnd = new DateTime(2019, 9, 29), Duration = 90, TimeZone="SA Eastern Standard Time" },
                new SessionTimeSlot { ID = "4", SessionStart = new DateTime(2019, 9, 14), SessionEnd = new DateTime(2019, 9, 18), Duration = 60 },
                new SessionTimeSlot { ID = "5", SessionStart = new DateTime(2019, 9, 19), SessionEnd = new DateTime(2019, 9, 28), Duration = 90 },
                new SessionTimeSlot { ID = "6", SessionStart = new DateTime(2019, 9, 4), SessionEnd = new DateTime(2019, 9, 15), Duration = 90 },
            };
            sessionTimeSlot.ForEach(t => context.SessionTimeSlotDbSet.AddOrUpdate(e => e.ID, t));
            context.SaveChanges();
            #endregion

            #region Create data for TimeSlots
            var lsTimeSlot = new List<TimeSlot>
            {
                new TimeSlot{ID = "10", DateStart = new DateTime(2019,9,1,17,0,0), Hour= 17, Minute= 0, Duration = 90,  Note = "Meeting with director",ID_SessionTimeSlot="1"},
                new TimeSlot{ID = "20", DateStart = new DateTime(2019,9,13,13,30,0), Hour= 13, Minute= 30, Duration = 90,  Note = "Meeting with director",ID_SessionTimeSlot="1"},
                new TimeSlot{ID = "30", DateStart = new DateTime(2019,9,11,15,15,0), Hour= 15, Minute= 15, Duration = 90,  Note = "Meeting with director",ID_SessionTimeSlot="1"},
                new TimeSlot{ID = "40", DateStart = new DateTime(2019,9,17,9,45,0), Hour= 9, Minute= 45, Duration = 90,  Note = "Meeting with director",ID_SessionTimeSlot="1"},
                new TimeSlot{ID = "50", DateStart = new DateTime(2019,9,12,15,0,0), Hour= 15, Minute= 0, Duration = 45,  Note ="Meeting with director",ID_SessionTimeSlot="2"},
                new TimeSlot{ID = "60", DateStart = new DateTime(2019,9,15,19,0,0), Hour= 19, Minute= 0, Duration = 45,  Note = "Meeting with director",ID_SessionTimeSlot="2"},
                new TimeSlot{ID = "102", DateStart = new DateTime(2019,9,28,20,0,0), Hour= 20, Minute= 0, Duration = 90,  Note = "Meeting with director",ID_SessionTimeSlot="3"},
                new TimeSlot{ID = "104", DateStart = new DateTime(2019,9,29,16,0,0), Hour= 16, Minute= 0, Duration = 90,  Note = "Meeting with director",ID_SessionTimeSlot="3"},
                new TimeSlot{ID = "70", DateStart = new DateTime(2019,9,23,13 ,30,0), Hour= 13, Minute= 30, Duration = 90,  Note = "NMeeting with director",ID_SessionTimeSlot="3"},
                new TimeSlot{ID = "80", DateStart = new DateTime(2019,9,14,16,30,0), Hour= 16, Minute= 30, Duration = 60,  Note = "Meeting with director",ID_SessionTimeSlot="4"},
                new TimeSlot{ID = "90", DateStart = new DateTime(2019,9,17,10,45,0), Hour= 10, Minute= 45, Duration = 60,  Note = "Meeting with director",ID_SessionTimeSlot="4"},
                new TimeSlot{ID = "100", DateStart = new DateTime(2019,9,18,8,0,0), Hour= 8, Minute= 0, Duration = 60, Note = "Meeting with director",ID_SessionTimeSlot="4"},
                new TimeSlot{ID = "101", DateStart = new DateTime(2019,9,10,13,40,0), Hour= 13, Minute= 40, Duration = 45, Note = "Meeting with director",ID_SessionTimeSlot="2"},
                 new TimeSlot{ID = "105", DateStart = new DateTime(2019,9,1,9,45,0), Hour= 9, Minute= 45, Duration = 90, Note = "Meeting with director",ID_SessionTimeSlot="1"}
            };
            lsTimeSlot.ForEach(t => context.TimeSlotDbSet.AddOrUpdate(e => e.ID, t));
            context.SaveChanges();
            #endregion

            #region Create data for calender
            var lsCalendar = new List<Calendar>
            {
               new Calendar{ ID = "1", IDPageRequest= "MGISolutions", IDCustomer="1", Date=new DateTime(2019,08,30,00,20,43),DateCreated = DateTime.Now,HourFrom=12,MinuteFrom=24,Duration=30, Status=CalendarStatus.Confirmed,Reason="Confirm",TimeZone="Alaskan Standard Time",IDTimeSlot = "10"},
               new Calendar{ ID = "2", IDPageRequest= "JouleBroker", IDCustomer="2", Date=new DateTime(2019,08,30,2,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=40,Status=CalendarStatus.WaitConfirm,Reason="WaitConfirm", TimeZone= "Alaskan Standard Time",IDTimeSlot = "3"},
               new Calendar{ ID = "3", IDPageRequest= "JouleBroker", IDCustomer="2", Date=new DateTime(2019,08,30,4,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=50,Status=CalendarStatus.EmailSended,Reason="EmailSended", TimeZone= "Alaskan Standard Time",IDTimeSlot = "2"},
               new Calendar{ ID = "4", IDPageRequest= "JouleBroker", IDCustomer="1", Date=new DateTime(2019,08,30,6,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=50,Status=CalendarStatus.Decline,Reason="Decline", TimeZone = "Alaskan Standard Time",IDTimeSlot = "5"},
               new Calendar{ ID = "5", IDPageRequest= "MGISolutions", IDCustomer="2", Date=new DateTime(2019,08,30,8,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=50,Status=CalendarStatus.Completed,Reason="Completed", TimeZone= "Alaskan Standard Time",IDTimeSlot = "6"},
               new Calendar{ ID = "6", IDPageRequest= "JouleBroker", IDCustomer="1", Date=new DateTime(2019,08,28,6,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=50,Status=CalendarStatus.Confirmed,Reason="Developer Web", TimeZone= "Alaskan Standard Timee",IDTimeSlot = "4"},
               new Calendar{ ID = "7", IDPageRequest= "MGISolutions", IDCustomer="2", Date=new DateTime(2019,08,28,8,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=50,Status=CalendarStatus.WaitConfirm,Reason="Developer Web", TimeZone="Alaskan Standard Time",IDTimeSlot = "8"},
               new Calendar{ ID = "8", IDPageRequest= "MGISolutions", IDCustomer="2", Date=new DateTime(2019,08,28,9,30,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=50,Status=CalendarStatus.Decline,Reason="Front Web", TimeZone = "Alaskan Standard Time",IDTimeSlot = "9"},
               new Calendar{ ID = "9", IDPageRequest= "JouleBroker", IDCustomer="1", Date=new DateTime(2019,08,28,11,00,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=60,Status=CalendarStatus.WaitConfirm,Reason="Thinh Dev", TimeZone= "Alaskan Standard Time",IDTimeSlot = "7"},
               new Calendar{ ID = "10", IDPageRequest= "JouleBroker", IDCustomer="1", Date=new DateTime(2019,08,29,10,00,10),DateCreated = DateTime.Now,HourFrom=14,MinuteFrom=25,Duration=60,Status=CalendarStatus.Completed,Reason="Hieu Dev", TimeZone = "Alaskan Standard Time",IDTimeSlot = "10"},

            };
            lsCalendar.ForEach(t => context.CalendarDbSet.AddOrUpdate(e => e.ID, t));
            context.SaveChanges();
            #endregion

            #region Create data for Calendarpooling
            var calendarPooling = new List<CalendarPooling>
            {
                new CalendarPooling {ID = "1", IDCalendar="1", IDPageQuestion="1",Question="Lorem Ipsum is simply dummy y?", OrderQuestion= 1, Answer="web la 1 cai gi do "},
                new CalendarPooling {ID = "2", IDCalendar="1", IDPageQuestion="8",Question="Lorem Ipsum is simply dummy text  industry?", OrderQuestion= 2, Answer="framwork4 la gi day"},
                new CalendarPooling {ID = "3", IDCalendar="2", IDPageQuestion="7",Question="Lorem Ipsum is simply dummy text of the printing and typesetting industry?", OrderQuestion= 3, Answer="framwork4 la gi day!!!"},
            };

            calendarPooling.ForEach(t => context.CalendarPoolingDbSet.AddOrUpdate(e => e.ID, t));
            context.SaveChanges();
            #endregion

            #region Create data for TimeSlot
            var lstimeSlot = new List<TimeSlot>
            {
                new TimeSlot{ ID = "1", DateStart = new DateTime(2019,08,04,10,00,00), Hour = 0, Minute = 0, Duration = 90, Note = "When life gives you a hundred reasons to cry, show life that you have a thousand reasons to smile", IDAppUser = ""},
                new TimeSlot{ ID = "2", DateStart = new DateTime(2019,08,05,10,30,00), Hour = 0, Minute = 0, Duration = 60, Note = "Sometimes people are beautiful. Not in looks. Not in what they say. Just in what they are", IDAppUser = ""}
            };
            lstimeSlot.ForEach(t => context.TimeSlotDbSet.AddOrUpdate(e => e.ID, t));
            context.SaveChanges();
            #endregion
        }
    }
}
