using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplAoC
{
    
    public class AoCUnionAttribute : Attribute
    { }

    [System.AttributeUsage(AttributeTargets.Method,
       AllowMultiple = false,
       Inherited = false)]
    public sealed class AoCDayAttribute : AoCUnionAttribute
    {
        public readonly int Day, Part;

        public AoCDayAttribute(int day, int part = 1)
        {
            (Day, Part) = (day, part);
        }
    }

    [System.AttributeUsage(AttributeTargets.Class,
       AllowMultiple = false,
       Inherited = false)]
    public sealed class AoCYearAttribute : AoCUnionAttribute
    {
        public readonly int Year;

        public AoCYearAttribute(int year)
        {
            Year = year;
        }
    }

    [System.AttributeUsage(AttributeTargets.Method,
       AllowMultiple = false,
       Inherited = false)]
    public sealed class AoCDateAttribute : AoCUnionAttribute
    {
        public readonly int Year, Day, Part;

        public AoCDateAttribute(int year, int day, int part = 1)
        {
            (Year, Day, Part) = (year, day, part);
        }
    }
}
