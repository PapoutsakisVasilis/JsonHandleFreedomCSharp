using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace JsonHandleFreedomLibrary
{
    class JsonDecodeMaster
    {
        public Object theObj;
        public string jsonObj;




        public ObjectReturn getTheObj(/*Object theObj*/dynamic obj, string type, bool bindStat = false)
        {
            JObject results = null;
            //string sampleJson = "{\"mainA\":\"name1\",\"mmD\":{\"de\":\"[1,2,3]\"}}";

            // Parse JSON into dynamic object, convenient!

            try
            {
                if (Helpers.IsJson(obj))
                {
                    try
                    {
                        results = JObject.Parse(obj);
                    }
                    catch (Exception ex)
                    {
                        ObjectReturn res = new ObjectReturn();
                        res.returnObj.type = obj.GetType().ToString();
                        res.returnObj.value = obj;
                        res.returnObj.error = ex.ToString();
                        return res;

                    }
                }
                else if (obj.Type == JTokenType.Object)
                {
                    results = obj;

                }
                else
                {
                    ObjectReturn res = new ObjectReturn();
                    res.returnObj.type = obj.GetType().ToString();
                    res.returnObj.value = obj;
                    return res;
                }
            }
            catch (Exception ex)
            {
                ObjectReturn res = new ObjectReturn();
                res.returnObj.type = obj.GetType().ToString();
                res.returnObj.value = obj;
                return res;

            }

            if (results.Type == JTokenType.Array && bindStat == false)
            {
                foreach (var item in results)
                {
                    //string transporter = JsonConvert.SerializeObject();
                    ObjectReturn ans = getTheObj(item.Value, type);
                    if (ans.returnObj.error == 0)
                    {
                        results[item.Key] = ans.returnObj.value.Value;
                    }

                }

                ObjectReturn returnVals = new ObjectReturn();
                returnVals.returnObj.value = results;
                return returnVals;
            }
            else if (results.Type == JTokenType.Object && bindStat == true)
            {
                Assembly assem = Assembly.GetExecutingAssembly();
                Type theT = assem.GetType(type);
                var fields = theT.GetFields().ToList();

                Object theFinal = assem.CreateInstance(type);
                foreach (var item in fields)
                {

                    foreach (var itemJson in results)
                    {
                        if (item.Name.ToString() == itemJson.Key.ToString())
                        {
                            FieldInfo prop = theT.GetField(item.Name);
                            Type ttype = theT.GetField(item.Name).FieldType;

                            // Set the value of the given property on the given instance
                            ObjectReturn ans = getTheObj(itemJson.Value, type);
                            if (ans.returnObj.error == 0)
                            {

                                prop.SetValue(theFinal, Convert.ChangeType(ans.returnObj.value.Value, ttype));
                            }


                        }
                    }


                    //Console.WriteLine(item.Name.ToString());
                }

                FieldInfo[] fieldsPrivate = theT.GetFields(
                         BindingFlags.NonPublic |
                         BindingFlags.Instance);



                foreach (var itemPrivate in fieldsPrivate)
                {
                    Console.WriteLine(itemPrivate.Name.ToString());
                    if (results.ContainsKey("FrPrivate"))
                    {
                        if (results.GetValue("FrPrivate").Type == JTokenType.Array)
                        {
                            foreach (var item in (JObject)results.GetValue("FrPrivate"))
                            {
                                if (itemPrivate.Name.ToString() == item.Key.ToString())
                                {
                                    ObjectReturn privVal = getTheObj(item.Value, type);
                                    if (privVal.returnObj.error == 0)
                                    {

                                        itemPrivate.SetValue(theFinal, Convert.ChangeType(privVal.returnObj.value.Value, itemPrivate.FieldType));
                                    }
                                }
                            }

                        }
                        else
                        {
                            foreach (var itemPrive in (JObject)results.GetValue("FrPrivate"))
                            {
                                if (itemPrive.Key == itemPrivate.Name.ToString())
                                {
                                    ObjectReturn privVal = getTheObj(itemPrive.Value, type);
                                    if (privVal.returnObj.error == 0)
                                    {


                                        itemPrivate.SetValue(theFinal, Convert.ChangeType(privVal.returnObj.value.Value, itemPrivate.FieldType));
                                    }
                                }
                            }

                        }


                    }

                }

                FieldInfo[] fieldsStatic = theT.GetFields(
                        BindingFlags.Static |
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var itemStatic in fieldsStatic)
                {

                    if (results.ContainsKey("FrStatic"))
                    {
                        if (results.GetValue("FrStatic").Type == JTokenType.Array)
                        {
                            foreach (var item in (JObject)results.GetValue("FrStatic"))
                            {
                                if (itemStatic.Name.ToString() == item.Key.ToString())
                                {
                                    ObjectReturn privVal = getTheObj(item.Value, type);
                                    if (privVal.returnObj.error == 0)
                                    {
                                        itemStatic.SetValue(theFinal, privVal.returnObj.value.Value);
                                    }
                                }
                            }

                        }
                        else
                        {
                            foreach (var itemsta in (JObject)results.GetValue("FrStatic"))
                            {
                                Console.WriteLine(itemsta.Key.ToString());
                                if (itemsta.Key == itemStatic.Name.ToString())
                                {
                                    ObjectReturn privVal = getTheObj(itemsta.Value, type);
                                    if (privVal.returnObj.error == 0)
                                    {


                                        itemStatic.SetValue(theFinal, Convert.ChangeType(privVal.returnObj.value.Value, itemStatic.FieldType));
                                    }
                                }
                            }




                        }


                    }

                }




                ObjectReturn finaler = new ObjectReturn();
                finaler.returnObj.type = "final";
                finaler.returnObj.value = theFinal;

                return finaler;

            }

            ObjectReturn finalerError = new ObjectReturn();
            finalerError.returnObj.type = "final";
            finalerError.returnObj.value = null;
            finalerError.returnObj.error = 1;
            return finalerError;


        }


        public void ManageArray(JToken val)
        {

        }





    }
}
