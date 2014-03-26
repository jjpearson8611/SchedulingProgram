using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SchedulingProgram
{
    class Graph
    {
        public Graph() { }

        public void TakeProcesses(Process[] Processes)
        {
            List<int> numbers = new List<int>();

            for(int i = 0; i < Processes.Length; i++)
            {
                numbers.Add(Processes[i].TotalTImeInRQ);
            }

            CreateGraph(numbers.ToArray());
        }
        public void CreateGraph(int[] numbers)
        {
            int i; int j;
            int MaxNumber = int.MinValue;
            for (i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > MaxNumber)
                {
                    MaxNumber = numbers[i];
                }
            }
            Console.WriteLine('|');
            for (i = 0; i < numbers.Length; i++)
            {
                Console.Write('|');
                for (j = 0; j < (int)((float)numbers[i] / (float)MaxNumber * 25f); j++)
                {
                    Console.Write('#');
                }
                Console.Write(" " + i.ToString() + " " + numbers[i].ToString());
                Console.WriteLine();
            }
        }
    }
}
