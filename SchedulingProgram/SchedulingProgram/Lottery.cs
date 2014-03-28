using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingProgram
{
    class Lottery: ScheduleClass
    {
        private List<Process> Processes = new List<Process>();
        private List<Process> IncomingProcesses = new List<Process>();
        private List<Process> FinishedProcesses = new List<Process>();
        Graph OutputGraph = new Graph();
        public Lottery(){ }


        public void InsertProcesses(Process CPU, Process Eql, Process IO)
        {
            IncomingProcesses.Add(CPU);
            IncomingProcesses.Add(Eql);
            IncomingProcesses.Add(IO);
        }

        public void SortLists()
        {
            Processes.Sort();
        }

        public void AddAnotherProcess()
        {
            if (IncomingProcesses.Count != 0)
            {
                Processes.Add(IncomingProcesses[0]);
                IncomingProcesses.Remove(IncomingProcesses[0]);
            }
        }
        
        public List<Process> Simulate()
        {
            bool NotDoneYet = true;
            List<Process> IOQueue = new List<Process>();
            AddAnotherProcess();
            Random rand = new Random();
            int NumberOfCycles = 0;
            int next = Next();
            while (NotDoneYet)
            {
                AddAnotherProcess();
                //timer to stop over consumption of cycles
                if (NumberOfCycles == 5)
                {
                    //we got kicked out of the cpu so reset the time in RQ and keep going
                    Processes[next].CurrentPriority = Processes[next].StartingPriority;
                    if (Processes[next].CurrentTimeInRQ > Processes[next].LongestTimeInRQ)
                    {
                        Processes[next].LongestTimeInRQ = Processes[next].CurrentTimeInRQ;
                    }
                    if (Processes[next].CurrentTimeInRQ < Processes[next].SmallestTimeInRQ)
                    {
                        Processes[next].SmallestTimeInRQ = Processes[next].CurrentTimeInRQ;
                    }

                    //reset the counter for in the ready queue
                    Processes[next].CurrentTimeInRQ = 0;

                    //holds the process while we re insert it
                    Process ProcessSwap = Processes[next];

                    //reset the timer
                    NumberOfCycles = 0;

                    //pick another random process
                    next = rand.Next(0, Processes.Count);

                    //remove and reinsert the process
                    Processes.Remove(ProcessSwap);
                    Processes.Add(ProcessSwap);
                }
                else
                {
                    //All of the processes are in the io queue
                    if (Processes.Count != 0)
                    {
                        //the current process is done runninng
                        if (Processes[next].TimeInCPU == Processes[next].TimeNeededInCPU)
                        {
                            //put it in the io queue to get io and remove it from the process
                            //queue till later
                            if (Processes[next].CurrentTimeInRQ > Processes[next].LongestTimeInRQ)
                            {
                                Processes[next].LongestTimeInRQ = Processes[next].CurrentTimeInRQ;
                            }
                            if (Processes[next].CurrentTimeInRQ < Processes[next].SmallestTimeInRQ)
                            {
                                Processes[next].SmallestTimeInRQ = Processes[next].CurrentTimeInRQ;
                            }
                            Processes[next].CurrentPriority = Processes[next].StartingPriority;
                            Processes[next].CurrentTimeInRQ = 0;
                            Processes[next].CurrentState = 2;
                            IOQueue.Add(Processes[next]);
                            Processes.Remove(Processes[next]);
                            next = Next();
                            NumberOfCycles = 0;
                        }
                        else
                        {
                            NumberOfCycles++;

                            //current state is 1 because we are in the cpu
                            Processes[next].CurrentState = 1;

                            //increase the time that it has been in the cpu
                            Processes[next].TimeInCPU++;
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
                                FinishedProcesses.Add(IOQueue[0]);

                                //remove the process from the io queue
                                IOQueue.Remove(IOQueue[0]);
                            }
                            else
                            {
                                //Console.WriteLine("moving from" + IOQueue[0].Name + " io back to cpu");
                                IOQueue[0].TimeInIO = 0;
                                Processes.Add(IOQueue[0]);
                                IOQueue.Remove(IOQueue[0]);
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
                }
                //we are all done with all the processes so lets end it
                if (IOQueue.Count == 0)
                {
                    if (Processes.Count == 0)
                    {
                        NotDoneYet = false;
                        Console.WriteLine("All Processes Have Been Eliminated");
                    }
                }
            }

            Console.WriteLine(FinishedProcesses.Count.ToString());
            OutputGraph.TakeProcesses(FinishedProcesses.ToArray());
            return FinishedProcesses;
        }

        public void AgeProcesses()
        {
            int i;
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
        public int Next()
        {
            int i; 
            int j;
            List<Process> tempList = new List<Process>();
            Random rand = new Random();

            if (Processes.Count > 1)
            {
                for (i = 0; i < Processes.Count; i++)
                {
                    for (j = 0; j < Processes[i].CurrentPriority; j++)
                    {
                        tempList.Add(Processes[i]);
                    }
                }
                int temp = rand.Next(0, tempList.Count -1);
                temp = Processes.IndexOf(tempList[temp]);
                return temp;
            }
            else
            {
                return 0;
            }
        }
    }
}