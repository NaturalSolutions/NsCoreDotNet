using CommonDynPropInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropManager
{


    /// <summary>
    /// Container of DYnamic property values
    /// </summary>
    public class DynPropList : Dictionary<string, IGenDynPropValue>
    {
        /// <summary>
        /// Basic constructor
        /// </summary>
        public DynPropList()
            : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Get all the actual values
        /// </summary>
        public Dictionary<string, object> Values { get { return this.ToDictionary(p => p.Key, p => GetRealValue(p.Value),StringComparer.CurrentCultureIgnoreCase); } }


        /// <summary>
        /// Get one RealValue regarding to its type
        /// </summary>
        /// <param name="MyValue"></param>
        /// <returns></returns>
        public static object GetRealValue(IGenDynPropValue MyValue, bool isForDTO = false)
        {
            switch (MyValue.DynProp.TypeProp.ToLower())
            {
                case "entier":
                    return MyValue.ValueInt;
                case "date":
                    if (isForDTO)
                    {
                        if (MyValue.ValueDate == null)
                        {
                            return "";
                        }
                        else
                        {

                            return ((DateTime) MyValue.ValueDate).ToString("dd/MM/yyyy HH:mm:ss");
                        }
                    }
                    else
                    {
                        return MyValue.ValueDate;
                    }

                case "float":
                    return MyValue.ValueFloat;
                case "string":
                    return MyValue.ValueString;
                case "list":
                    return JsonConvert.DeserializeObject<List<object>>(MyValue.ValueString);
                default:
                    return MyValue.ValueString;
            }

        }


        /// <summary>
        /// Set one RealValue regarding to its type
        /// </summary>
        /// <param name="MaDynProp"></param>
        /// <param name="TypeValue"></param>
        /// <param name="MaValeur"></param>
        public static void SetRealValue(IGenDynPropValue MaDynProp, string TypeValue, object MaValeur)
        {

            MaDynProp.ValueInt = null;
            MaDynProp.ValueDate = null;
            MaDynProp.ValueFloat = null;
            MaDynProp.ValueString = null;
            if (MaValeur == null) return;


            switch (TypeValue.ToLower())
            {
                case "entier":
                    MaDynProp.ValueInt = long.Parse(MaValeur.ToString());
                    break;
                case "date":
                    MaDynProp.ValueDate = DateTime.Parse(MaValeur.ToString());
                    break;
                case "float":
                    MaDynProp.ValueFloat = decimal.Parse(MaValeur.ToString());
                    break;
                case "string":

                    MaDynProp.ValueString = MaValeur.ToString();
                    break;
                case "list":
                    // TODO enlver les caractères en trop
                    MaDynProp.ValueString = MaValeur.ToString();
                    break;
                default:
                    MaDynProp.ValueString = (string) MaValeur;
                    break;

            }
        }

    }
}
