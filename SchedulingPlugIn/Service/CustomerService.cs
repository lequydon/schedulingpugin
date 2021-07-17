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
    public class CustomerService : ICustomerService
    {
        private IDALContext context;
        public CustomerService( IDALContext dal)
        {
            context = dal;
        }
        public CustomerService()
        {
            context = new DALContext();
        }
        public CODE_RESULT_RETURN Create(Customer t)
        {
            var customer = new Customer { ID = t.ID, FirstName = t.FirstName, LastName = t.LastName, Email = t.Email,Phone=t.Phone };
            context.Customers.Create(customer);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        public Customer Get(string id)  
        {
            var customer = context.Customers.Filter(c => c.ID == id).FirstOrDefault();
            return customer;
        }

        public List<Customer> GetList()
        {
            return context.Customers.All().ToList();
        }
        public CODE_RESULT_RETURN Delete(Customer t)
        {
            //var customer = context.Customers.Filter(e => e.ID == t.ID).FirstOrDefault();
            context.Customers.Delete(e=>e.ID == t.ID);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }   
      
        public CODE_RESULT_RETURN Update(Customer t)
        {
            var customer = context.Customers.Find(t.ID);
           // customer.FirstName = t.FirstName;
           // customer.LastName = t.LastName;
           // customer.Email = t.Email;
            customer.Phone = t.Phone;
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
          public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
        public CODE_RESULT_RETURN CheckEmail(string email)
        {
            var customerEmail = context.Customers.Find(e => e.Email == email);
            if (customerEmail == null) return CODE_RESULT_RETURN.ThanhCong;
            else return CODE_RESULT_RETURN.EmailTrung;
        }
        public int CountID()
        {
            int i = context.Customers.All().Count();
            return i;
        }
        public string getIDCustomerFromEmail(string email)
        {
            var customer = context.Customers.Find(e => e.Email == email);
            return customer.ID;
        }
    }
}