using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace JsonHandleFreedomLibrary
{
    class ObjectReturn
    {
        public dynamic returnObj;

        public ObjectReturn()
        {

            this.returnObj = new ExpandoObject();
            this.returnObj.error = 0;

        }


    }
}
