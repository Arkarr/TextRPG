using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
    public static class MathHelper
    {
        public static double Clamp (double value, double min, double max)
        {
            if (value > max) value = max;
            else if (value < min) value = min;
            return value;
        }
    }
}
