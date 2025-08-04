using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.WinForms.DataObjects
{
    internal class TimeAccountSummaryGridEntry
    {
        [DisplayName("Time Account")]
        public string TimeAccountName { get; }
        public TimeSpan Duration { get; }

        public TimeAccountSummaryGridEntry(string timeAccountName, TimeSpan duration)
        {
            TimeAccountName = timeAccountName;
            Duration = duration;
        }
    }
}
