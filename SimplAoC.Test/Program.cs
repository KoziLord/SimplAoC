using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SimplAoC.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input your session cookie:");
            SolutionRunner.SessionCookie = Console.ReadLine();

            SolutionRunner.RunFromClass<Solutions>();

        }
    }


    [AoCYear(2022)]
    public class Solutions
    {
        [AoCDay(1)]
        public static string Day1Fake(string input)
        {
            return "xd";
        }

        //SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        #region SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        //SPOILERS
        [AoCDay(1)]
        public static string Day1(string input)
        {
            var lines = input.Split('\n');
            List<int> dwarves = new();

            dwarves.Add(0);

            foreach (var line in lines)
            {
                if (line == "")
                    dwarves.Add(0);
                else
                    dwarves[dwarves.Count-1] += int.Parse(line);
            }

            int largestIndex = -1;
            int largestDwarf = -1;

            int i = 0;
            foreach (var dwarf in dwarves)
            {
                if (dwarf > largestDwarf)
                    (largestDwarf, largestIndex) = (dwarf, i);

                i++;
            }

            return largestDwarf.ToString();
        }
#endregion
    }
}