using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SimplAoC
{
    public static class SolutionRunner
    {
        private static DayInfo[][]? Infos;

        static SolutionRunner()
        {
            Infos = new DayInfo[7][];

            Infos[6] = new DayInfo[30];
            Infos[6][0] = new DayInfo("ABCD", "1234", null);
        }

        public static void RunAll()
        { 
            
        }

        public static void RunFromClass<T>() where T : class
        {
            Type t = typeof(T);

            MemberInfo[] funcs = t.GetMethods(BindingFlags.Static | BindingFlags.Public);

            var attrib = t.GetCustomAttribute<AoCYearAttribute>();
            int year = 0;
            if (attrib != null)
                year = attrib.Year;
            
            
            foreach (MethodInfo func in funcs)
            {

                var attr = func.GetCustomAttribute<AoCUnionAttribute>();
                if (attr is null)
                    continue;
                if (!func.IsValidSignature())
                {
                    var par = func.GetParameters();

                    var builder = new StringBuilder(1024);
                    builder.Append('(');
                    builder.AppendJoin(", ", par.Select(static info => info.ParameterType.Name));
                    builder.Append(')');

                    Console.WriteLine($"INVALID SIGNATURE: {func.Name}.\n" +
                        $"Expected (string) => string,\n" +
                        $"Got {builder} => {func.ReturnType.Name}\n");

                    continue;
                }

                Identifier id = attr switch
                {
                    AoCDayAttribute  a => new(year, a.Day, a.Part),
                    AoCDateAttribute a => new(a.Year, a.Day, a.Part),
                    AoCYearAttribute => throw new ArgumentException("Methods can't have the Year attribute applied"),
                    _ => throw new ArgumentException($"Unknown attribute: {attr.GetType()}")
                };

                if (!id.IsSet())
                    throw new Exception("Fuck");

                var info = Infos[id.Year - 2016][id.Day - 1];

                Console.WriteLine($"Running {func.Name} from {id}");
                string output = (string)func.Invoke(null, new object[] { info.Input });

                if (output == info[id.Part - 1])
                    Console.WriteLine($"Pass! Output:\n{output}");
                else
                    Console.WriteLine($"Fail. Output:\n{output}");
            }
        }

        /// <summary>
        /// Checks if the signature matches (string) => string
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static bool IsValidSignature(this MethodInfo info)
        {
            var param = info.GetParameters();

            if (param.Length != 1 || param[0].ParameterType != typeof(string))
                return false;

            if (info.ReturnType != typeof(string)) 
                return false;
            
            return true;
        }
    }

    

    public readonly struct DayInfo
    {
        public readonly string Input, Part1;
        public readonly string? Part2;

        public DayInfo(string input, string part1, string? part2) 
        {
            (Input, Part1, Part2) = (input, part1, part2); 
        }

        /// <summary>
        /// Part Indexer
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in Part1), 2)[index];
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal struct Identifier
    {
        public int Year, Day, Part;

        public Identifier(int year, int day, int part)
        { 
            (Year, Day, Part) = (year, day, part);  
        }

        public override string ToString()
        {
            return $"{{Y: {Year}, D: {Day}, P: {Part}}}";
        }

        public bool IsSet() => (Year, Day, Part) != (0, 0, 0);
    }
}



