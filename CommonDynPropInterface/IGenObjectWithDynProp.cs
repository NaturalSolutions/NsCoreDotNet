using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{

    /// <summary>
    /// Interface definining one objet containing DYnamic properties
    /// </summary>
    /// <typeparam name="P"> P is the Type of the object</typeparam>
    public interface IGenObjectWithDynProp<P> where P : IGenType 
    {
        
        long ID {get;set;}
        P TypeObj { get; }
        string Name {get;set;}
       
        
    }
}
