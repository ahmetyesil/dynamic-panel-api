using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchNet
{
    public interface IBaseObject
    {
        string Id { get; set; }
        string Rev { get; set; }
    }
}