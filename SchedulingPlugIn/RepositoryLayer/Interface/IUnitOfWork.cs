using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.RepositoryLayer.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}