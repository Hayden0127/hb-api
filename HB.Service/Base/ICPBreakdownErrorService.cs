using HB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Service
{
    public interface ICPBreakdownErrorService
    {
        BreakdownErrorDetails GetErrorListByCPId(int id);
        List<CPBreakdownDurationDetailsModel> GetBreakdownDurationByCPId(int id);
    }
}
