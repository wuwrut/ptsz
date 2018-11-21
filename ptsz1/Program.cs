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
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Incorrect arguments. Arguments: <input file path> <k> <h>");
                return;
            }

            string file_name = args[0];
            int k = int.Parse(args[1]) - 1; //k starts from 1
            float h = float.Parse(args[2].Replace(',', '.'), CultureInfo.InvariantCulture);

            if (h > 1 || h < 0)
            {
                Console.WriteLine(string.Format("Passed invalid h value (h = {0})!", h));
                return;
            }

            Parser parser = new Parser();
            var raw_data = parser.ParseInputFile(file_name);
            var instances = GetInstances(raw_data);

            ProblemInstance instance = instances[k];

            var result = solve2(instance, h);
            Console.WriteLine(instance.CalculateResult(h, result));
            Console.WriteLine(string.Join(" ", result.Select(x => string.Format("t{0}", x))));

            int b = 0;
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
                             .OrderBy(x => instance.tasks[x][2] - instance.tasks[x][1])
                             .ToList();
        }

        static IEnumerable<int> solve2(ProblemInstance instance, float h)
        {
            int deadline = instance.CalculateDeadline(h);
            int current_time = 0;
            int i = 0;

            List<int> ret = new List<int>(instance.tasks.Count);

            List<int> sorted_by_a = Enumerable.Range(0, instance.tasks.Count)
                                              .OrderBy(x => instance.tasks[x][1] / instance.tasks[x][2])
                                              .OrderBy(x => instance.tasks[x][1])
                                              .ToList();

            while(current_time < deadline && i < sorted_by_a.Count)
            {
                ret.Add(sorted_by_a[i]);
                current_time += instance.tasks[i][0];
                ++i;
            }

            IEnumerable<int> sorted_by_b = sorted_by_a.Skip(i)
                                                      .OrderByDescending(x => instance.tasks[x][2]);

            foreach(int task_num in sorted_by_b)
                ret.Add(task_num);

            return ret;
        }
    }
}
