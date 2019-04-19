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

         /// <summary>
         /// Gives an empty dictionnaray containing one entry for each dynamic property associated to the type
         /// </summary>
         /// <param name="IsRequired">indicates if properties have to be required or not=> 
         /// <BR></BR>null : all dynamic properties<BR></BR>true: only required properties<BR></BR>false: only NOT required properties</param>
         /// <returns></returns>
         Dictionary<string, object> GetEmptyValueList();
    }
}
