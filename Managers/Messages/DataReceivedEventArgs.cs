using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Messages
{
    public class DataReceivedEventArgs : EventArgs
    {
        public bool IsCompleted { get; set; }
    }
}
