using DNP3.DNP3Functions;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP3
{
    public class DNP3FunctionFactory
    {
        public static IDNP3Functions CreateDNP3Function(DNP3ApplicationObjectParameters objectParameters)
        {
            //Ovde se vracaju konkretne funkcije (klase) sto nasledjuju ovaj interfejs i u njima je popunjen niz bajtova...
            throw new NotImplementedException();
        }
    }
}
