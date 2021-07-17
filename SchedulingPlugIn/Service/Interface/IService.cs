using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Service.Interface
{
    public interface IService<T> : IDisposable
    {
        List<T> GetList();
        T Get(string id);
        CODE_RESULT_RETURN Create(T t);
        CODE_RESULT_RETURN Update(T t);
        CODE_RESULT_RETURN Delete(T t);
    }
    public enum CODE_RESULT_RETURN
    {
        ThanhCong = 1,
        MaTrung = -1,
        TenTrung = -2,
        EmailTrung = -3,
        IDTimeSlotTrung = -4,
        ThatBai = 0
    }
}