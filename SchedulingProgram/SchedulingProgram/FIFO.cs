﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace SchedulingProgram
{
    class FIFO : ScheduleClass
    {
        private List<Process> Processes = new List<Process>();
        private List<Process> IncomingProcesses = new List<Process>();
        private List<Process> FinishedProcesses = new List<Process>();
        Graph OutputGraph = new Graph();
        public FIFO() { }


        //adds some processes into the incoming processes
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

        //Inserts a process if there are any incoming left
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
            while (NotDoneYet)
            {
                AddAnotherProcess();
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
                        Processes[0].CurrentPriority = Processes[0].StartingPriority;
                        Processes[0].CurrentTimeInRQ = 0;
                        Processes[0].CurrentState = 2;
                        IOQueue.Add(Processes[0]);
                        Processes.Remove(Processes[0]);
                    }
                    else
                    {
                        //current state is 1 because we are in the cpu
                        Processes[0].CurrentState = 1;

                        //increase the time that it has been in the cpu
                        Processes[0].TimeInCPU++;
                    }
                }
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
                            //add to the list of finished processes
                            FinishedProcesses.Add(IOQueue[0]);

                            //remove the process from the io queue
                            IOQueue.Remove(IOQueue[0]);
                        }
                        else
                        {
                            //Console.WriteLine("moving from" + IOQueue[0].Name + " io back to cpu");
                            IOQueue[0].TimeInIO = 0;

                            //put it back in front
                            Processes.Insert(0, IOQueue[0]);
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
