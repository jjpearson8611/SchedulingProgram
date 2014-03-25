using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace SchedulingProgram
{
    class PriorityQueue
    {
        private List<Process> Processes = new List<Process>();

        public PriorityQueue(){ }


        public void InsertProcesses(Process CPU, Process Eql, Process IO)
        {
            Processes.Add(CPU);
            Processes.Add(Eql);
            Processes.Add(IO);
        }

        public void SortLists()
        {
            Processes.Sort();
        }
        public void PrintOutProcesses()
        {
            int i;
            Console.WriteLine("CPU Bound Processes");
            for (i = 0; i < Processes.Count; i++)
            {
                Console.WriteLine(Processes[i].ToString());
            }
        }

        public Process GetTop(int type)
        {
            int i;

            //find the top cpu
            for (i = 0; i < Processes.Count; i++)
            {
                if (Processes[i].ProcessType == type)
                {
                    return Processes[i];
                }
            }
            if (type == 0)
            {
                type = 3;
            }
            //this will only happen if no top cpu process has been found
            // this will return another process if none exist
            return GetTop(--type);
        }
        public void Simulate()
        {
            bool NotDoneYet = true;
            int WhichType = 2;
            Process CPUWorkingProcess = new Process();
            CPUWorkingProcess.ProcessType = -1;
            CPUWorkingProcess.TimeNeededInCPU = -1;
            List<Process> IOQueue = new List<Process>();
            while (NotDoneYet)
            {
                if (CPUWorkingProcess.TimeInCPU == CPUWorkingProcess.TimeNeededInCPU || CPUWorkingProcess.ProcessType == -1)
                {
                    if(CPUWorkingProcess != null)
                    {
                        IOQueue.Add(CPUWorkingProcess);
                    }
                    switch (WhichType)
                    {
                        case 1:
                            //Console.WriteLine("Equal");
                            CPUWorkingProcess = GetTop(WhichType);
                            WhichType--;
                            break;
                        case 2:
                            //Console.WriteLine("IO");
                            CPUWorkingProcess = GetTop(WhichType);
                            WhichType--;
                            break;

                        case 0:
                            //Console.WriteLine("CPU");
                            CPUWorkingProcess = GetTop(WhichType);
                            WhichType = 2;
                            break;
                    }
                }
                else
                {
                    CPUWorkingProcess.CurrentState = 1;
                    CPUWorkingProcess.TimeInCPU++;
                }
                if (IOQueue.Count != 0)
                {
                    if (IOQueue[0].TimeInIO == IOQueue[0].TimeNeededInIO)
                    {
                        IOQueue[0].CurrentState = 0;
                        IOQueue[0].TotalCyclesCompleted++;

                        if (IOQueue[0].TotalCyclesCompleted == IOQueue[0].TotalCyclesNeeded)
                        {
                            Console.WriteLine("one has been removed");
                            Processes.Remove(IOQueue[0]);
                            PrintOutProcessInformation(IOQueue[0]);
                            IOQueue.Remove(IOQueue[0]);

                        }
                    }
                    else
                    {
                        IOQueue[0].TimeInIO++;
                        IOQueue[0].CurrentState = 2;
                    }
                }

                AgeProcesses();
                if (Processes.Count == 0)
                {
                    NotDoneYet = false;
                    Console.WriteLine("we made it");
                }
            }
        }
        public void PrintOutProcessInformation(Process x)
        {
            Console.WriteLine("YAY");
        }

        public void AgeProcesses()
        {
            int i;
           // Console.WriteLine("Aging CPU Bound Processes");
            for (i = 0; i < Processes.Count; i++)
            {
                if (Processes[i].CurrentState == 0)
                {
                    Processes[i].TotalTImeInRQ++;
                    Processes[i].CurrentTimeInRQ++;
                    Processes[i].CurrentPriority++;
                    if (Processes[i].CurrentTimeInRQ > Processes[i].LongestTimeInRQ)
                    {
                        Processes[i].LongestTimeInRQ = Processes[i].CurrentTimeInRQ;
                    }
                    if (Processes[i].CurrentTimeInRQ < Processes[i].SmallestTimeInRQ)
                    {
                        Processes[i].SmallestTimeInRQ = Processes[i].CurrentTimeInRQ;
                    }
                }
                else if (Processes[i].CurrentState == 1)
                {
                    Processes[i].TotalTimeInCPU++;
                }
                else
                {
                    Processes[i].TotalTimeInIO++;
                }
            }

        }
    }
}
