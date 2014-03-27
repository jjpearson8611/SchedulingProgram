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
        private List<Process> FinishedProcesses = new List<Process>();
        Graph OutputGraph = new Graph();

        int temp = 0;
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
            List<Process> IOQueue = new List<Process>();
            while (NotDoneYet)
            {
                //All of the processes are in the io queue
                if (Processes.Count != 0)
                {
                    //the current process is done runninng
                    if (Processes[0].TimeInCPU == Processes[0].TimeNeededInCPU)
                    {
                            //put it in the io queue to get io and remove it from the process
                            //queue till later
                        if (Processes[0].CurrentTimeInRQ > Processes[0].LongestTimeInRQ)
                        {
                            Processes[0].LongestTimeInRQ = Processes[0].CurrentTimeInRQ;
                        }
                        if (Processes[0].CurrentTimeInRQ < Processes[0].SmallestTimeInRQ)
                        {
                            Processes[0].SmallestTimeInRQ = Processes[0].CurrentTimeInRQ;
                        }
                        Processes[0].CurrentTimeInRQ = 0;
                            IOQueue.Add(Processes[0]);
                            Processes.Remove(Processes[0]);
                    }
                    else
                    {
                        //we are about to reset the counter because we are in the cpu now
                        //check longest and shortest times then restart current time in rq
                       

                        //current state is 1 because we are in the cpu
                        Processes[0].CurrentState = 1;

                        //increase the time that it has been in the cpu
                        Processes[0].TimeInCPU++;
                    }
                }

                //all the processes are in the priority cpu queue
                if (IOQueue.Count != 0)
                {
                    
                    //if it has gotten all of its IO time done
                    if (IOQueue[0].TimeInIO == IOQueue[0].TimeNeededInIO)
                    {

                        //Set the state to ready queue and increase the number of cycles completed
                        IOQueue[0].CurrentState = 0;
                        IOQueue[0].TotalCyclesCompleted++;

                        //if all the cycles are complete remove the process and log its values
                        if (IOQueue[0].TotalCyclesCompleted == IOQueue[0].TotalCyclesNeeded)
                        {
                            //print out the process information and log the information
                            PrintOutProcessInformation(IOQueue[0]);

                            //remove the process from the io queue
                            IOQueue.Remove(IOQueue[0]);
                        }
                        else
                        {
                            //Console.WriteLine("moving from" + IOQueue[0].Name + " io back to cpu");
                            IOQueue[0].TimeInIO = 0;
                            Insertion(IOQueue[0]);
                            IOQueue.Remove(IOQueue[0]);
                            int temp = Processes.Count;
                        }
                    }
                    else
                    {
                        IOQueue[0].TimeInIO++;
                        IOQueue[0].CurrentState = 2;
                    }
                }

                //we need to age the processes and increase priorities accordingly
                AgeProcesses();

                //we are all done with all the processes so lets end it
                if (Processes.Count == 0 && IOQueue.Count == 0)
                {
                    NotDoneYet = false;
                    Console.WriteLine("All Processes Have Been Eliminated");
                }
            }

            Console.WriteLine(FinishedProcesses.Count.ToString());
            OutputGraph.TakeProcesses(FinishedProcesses.ToArray());
        }
        public void PrintOutProcessInformation(Process x)
        {
            temp++;
            Console.WriteLine("Process Number: " + x.Name + "number printed" + temp.ToString());
            FinishedProcesses.Add(x);
            //Console.WriteLine("Starting Priority: " + x.StartingPriority);
            //Console.WriteLine("Current Priority: " + x.CurrentPriority);
            //Console.WriteLine("Total Cycles Completed: " + x.TotalCyclesCompleted);
            //Console.WriteLine("Total In CPU: " + x.TotalTimeInCPU);
            //Console.WriteLine("Total In IO: " + x.TotalTimeInIO);
            //Console.WriteLine("Total in Ready Queue: " + x.TotalTImeInRQ);
            //Console.WriteLine("Smallest In Ready Queue: " + x.SmallestTimeInRQ);
            //Console.WriteLine("Longest In Ready Queue: " + x.LongestTimeInRQ);
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
                    if (Processes[i].CurrentPriority < 15)
                    {
                        Processes[i].CurrentPriority++;
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

            //do a stable insert of the processs
            for (numbers = 0; numbers < Processes.Count; numbers++)
            {
                if (Processes[numbers].CurrentPriority < temp.CurrentPriority)
                {
                    Processes.Insert(numbers, temp);
                    numbers = int.MaxValue - 1;
                }
            }

            //it is actually the smallest so put it at the end
            if (numbers < 1000)
            {
                Processes.Add(temp);
            }
        }
    }
}
