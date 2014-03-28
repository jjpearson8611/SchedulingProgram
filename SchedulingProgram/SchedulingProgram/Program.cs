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
            /* For the CPU bound processes time needed is
             *              CPU = 5 - 8
             *              IO  = 3 - 4
             *              
             * For the Equal processes time needed is
             *              CPU = 3 - 5
             *              IO  = 3 - 5
             *              
             * For the IO bound processes time needed is
             *              CPU = 3 - 4
             *              IO  = 5 - 8
             * 
             * 
             * Program Explaination and talking about methods
             * The first required method is the priority queue with aging.
             *      Processes are stored in an ordered list, as the different processes
             *      are cycled through the program they have 5 cycles in the cpu before they
             *      are kicked out if they complete their required cpu instructions they are put
             *      into the IO queue which does IO as they come. The programs that are stuck in the
             *      ready queue age while waiting and a stable insert is used to make sure everyone stays
             *      in the correct spot in line. 
             *Source: https://cs.wmich.edu/~bhardin/cs4540/simulationF14.htm -- specs
             * The second method is similar to the first in first out method
             *      in this one once a process finishes in the cpu it loads a new
             *      one into the IO queue but the catch is once it finishes in the io
             *      it kicks the new one out of the cpu becuase it was there first
             *      it does this until all are done and will starve the smaller processes
             *      Not perfect implimentation of FIFO because of other processes being alowed to
             *      enter cpu while the main process is in the io. But this does make the
             *      cpu work more often and I think is a good trade off
             *      Did this one for the coolness factor
             *Source: http://en.wikipedia.org/wiki/FIFO
             * The third method is a round robin scheduling algorithm which is very similar to the
             *      priority queue with aging, but the difference is there is no aging. once a process
             *      has completed its cycles or has been kicked out of the CPU it is put onto the end
             *      of the list, this helps a lot with starvation but doesn't account for higher priority
             *      processes being able to run first
             *Source: http://en.wikipedia.org/wiki/Round-robin_scheduling      
             *
             * The fourth method is a lottery method with added weighting. 
             *      Typically the lottery method just randomly selects a process and excecutes it
             *      I changed this a little bit to be based on its priority it gets entered 1 extra time per 
             *      priority and they can also age to help avoid starvation. once a process has been selected
             *      then the priority is reset  
             * Source:http://en.wikipedia.org/wiki/Lottery_scheduling
             */
            //different methods and process generation for each
            PriorityQueue RequiredMethod = new PriorityQueue();
            FIFO SimpleMethod = new FIFO();
            RoundRobin RRQueue = new RoundRobin();
            Lottery LotteryScheduling = new Lottery();
            GenerateProcesses(SimpleMethod);
            GenerateProcesses(RequiredMethod);
            GenerateProcesses(RRQueue);
            GenerateProcesses(LotteryScheduling);

            List<Process> PriorityQueue = new List<Process>();
            List<Process> NormalQueue = new List<Process>();
            List<Process> RoundRobinQueue = new List<Process>();
            List<Process> LotteryMethod = new List<Process>();
            ////we will now simulate
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
            Console.ReadLine();
            Console.WriteLine("\nDoing The Lottery Method");
            Console.ReadLine();
            LotteryMethod = LotteryScheduling.Simulate();
            Console.ReadLine();
            Graph grapher = new Graph();

        }


        public static void GenerateProcesses(ScheduleClass Temp)
        {
            int i;
            Process CPU;
            Process Eql;
            Process IO;
            Random rand = new Random();
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

                CPU.TimeNeededInCPU = rand.Next(5, 8);
                CPU.TimeNeededInIO = rand.Next(3,4);

                Eql.TimeNeededInCPU = rand.Next(3,5);
                Eql.TimeNeededInIO = rand.Next(3,5);


                IO.TimeNeededInCPU = rand.Next(3,4);
                IO.TimeNeededInIO = rand.Next(5,8); 

                CPU.TotalCyclesNeeded = rand.Next(4,6);
                IO.TotalCyclesNeeded = rand.Next(4, 6);
                Eql.TotalCyclesNeeded = rand.Next(4,6);

                Temp.InsertProcesses(CPU, Eql, IO); 
            }

            Temp.SortLists();
        }

    }
}
