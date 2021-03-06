﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using CommonDynPropInterface;
using System.Reflection;
using System.ComponentModel;
using NS.Logs;
using NS.Logs.DbConnexion;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;
using CommonDynPropManager;
using NSConfManager;


namespace CommonDynPropManager
{

    


    /// <summary>
    /// Asbtract class to define objets with Dynamic properties
    /// </summary>
    public abstract class CommonObjectWithDynProp
    {
        #region "Privates Properties"
        /// <summary>
        /// Current values of dynamic properties
        /// </summary>
        protected DynPropList _PropDynValuesOfNow;


       

        /// <summary>
        /// Get the type of objectwithdynprops
        /// </summary>
        protected abstract IGenType CurType { get; }



        /// <summary>
        /// Accessor for dynpropvaluesNow
        /// </summary>
        protected DynPropList PropDynValuesOfNow { get { if (_PropDynValuesOfNow == null) LoadNowValues(); return _PropDynValuesOfNow; } }

        protected abstract List<string> allowedTables { get; }

        #endregion


        /// <summary>
        /// Get all current values in a dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetProperties(bool forDTO = false)
        {
            return PropDynValuesOfNow.ToDictionary(p => p.Key.Replace("@", "").ToLower(), p => GetRealValue(p.Value,forDTO));
        }

        #region "Constructors


        /// <summary>
        /// Basic Constructor
        /// </summary>
        public CommonObjectWithDynProp()
        {
            // this.LoadNowValues();
        }
        #endregion




        #region "Asbtract Methods"
        /// <summary>
        /// Load the current values in _PropDynValuesOfNow
        /// </summary>
        public abstract void LoadNowValues();


        /// <summary>
        /// Get ValuesNow for existing DynPropList
        /// </summary>
        /// <param name="MyList"></param>
        public void LoadNowValues(DynPropList MyList)
        {
            _PropDynValuesOfNow = MyList;
        }


        /// <summary>
        /// Affect the Context information
        /// </summary>
        /// <param name="InMyContext"></param>
        public abstract void SetContext(DbContext InMyContext);


        /// <summary>
        /// Get one value from database with a specific date
        /// </summary>
        /// <param name="DynPropName">Property Name</param>
        /// <param name="DateValeur">Refrence Date for the value</param>
        /// <returns></returns>
        protected abstract IGenDynPropValue GetValueFromDB(string DynPropName, DateTime DateValeur);

        /// <summary>
        /// Record one new value in DB
        /// </summary>
        /// <param name="DynPropName">Property Name</param>
        /// <param name="DynPropValue">The value</param>
        /// <param name="DateValeur">StartDate of the new value</param>
        protected abstract IGenDynPropValue SetValueInDB(string DynPropName, object DynPropValue, DateTime DateValeur);
        protected abstract IGenDynPropValue SetValueInDB(long DynPropId, object DynPropValue, DateTime DateValeur);

