using System.Collections.Generic;
using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public class ExpressionSyntax : BaseSyntax
    {
        public string Field { get; set; }
        public FilterQueryOperator Operator { get; set; } 
        public string FieldValue { get; set; }

        public override void Accept(BaseSyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}