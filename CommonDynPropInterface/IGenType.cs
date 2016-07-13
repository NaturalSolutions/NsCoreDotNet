using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{



   




    /// <summary>
    /// Represents a type of IObjectWithDynProp
    /// </summary>
    public interface IGenType
    {
        /// <summary>
        /// 
        /// </summary>
         long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
         string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
         Status Status { get; set; }

       
       

    }
}
