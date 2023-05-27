using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SimplAoC
{
    public static class SolutionRunner
    {
        private static string? _cookie;
        public static string? SessionCookie
        {
            get => _cookie;
            set 
            {
                _cookie = value;
                client.DefaultRequestHeaders.Remove("cookie");
                client.DefaultRequestHeaders.Add("cookie", $"session={SessionCookie}");
            }
        }   
        private static Dictionary<(int Year, int Day), string> inputs = new();
        private static HttpClient client = new HttpClient();

        private static void CacheInputsToFile()
        {
            var filePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.AsSpan())}/InputCache.kas";

            var file = File.OpenWrite(filePath);

            foreach (var entry in inputs)
            {
                file.Write(entry.Key);
                file.Write(entry.Value.Length);
                file.Write(MemoryMarshal.Cast<char, byte>(entry.Value.AsSpan()));
            }

            file.Close();
        }
        /*
        private static void ReadInputsFromFile()
        {
            List<byte> builder = new(1024 * 4);
            Span<byte> buffer = stackalloc byte[1024];

            
            var filePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.AsSpan())}/InputCache.kas";

            FileStream file;
            try
            {
                file = File.OpenRead(filePath);
            }
            catch
            {
                return;
            };
            var readBytes = 0;
            do
            {
                (int, int, int) info = default;
                var slice = MemoryMarshal.CreateSpan(ref Unsafe.As<(int, int, int), byte>(ref info), 12);
                readBytes = file.Read(slice);
                var (year, day, leftToRead) = info;

                do
                {
                    readBytes = file.Read(buffer);
                    builder.AddRange(buffer.Slice(0, readBytes));
                } while (readBytes != 0);

                var str = MemoryMarshal.Cast<byte, char>(CollectionsMarshal.AsSpan(builder));
                inputs.Add((year, day), )
            } while (readBytes != 0);
        }
        */

        private static bool TryGetInput(int year, int day, out string? input)
        { 
            if (inputs.TryGetValue((year, day), out input))
                return true;

            try
            {
                var str = client.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input").Result;

                inputs.Add((year, day), str);

                input = str;
                return true;
            }

            catch
            {
                input = null;
                return false;
            }
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

                bool ok = TryGetInput(id.Year, id.Day, out var input);
                if (!ok)
                {
                    Console.WriteLine($"Skipping {func.Name}, could not load the input for Day {id.Day} of Year {id.Year}");
                    continue;
                }
                Console.WriteLine($"Running {func.Name} from {id}");
                string? output = (string?)func.Invoke(null, new object[] { input });

                if (output != null)
                    Console.WriteLine($"Pass! Output:\n{output}");
                else
                    Console.WriteLine($"Fail. Output:\n{output}");

            }

            CacheInputsToFile();
        }

        /// <summary>
        /// Checks if the Method signature matches (string) => string
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



