using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ptsz1
{
    class Program
    {
        sealed class ProblemInstance
        {
            public ProblemInstance()
            {
                tasks = new List<int[]>();
            }

            public List<int[]> tasks;
        }

        static void Main(string[] args)
        {
            string file_name = args[0];
            int k = int.Parse(args[1]) - 1; //k starts from 1
            float h = float.Parse(args[2], CultureInfo.InvariantCulture);

            Parser parser = new Parser();
            var raw_data = parser.ParseInputFile(file_name);
            var instances = GetInstances(raw_data);

            ProblemInstance instance = instances[k];

            var result = solve(instance, h);
            Console.WriteLine(calculate_result(instance, h, result));
            Console.WriteLine(string.Join(" ", result.Select(x => string.Format("t{0}", x))));
        }

        static List<ProblemInstance> GetInstances(List<int[]> rawData)
        {
            int count = 0;
            List<ProblemInstance> ret = new List<ProblemInstance>();
            ProblemInstance tmp = new ProblemInstance();

            foreach (var row in rawData)
            {
                if (row.Length == 1)
                {
                    count = row[0];

                    if (tmp.tasks.Count > 0)
                    {
                        ret.Add(tmp);
                        tmp = new ProblemInstance();
                    }

                    continue;
                }

                if (count > 0)
                {
                    tmp.tasks.Add(row);
                    --count;
                }
            }

            if (tmp.tasks.Count > 0)
                ret.Add(tmp);

            return ret;
        }

        static IEnumerable<int> solve(ProblemInstance instance, float h)
        {
            return Enumerable.Range(0, instance.tasks.Count)
                             .OrderBy(x => Math.Abs(instance.tasks[x][1] - instance.tasks[x][2]))
                             .ToList();
        }

        static int calculate_deadline(ProblemInstance instance, float h)
        {
            return (int)Math.Round(instance.tasks.Sum(x => x[0]) * h);
        }

        static double calculate_result(ProblemInstance instance, float h, IEnumerable<int> order)
        {
            int current_time = 0;
            double cost = 0.0;
            int deadline = calculate_deadline(instance, h);

            foreach(int task_num in order)
            {
                int task_end = current_time + instance.tasks[task_num][0];

                if (task_end < deadline)
                    cost += (deadline - current_time) * instance.tasks[task_num][1];

                else if (task_end > deadline)
                    cost += (current_time - deadline) * instance.tasks[task_num][2];

                // if task_end == deadline, we pay no penalty
                current_time += instance.tasks[task_num][0];
            }

            return cost;
        }
    }
}
