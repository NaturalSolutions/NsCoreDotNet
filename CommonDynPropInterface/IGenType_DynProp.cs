using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{

    /// <summary>
    /// Represents the link beetween a type and its dynamic properties
    /// </summary>
    public interface IGenType_DynProp
    {
        /// <summary>
        /// 
        /// </summary>
         long ID { get; set; }
        /// <summary>
        /// 0 if no recurence, else the minimum number of occurence
        /// </summary>
        
        /// <summary>
        /// Associated dynamic property
        /// </summary>
         IGenDynProp  DynProp { get; }
        /// <summary>
        /// Associated Type
        /// </summary>
         IGenType ObjType { get;  }

         string LinkedTable { get; set; }
         string LinkedField { get; set; }
         string LinkedID { get; set; }
         string LinkSourceID { get; set; }


    }
}
