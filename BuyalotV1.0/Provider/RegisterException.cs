using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.Provider
{
    public class RegisterException:  Exception
    {
        public RegisterException(string errMessage)
            :base (errMessage)
        {
        }
    }
}