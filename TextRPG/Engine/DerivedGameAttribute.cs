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

        public override double Current
        {
            get { return base.Current + ValueExpression.GetResolvedDouble(); }
            protected set { base.Current = value; }
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
