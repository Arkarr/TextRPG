using Engine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class DerivedGameAttribute : GameAttribute
    {
        public AttributeText ValueExpression { get; private set; }

        public double baseValue = 0.0;
        public override int Current
        {
            get { return (int)baseValue + ValueExpression.GetResolvedInteger(); }
            protected set { baseValue = value; }
        }

        public double DoubelValue
        {
            get { return baseValue + ValueExpression.GetResolvedDouble(); }
            set { baseValue = value; }
        }

        public List<GameAttribute> DependantAttributes
        {
            get { return ValueExpression.DependantAttributes; }
            set { ValueExpression.DependantAttributes = value; }
        }

        public DerivedGameAttribute (string name) : base(name)
        {
            ValueExpression = new AttributeText(this);
        }
    }
}
