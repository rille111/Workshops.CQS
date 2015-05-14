using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqs.Infrastructure
{
    /// <summary>
    ///  Right now only works as a marker interface, probably for Models that are Entities that can be stored somewhere.
    ///  Might be unnecessary. Not sure yet.
    /// </summary>
    public interface IEntity
    {
        // TODO: Is is possible to remove the forcing of having Id in IEntity?
        int Id { get; set; }
    }
}
