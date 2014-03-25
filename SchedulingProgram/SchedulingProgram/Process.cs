using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingProgram 
{
    class Process : IComparable
    {

        public Process()
        {
            TotalCyclesNeeded = 1;
            TotalCyclesCompleted = 0;
            TotalTimeInCPU = 0;
            TotalTimeInIO = 0;
            TotalTImeInRQ = 0;
            TimeInCPU = 0;
            TimeInIO = 0;
            SmallestTimeInRQ = int.MaxValue;
            LongestTimeInRQ = int.MinValue;
            CurrentTimeInRQ = 0;
        }

        public string Name { get; set; }
        
        public int StartingPriority { get; set; }
        public int CurrentPriority{ get; set; }

        public int TimeNeededInCPU{ get; set; }
        public int TimeNeededInIO{ get; set; }

        public int TotalCyclesCompleted { get; set; }
        public int TotalCyclesNeeded { get; set; }
        
        public int TimeInCPU { get; set; }
        public int TimeInIO { get; set; }
        
        public int TotalTimeInCPU { get; set; }
        public int TotalTimeInIO { get; set; }
        public int TotalTImeInRQ{ get; set; }
        
        public int SmallestTimeInRQ { get; set; }
        public int LongestTimeInRQ { get; set; }
        public int CurrentTimeInRQ { get; set; }

        //0 cpu bounded 1 equal 2 io bounded
        public int ProcessType { get; set; }

        //0 ready queue 1 cpu 2 io
        public int CurrentState { get; set; }


        public override string ToString()
        {
            return "Process " + Name + " Current Priority " + CurrentPriority.ToString();
        }

        public int CompareTo(object x)
        {
            Process other = x as Process;
            if (this.CurrentPriority > other.CurrentPriority)
            {
                return -1;
            }
            else
            {
                if (this.CurrentPriority < other.CurrentPriority)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
