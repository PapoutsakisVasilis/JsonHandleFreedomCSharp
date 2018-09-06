using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonHandleFreedomLibrary
{
    class JsonEncodeMaster
    {

        public ObjectReturn createJsonObj(Object val, string type)
        {

            Assembly assem = Assembly.GetExecutingAssembly();
            Type theT = assem.GetType(type);
            var fields = theT.GetFields().ToList();
            return new ObjectReturn();
        }
    }
}
