using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.SmartSD.Integrator
{
    public class SmartSDException: Exception
    {
        public SmartSDException(string ex) : base(ex) { }
    }
}
