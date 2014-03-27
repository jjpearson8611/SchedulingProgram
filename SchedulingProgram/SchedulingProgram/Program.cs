using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingProgram
{
    class Program
    {
        static void Main(string[] args)
        {            
            /* Program Explaination and talking about methods
             * The first required method is the priority queue with aging.
             *      Processes are stored in an ordered list, as the different processes
             *      are cycled through the program they have 5 cycles in the cpu before they
             *      are kicked out if they complete their required cpu instructions they are put
             *      into the IO queue which does IO as they come. The programs that are stuck in the
             *      ready queue age while waiting and a stable insert is used to make sure everyone stays
             *      in the correct spot in line. 
             * The second method is similar to the first in first out method
             *      in this one once a process finishes in the cpu it loads a new
             *      one into the IO queue but the catch is once it finishes in the io
             *      it kicks the new one out of the cpu becuase it was there first
             *      it does this until all are done and will starve the smaller processes
             *      Did this one for the coolness factor
             * The third method is a round robin scheduling algorithm which is very similar to the
             *      priority queue with aging, but the difference is there is no aging. once a process
             *      has completed its cycles or has been kicked out of the CPU it is put onto the end
             *      of the list, this helps a lot with starvation but doesn't account for higher priority
             *      processes being able to run first
             */
            //different methods and process generation for each
            PriorityQueue RequiredMethod = new PriorityQueue();
            FIFO SimpleMethod = new FIFO();
            RoundRobin RRQueue = new RoundRobin();
            GenerateProcesses(SimpleMethod);
            GenerateProcesses(RequiredMethod);
            GenerateProcesses(RRQueue);

            List<Process> PriorityQueue = new List<Process>();
            List<Process> NormalQueue = new List<Process>();
            List<Process> RoundRobinQueue = new List<Process>();
            //we will now simulate
            Console.WriteLine("Doing the Required Priority Queue With Aging");
            PriorityQueue = RequiredMethod.Simulate();
            Console.ReadLine();
            Console.WriteLine("\nDoing The FIFO Starvation Method");
            Console.ReadLine();
            NormalQueue = SimpleMethod.Simulate();
            Console.ReadLine();
            Console.WriteLine("\nDoing The Round Robin Method");
            Console.ReadLine();
            RoundRobinQueue = RRQueue.Simulate();
        }


        public static void GenerateProcesses(ScheduleClass Temp)
        {
            int i;
            Process CPU;
            Process Eql;
            Process IO;
            for (i = 0; i <= 15; i++)
            {
                CPU = new Process();
                Eql = new Process();
                IO = new Process();

                CPU.Name = "CPU" + i.ToString();
                Eql.Name = "Equal" + i.ToString();
                IO.Name = "IO" + i.ToString();

                CPU.CurrentPriority = CPU.StartingPriority = i;
                Eql.CurrentPriority = Eql.StartingPriority = i;
                IO.CurrentPriority = IO.StartingPriority = i;

                CPU.TimeNeededInCPU = 6;
                CPU.TimeNeededInIO = 2;

                Eql.TimeNeededInCPU = 3;
                Eql.TimeNeededInIO = 3;


                IO.TimeNeededInCPU = 2;
                IO.TimeNeededInIO = 6; 

                CPU.TotalCyclesNeeded = 3;
                IO.TotalCyclesNeeded = 3;
                Eql.TotalCyclesNeeded = 3;

                Temp.InsertProcesses(CPU, Eql, IO); 
            }

            Temp.SortLists();
        }

    }
}
