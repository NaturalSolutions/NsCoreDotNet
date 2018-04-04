using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{

    /// <summary>
    /// INterface Defining one dated Value on a dynamic property
    /// </summary>
    public interface IGenDynPropValue
    {
        /// <summary>
        /// 
        /// </summary>
        long ID { get; set; }
        /// <summary>
        /// Date the value start to be effective
        /// </summary>
        DateTime StartDate { get; set; }
        IGenDynProp  DynProp { get;}
        /// <summary>
        /// If DynProp Type is Entier, value will be recorded there
        /// </summary>
        Nullable<long> ValueInt { get; set; }
        /// <summary>
        /// If DynProp Type is String, value will be recorded there
        /// </summary>
        string ValueString { get; set; }
        /// <summary>
        /// If DynProp Type is Date, value will be recorded there
        /// </summary>
        Nullable<DateTime> ValueDate { get; set; }
        /// <summary>
        /// If DynProp Type is Float, value will be recorded there
        /// </summary>
        Nullable<decimal> ValueFloat { get; set; }

        string ValueThesaurus { get; set; }
    }
}
