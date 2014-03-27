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
            List<string> names = new List<string>();
            float average = 0;

            Console.WriteLine("Longest Time In Ready Queue");
            for (int i = 0; i < Processes.Length; i++)
            {
                names.Add(Processes[i].Name);
                numbers.Add(Processes[i].LongestTimeInRQ);
                average += Processes[i].LongestTimeInRQ;
            }
            average = (average / Processes.Length);
            Console.WriteLine("Average Longest Time: " + average.ToString());
            CreateGraph(numbers,names);

            average = 0;

            Console.WriteLine("Shortest Time In Ready Queue");
            numbers.RemoveRange(0, numbers.Count);
            for (int i = 0; i < Processes.Length; i++)
            {
                numbers.Add(Processes[i].SmallestTimeInRQ);
                average += Processes[i].SmallestTimeInRQ;
            }
            average = (average / Processes.Length);
            Console.WriteLine("Average Smallest Time: " + average.ToString());
            CreateGraph(numbers,names);

            average = 0;

            Console.WriteLine("Total Time In Ready Queue");
            numbers.RemoveRange(0, numbers.Count);
            for (int i = 0; i < Processes.Length; i++)
            {
                numbers.Add(Processes[i].TotalTImeInRQ);
                average += Processes[i].TotalTImeInRQ;
            }
            average = (average / Processes.Length);
            Console.WriteLine("Average RQ Time: " + average.ToString());
            CreateGraph(numbers,names);
        }
        public void CreateGraph(List<int> numbers, List<string> names)
        {
            int i; int j;
            int MaxNumber = int.MinValue;
            for (i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] > MaxNumber)
                {
                    MaxNumber = numbers[i];
                }
            }
            Console.WriteLine('|');
            for (i = 0; i < numbers.Count; i++)
            {
                Console.Write('|');
                for (j = 0; j < (int)((float)numbers[i] / (float)MaxNumber * 25f); j++)
                {
                    Console.Write('#');
                }
                Console.Write(" " + names[i] + " " + numbers[i].ToString());
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
