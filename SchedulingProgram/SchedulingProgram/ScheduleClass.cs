using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingProgram
{
    interface ScheduleClass
    {
        void SortLists();
        void InsertProcesses(Process CPU, Process Eql, Process IO);
        List<Process> Simulate();
    }
}
