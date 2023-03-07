using System.Runtime.CompilerServices;

using SimplAoC;

namespace SimplAoC.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SolutionRunner.RunFromClass<Solutions>();
            SolutionRunner.RunFromClass<BasedSolutions>();
        }
    }


    [AoCYear(2022)]
    public class Solutions
    {
        [AoCDay(1)]
        public static string Day1(string input)
        {
            return input;
        }

        [AoCDay(1)]
        public static string Day1SIMD(string input)
        {
            var epicSIMDstr = "1234";
            return Unsafe.As<string>(Unsafe.As<object>(epicSIMDstr));
        }
    }

    public class BasedSolutions
    {
        [AoCDate(2022, 1)]
        public static string Day1(string input)
        {
            return input;
        }

        [AoCDate(2022, 1)]
        public static string Day1SIMD(string input)
        {
            var epicSIMDstr = "1234";
            return Unsafe.As<string>(Unsafe.As<object>(epicSIMDstr));
        }
    }
}