        /// <summary>
        /// Update a complex Data from a Value
        /// </summary>
        /// <param name="DataKey"></param>
        /// <param name="Data"></param>
        protected virtual void UpdateData(string DataKey, object Data)
        {
            // default do nothing
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract DbContext ObjContext { get; }


        /// <summary>
        /// When a complex type has to be serialized, this function will be called from the herited class
        /// </summary>
        /// <param name="PropName">Name of the property to be serialized</param>
        /// <param name="Resultat">DIctonnaray containing the resultat, the value would be added on the Key "PropName"</param>
        public abstract void AddDTOInList(string PropName, Dictionary<string, object> Resultat);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<IGenType_DynProp> GetLinkedDynProps();


        public abstract object GetEntryByName(string EntryName);


        #endregion

        #region "Méthodes héritables"





        /// <summary>
        /// Set Current value to a property
        /// </summary>
        /// <param name="DynPropName"></param>
        /// <param name="DynPropValue"></param>
        protected void SetValue(string DynPropName, object DynPropValue)
        {

            DateTime Now = DateTime.Now;
            _PropDynValuesOfNow[DynPropName] = SetValueInDB(DynPropName, DynPropValue, Now);

        }

        #endregion

        #region "Methode privées non-overwritable"

        /// <summary>
        /// Get current value of a dynamic property
        /// </summary>
        /// <param name="DynPropName"></param>
        /// <returns></returns>
        protected IGenDynPropValue GetValue(string DynPropName)
        {
            return PropDynValuesOfNow[DynPropName];
        }

        /// <summary>
        /// Set  value to a property at a specific date
        /// </summary>
        /// <param name="DynPropName"></param>
        /// <param name="DynPropValue"></param>
        /// <param name="DateValeur"></param>
        protected void SetValue(string DynPropName, object DynPropValue, DateTime DateValeur)
        {
            SetValueInDB(DynPropName, DynPropValue, DateValeur);
        }


        /// <summary>
        /// Get real value regarding to its type
        /// </summary>
        /// <param name="MaValeur"></param>
        /// <returns></returns>
        protected object GetRealValue(IGenDynPropValue MaValeur,bool isForDTO = false)
        {
            return DynPropList.GetRealValue(MaValeur,isForDTO);


        }
        /// <summary>
        /// set real value regarding to its type
        /// </summary>
        /// <param name="MaDynProp"></param>
        /// <param name="TypeValue"></param>
        /// <param name="MaValeur"></param>
        protected void SetRealValue(IGenDynPropValue MaDynProp, string TypeValue, object MaValeur)
        {
            DynPropList.SetRealValue(MaDynProp, TypeValue, MaValeur);
        }

        #endregion


        #region "Méthodes publiques"
        /// <summary>
        /// Get/Set Curent Value of one dynamic property
        /// </summary>
        /// <param name="DynPropName"></param>
        /// <returns></returns>
        public object this[string DynPropName]
        {
            get
            {
                if (_PropDynValuesOfNow.ContainsKey(DynPropName))
                {
                    return GetRealValue(_PropDynValuesOfNow[DynPropName]);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                SetValue(DynPropName, value);
            }
        }



        /// <summary>
        ///  Get/Set Curent  of one dynamic property at a specific date
        /// </summary>
        /// <param name="DynPropName"></param>
        /// <param name="DateValeur"></param>
        /// <returns></returns>
        public object this[string DynPropName, DateTime DateValeur]
        {
            get
            {
                return GetRealValue(GetValueFromDB(DynPropName, DateValeur));
            }
            set
            {
                SetValue(DynPropName, value, DateValeur);
            }
        }

        #endregion

        /// <summary>
        /// Get a dictionary including static and dynamic values 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetDTO(bool IsFromList = false)
        {
            Dictionary<string, object> Resultat = new Dictionary<string, object>();
            
            foreach (var val in this.GetDTOForProperties())
            {
                if (!Resultat.Keys.Contains(val.Key))
                    Resultat.Add(val.Key, val.Value);
            }

            return Resultat;
        }

        public virtual Dictionary<string, object> GetDTOForProperties(Dictionary<string, object> toret = null, PropertyInfo[] Props = null)
        {
            bool defaultCase = false;
            if (toret == null && Props == null)
                defaultCase = true;

            if (toret == null)
                toret = this.GetProperties(true);
            if (Props == null)
                Props = this.GetType().GetProperties();

            object Mavaleur;
            for (int i = 0; i < Props.Length; i++)
            {
                if (Props[i].Name.ToLower() != "item" && !Props[i].PropertyType.FullName.Contains("ICollection"))// Only if not a collection
                {
                    if (!Props[i].PropertyType.FullName.Contains("ECollection"))// if standard type
                    {
                        if (Props[i].Name.ToLower() == "status")
                        {// If status get the status nd statusName
                            Mavaleur = ObjContext.Entry(this).Property(Props[i].Name).CurrentValue;
                            if (toret.Where(item => item.Key == "status").ToList().Count == 0)
                            {
                                toret.Add("status", (Status)Mavaleur);
                                toret.Add("statusname", StatusManager.GetStatusName((Status)Mavaleur, "EN"));
                            }
                        }
                        else
                        {
                            Mavaleur = ObjContext.Entry(this).Property(Props[i].Name).CurrentValue;
                            if (Props[i].PropertyType.FullName == "System.DateTime")
                            {
                                if ((DateTime)Mavaleur == DateTime.MinValue)
                                {
                                    Mavaleur = null;
                                }
                                else
                                {

                                    Mavaleur = ((DateTime)Mavaleur).ToString("dd/MM/yyyy");
                                }
                            }
                            if (toret.Keys.Contains(Props[i].Name.ToLower()))
                            {
                                if (toret[Props[i].Name.ToLower()] == null)
                                    toret[Props[i].Name.ToLower()] = Mavaleur;
                            }
                            else
                                toret.Add(Props[i].Name.ToLower(), Mavaleur);
                        }

                    }
                    else
                    {// If complex Type
                        if (Props[i].Name == "TypeObj" && defaultCase)
                        {
                            Mavaleur = ((IGenType)Props[i].GetValue(this, null)).ID;
                            toret.Add(Props[i].Name.ToLower(), Mavaleur);
                            Mavaleur = ((IGenType)Props[i].GetValue(this, null)).Name;
                            toret.Add(Props[i].Name.ToLower() + "name", Mavaleur);
                        }
                        else
                        {
                            this.AddDTOInList(Props[i].Name, toret);
                        }
                    }
                }
            }

            return (toret);
        }

        /// <summary>
        /// Gives an empty dictionnaray constaining one entry for each dynamic property associated to the ObjType of current objet
        /// </summary>
        /// <param name="IsRequired">indicates if properties have to be required or not=> 
        /// <BR></BR>null : all dynamic properties<BR></BR>true: only required properties<BR></BR>false: only NOT required properties</param>
        /// <returns></returns>
        public Dictionary<string, object> GetEmptyValueList()
        {
            return this.CurType.GetEmptyValueList();
        }



        

        /// <summary>
        /// Update one object from Dictionary key/value
        /// </summary>
        /// <param name="MyData">Values</param>
        /// <param name="UpdateStaticFields">True if static Fields have to be updated</param>
        /// <param name="UpdateDynamicFields">True idf ynamic fields have to be updated</param>
        public virtual void UpdateFromDic(Dictionary<string, object> MyData, bool UpdateStaticFields = true, bool UpdateDynamicFields = true)
        {
            List<string> StaticFields = this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Select(s => s.Name).ToList();

            Dictionary<string, object> DynFields = this.CurType.GetEmptyValueList();

            foreach (string DataKey in MyData.Keys.Where(k => k.ToLower() != "id" && k.ToLower() != "typeobj"))
            {
                this.SetPropValue(DataKey, MyData[DataKey]);
                
                /* MAYBE USELESS ? SetPropValue does the job !
                 
                int i = StaticFields.FindIndex(s => s.ToLower() == DataKey.ToLower());
                if (i > -1)
                {// if static Field, affect field
                    if (UpdateStaticFields)
                    {
                        if (MyData[DataKey] != null && !string.IsNullOrEmpty(MyData[DataKey].ToString()))
                        {
                            try
                            {
                                var CurrentValue = ObjContext.Entry(this).CurrentValues[StaticFields[i]];
                                Type CurrentType = this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(s => s.Name.ToLower() == DataKey.ToLower()).FirstOrDefault().PropertyType;
                                object MyParsedData;
                                if (CurrentType.FullName.ToLower() == "system.datetime")
                                {
                                    try
                                    {
                                        MyParsedData = DateTime.ParseExact(MyData[DataKey].ToString().Replace(" ", " "), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        int n;
                                        if (MyData[DataKey].ToString().Length == 4 && int.TryParse(MyData[DataKey].ToString(), out n))
                                        {
                                            MyData[DataKey] = "01/01/" + MyData[DataKey].ToString();
                                        }
                                        MyParsedData = DateTime.ParseExact(MyData[DataKey].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }
                                }
                                else
                                {
                                    MyParsedData = TypeDescriptor.GetConverter(CurrentType).ConvertFrom(MyData[DataKey].ToString());
                                }
                                if (CurrentValue == null)
                                {
                                    if (string.IsNullOrEmpty(MyData[DataKey].ToString()))
                                    {// nothing to do both null 
                                    }
                                    else
                                    {
                                        ObjContext.Entry(this).CurrentValues[StaticFields[i]] = MyParsedData;
                                    }
                                }
                                else
                                {
                                    if (!CurrentValue.Equals(MyParsedData))
                                    {
                                        ObjContext.Entry(this).CurrentValues[StaticFields[i]] = MyParsedData;
                                    }
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // complex type
                                this.UpdateData(DataKey, MyData[DataKey]);
                            }
                        }
                    }// end if UpdateStaticFields
                }// end if static
                else
                {
                    try
                    {
                        if (UpdateDynamicFields)
                        {
                            if (MyData[DataKey] == null || string.IsNullOrEmpty(MyData[DataKey].ToString()))
                            {

                            }
                            else
                            {
                                if (!CompareValues(DataKey, MyData[DataKey]))
                                {
                                    this[DataKey] = MyData[DataKey];
                                }
                            }
                        }
                    }
                    catch
                    {
                        // nothing to do form the moment
                        //TODO ADD WARNING
                    }
                } /**/
            }
        }

        public void UpdateChampsLies()
        {            
            IQueryable<IGenType_DynProp> MyLinkedDynProp = GetLinkedDynProps();
            SqlCnx MyConn = new SqlCnx(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            foreach (IGenType_DynProp typeDynProp in MyLinkedDynProp)
            {
                var valeur = this[typeDynProp.DynProp.Name];
                if (valeur != null && valeur.GetType().Name.Substring(0, 4).ToLower() == "list")
                {
                    valeur = JsonConvert.SerializeObject(valeur);
                }
                string sourceId = typeDynProp.LinkSourceID;
                object sourceIdValeur;
                if (sourceId.Substring(0, 3).ToLower() == "@s!")
                {
                    string[] ValDecompese = sourceId.Substring(3).Split('.');
                    if (ValDecompese.Length == 1)
                    {
                        // propriétés directe de l'entité
                        sourceIdValeur = ObjContext.Entry(this).CurrentValues[sourceId.Substring(3)];
                    }
                    else
                    {
                        // propriétés d'un entité dépendante
                        object MyEntry = GetEntryByName(ValDecompese[0]);
                        sourceIdValeur = ObjContext.Entry(MyEntry).CurrentValues[ValDecompese[1]];
                    }
                }
                else
                {
                    sourceIdValeur = this[sourceId.Substring(3)];
                }
                

                string requete;
                if (allowedTables.Contains(typeDynProp.LinkedTable, StringComparer.CurrentCultureIgnoreCase))
                {
                    if (valeur != null && !String.IsNullOrEmpty(valeur.ToString()))
                    {
                        if (typeDynProp.LinkedField.Length >= 5 && typeDynProp.LinkedField.Substring(0, 5) == "@Dyn:")
                        {
                            // Pour l'instant gestion des dest string uniquement

                            requete = "select ValueString from " + typeDynProp.LinkedTable + "DynPropValues V JOIN " + typeDynProp.LinkedTable +
                                "DynProps P ON V." + typeDynProp.LinkedTable + "DynProp_ID = P.ID ";
                            requete += "WHERE V." + typeDynProp.LinkedTable + "_ID = @id ";
                            requete += "AND P.Name ='" + typeDynProp.LinkedField.Substring(5) + "' ";
                            DataTable Retour = MyConn.GetDataTableFromCnxWithArgs(requete, new object[2] { "@id", sourceIdValeur });
                            Console.WriteLine(requete);
                            if (Retour.Rows.Count >= 1)
                            { // Il existe une valeur à la même date
                                if (Retour.Rows[0][0].ToString() == valeur.ToString())
                                {
                                    // même valeur, on ne fait rien
                                }
                                else
                                {
                                    requete = "UPDATE V SET StartDate = GETDATE(), ValueString=@val from " + typeDynProp.LinkedTable + "DynPropValues V JOIN " + typeDynProp.LinkedTable +
                                        "DynProps P ON V." + typeDynProp.LinkedTable + "DynProp_ID = P.ID ";
                                    requete += "WHERE V." + typeDynProp.LinkedTable + "_ID = @id ";
                                    requete += "AND P.Name ='" + typeDynProp.LinkedField.Substring(5) + "'";
                                    object[] Params = new object[4] { "@val", valeur, "@id", sourceIdValeur};
                                    MyConn.ExecuteQueryWithArgs(requete, Params);
                                    Console.WriteLine(requete);
                                }
                            }
                            else
                            {
                                /* OLD, MAYBE BAD ? 
                                requete = "INSERT INTO " + typeDynProp.LinkedTable + "DynPropValues  ( GETDATE(),[ValueString]," + typeDynProp.LinkedTable + "_ID";
                                requete += "," + typeDynProp.LinkedTable + "DynProp_ID)";
                                requete += " select @val,S.ID,p.ID from " + typeDynProp.LinkedTable + "s S JOIN " + typeDynProp.LinkedTable + "DynProps P ON p.Name ='" + typeDynProp.LinkedField.Substring(5) + "'";
                                requete += " WHERE S." + typeDynProp.LinkedID + " = @id";
                                */

                                requete = "INSERT INTO " + typeDynProp.LinkedTable + "DynPropValues  ( StartDate,[ValueString]," + typeDynProp.LinkedTable + "_ID";
                                requete += "," + typeDynProp.LinkedTable + "DynProp_ID)";
                                requete += " select GETDATE(),@val,S.ID,p.ID from " + typeDynProp.LinkedTable + "s S JOIN " + typeDynProp.LinkedTable + "DynProps P ON p.Name ='" + typeDynProp.LinkedField.Substring(5) + "'";
                                if (typeDynProp.LinkedField.Substring(5).ToLower() == "container")
                                {
                                    Console.WriteLine("yo !");
                                }
                                requete += " WHERE S." + typeDynProp.LinkedID + " = @id";
                                object[] Params = new object[4] { "@val", valeur, "@id", sourceIdValeur};
                                MyConn.ExecuteQueryWithArgs(requete, Params);
                                Console.WriteLine(requete);
                            }
                        }
                        else
                        {
                            // TODO : TMP, UGLY, SHOULD FIND A WAY THROUGH THE CONF
                            requete = "UPDATE " + typeDynProp.LinkedTable + (typeDynProp.LinkedTable == "Sample" ? "s" : "" ) + " SET " + typeDynProp.LinkedField + " = @val where " + typeDynProp.LinkedID + " = @id";
                            //LogManager.ZeLogger.SendNotice(LogDomaine.WebApplication, requete);
                            //LogManager.ZeLogger.SendNotice(LogDomaine.WebApplication, ObjContext.Database.Connection.ConnectionString);
                            object[] Params = new object[4] { "@val", valeur, "@id", sourceIdValeur };
                            MyConn.ExecuteQueryWithArgs(requete, Params);
                            //SQLDirect.ExecuteSQL(requete, Params, ((SqlConnection)ObjContext.Database.Connection));
                            Console.WriteLine(requete);
                        }
                    }
                    else
                    {
                        LogManager.ZeLogger.SendNotice(LogDomaine.WebApplication, "Empty value for UpdateChampsLies(). LinkedField : " + typeDynProp.LinkedField + "; LinkedTable : " + typeDynProp.LinkedTable);
                    }
                }
                else
                {
                    throw new Exception("Wrong LinkedTable value for UpdateChampsLies(). LinkedField : " + typeDynProp.LinkedField + "; LinkedTable : " + typeDynProp.LinkedTable);
                }
            }
        }


        /// <summary>
        /// Compare one OldValue with new one 
        /// </summary>
        /// <param name="DataKey"></param>
        /// <param name="NewValeur"></param>
        /// <returns>True if identicial, False otherwise</returns>
        protected bool CompareValues(string DataKey, object NewValeur)
        {
            if (!this._PropDynValuesOfNow.ContainsKey(DataKey))
            {
                return (String.IsNullOrEmpty(NewValeur.ToString()));

            }

            if (this._PropDynValuesOfNow[DataKey].DynProp.TypeProp != "list")
                return this[DataKey].Equals(NewValeur);
            
            List<object> NewValeurList = JsonConvert.DeserializeObject<List<object>>(NewValeur.ToString());
            List<object> curValeur = (List<object>)this[DataKey];
            if (NewValeurList.Count != curValeur.Count) return false;
            for (int i = 0; i < curValeur.Count; i++)
            {
                if (NewValeurList[i].ToString() != curValeur[i].ToString()) return false;
            }
            return true;

        }

        public abstract bool SetPropValue(string propName, dynamic propValue);
        public abstract dynamic GetPropValue(string propName, bool objectAsContext);
    }
}

