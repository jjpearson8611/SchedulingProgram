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
        
        public void Simulate()
        {
            bool NotDoneYet = true;
            int WhichType = 2;
            List<Process> IOQueue = new List<Process>();
            while (NotDoneYet)
            {

                if (Processes[0].TimeInCPU == Processes[0].TimeNeededInCPU)
                {
                    if (Processes[0] != null)
                    {
                        IOQueue.Add(Processes[0]);
                    }
                    
                }
                else
                {
                    Processes[0].CurrentState = 1;
                    Processes[0].TimeInCPU++;
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
                            Processes.Sort();
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
            Console.WriteLine("Process Number: " + x.Name);
            Console.WriteLine("Starting Priority: " + x.StartingPriority);
            Console.WriteLine("Current Priority: " + x.CurrentPriority);
            Console.WriteLine("Completed: " + x.TotalCyclesCompleted);
            Console.WriteLine("Total In CPU: " + x.TotalTimeInCPU);
            Console.WriteLine("Total In IO: " + x.TotalTimeInIO);
            Console.WriteLine("Total in Ready Queue: " + x.TotalTImeInRQ);
            Console.WriteLine("Smallest In Ready Queue: " + x.SmallestTimeInRQ);
            Console.WriteLine("Longest In Ready Queue: " + x.LongestTimeInRQ);
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

        public void Insertion(Process temp)
        {
            int numbers;
            for (numbers = 0; numbers < Processes.Count; numbers++)
            {
                if (Processes[numbers].CurrentPriority < temp.CurrentPriority)
                {
                    Processes.Insert(numbers, temp);
                }
            }
        }
    }
}
