using Engine.Text;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class GameAttribute : EngineObject
    {
        public const int DEFAULT_CURRENT_VALUE = 0;
        public const int DEFAULT_MIN_VALUE = 0;
        public const int DEFAULT_MAX_VALUE = int.MaxValue;

        public virtual int Current { get; protected set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public string FormatString { get; set; }

        public string Abbreviation { get; set; }
        public AttributeText Descriptor { get; set; }
        public string DescriptorString
        {
            get { return Descriptor != null ? Descriptor.GetResolvedText() : null; }
            set { Descriptor.SimpleText = value; }
        }

        public string DefaultText
        {
            get { return TextFormatter.GetFormattedText(this, FormatString); }
        }

        public GameAttribute (string name) : base(name)
        {
            Current = DEFAULT_CURRENT_VALUE;
            Min = DEFAULT_MIN_VALUE;
            Max = DEFAULT_MAX_VALUE;
        }

        public GameAttribute (string name, int current, int min, int max) : base(name)
        {
            Current = current;
            Min = min;
            Max = max;
        }

        public string Format (string formatString)
        {
            return TextFormatter.GetFormattedText(this, formatString);
        }

        public void Increase (int value)
        {
            if (value < 1) Debug.Log(this, string.Format("Abnormal value received while increasing an attribute ({0})", value), DebugMessageType.Warning);

            Current += value;

            if (Current > Max)
            {
                Current = Max;
                Debug.Log(this, string.Format("Current value increased and exceed the maximum and thus was clamped ({0})", Max), DebugMessageType.Warning);
            }
        }

        public void Decrease (int value)
        {
            if (value < 1) Debug.Log(this, string.Format("Abnormal value received while decreasing an attribute ({0})", value), DebugMessageType.Warning);

            Current -= value;

            if (Current < Min)
            {
                Current = Min;
                Debug.Log(this, string.Format("Current value decreased and became inferior to the minimum and thus was clamped ({0})", Min), DebugMessageType.Warning);
            }
        }

        public void Set (int value)
        {
            Current = value;

            if (Current > Max || Current < Min)
            {
                Debug.Log(this, string.Format("Current value was set and is now out of bounds {0} < {1} < {2}, if it is not intended please be careful", Min, Current, Max), DebugMessageType.Warning);
            }
        }
    }
}
