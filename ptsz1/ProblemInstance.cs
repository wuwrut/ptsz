﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptsz1
{
    sealed class ProblemInstance
    {
        public ProblemInstance()
        {
            tasks = new List<int[]>();
        }

        public void AddTask(int[] task)
        {
            tasks.Add(task);
        }

        public int CalculateDeadline(float h)
        {
            return (int)(tasks.Sum(x => x[0]) * h);
        }

        public double CalculateResult(float h, IEnumerable<int> order)
        {
            int current_time = 0;
            double cost = 0.0;
            int deadline = CalculateDeadline(h);

            foreach (int task_num in order)
            {
                int task_end = current_time + tasks[task_num][0];

                if (task_end < deadline)
                    cost += (deadline - task_end) * tasks[task_num][1];

                else
                    cost += (task_end - deadline) * tasks[task_num][2];

                current_time += tasks[task_num][0];
            }

            return cost;
        }

        public List<int[]> tasks;
    }
}
