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
            PriorityQueue FirstMethod = new PriorityQueue();
            GenerateProcesses(FirstMethod);

            //we will now simulate
            FirstMethod.Simulate();
        }


        public static void GenerateProcesses(PriorityQueue Temp)
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

                CPU.TimeNeededInCPU = 3;
                CPU.TimeNeededInIO = 1;

                Eql.TimeNeededInCPU = 2;
                Eql.TimeNeededInIO = 2;

                IO.TimeNeededInIO = 3;
                IO.TimeNeededInCPU = 1;

                Temp.InsertProcesses(CPU, Eql, IO); 
            }

            Temp.SortLists();
            Temp.PrintOutProcesses();
        }

    }
}
