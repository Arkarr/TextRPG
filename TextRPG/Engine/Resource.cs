using Engine.Text;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Resource : GameAttribute
    {
        public new const int DEFAULT_CURRENT_VALUE = 0;
        public new const int DEFAULT_MIN_VALUE = 0;
        public new const int DEFAULT_MAX_VALUE = 100;

        public bool Depleted { get { return Current == Min; } }
        public bool Full { get { return Current == Max; } }

        /// <summary>
        /// Impl. later with character class
        /// </summary>
        public bool StartsFull { get; set; }

        /// <summary>
        /// Gets or sets the Current value in percents (0 to 100)
        /// </summary>
        public double Percentage
        {
            get { return (double)Current / Max * 100.0; }
            set { Current = (int)(Max * (MathHelper.Clamp(value, 0.0, 100.0)  / 100.0)); }
        }

        public Resource (string name) : base (name, DEFAULT_CURRENT_VALUE, DEFAULT_MIN_VALUE, DEFAULT_MAX_VALUE)
        {
        }
    }
}
