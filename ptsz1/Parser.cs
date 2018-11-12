using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ptsz1
{
    sealed class Parser
    {
        public Parser()
        {
            matcher = new Regex(@"\d+", RegexOptions.Compiled);
        }

        public List<int[]> ParseInputFile(string name)
        {
            return File.ReadLines(name)
                       .Select(ParseFileSingleLine)
                       .Skip(1)
                       .ToList();
        }

        private int[] ParseFileSingleLine(string line)
        {
            return matcher.Matches(line)
                          .Cast<Match>()
                          .Select(m => m.Value)
                          .Select(int.Parse)
                          .ToArray();
        }

        private readonly Regex matcher;
    }
}
