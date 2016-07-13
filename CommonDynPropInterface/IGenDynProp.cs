using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{
    /// <summary>
    /// Interface defining one Dynamic property
    /// </summary>
    public interface IGenDynProp
    {
         long ID { get; set; }
         string Name { get; set; }
         string TypeProp { get; set; }
    }
}
