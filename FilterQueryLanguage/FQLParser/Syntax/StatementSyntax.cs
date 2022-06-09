using System.Collections.Generic;
using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public class StatementSyntax : BaseSyntax
    {
        public ExpressionSyntax Expression { get; set; }
        public FilterQueryLogicalOperator? Operator { get; set; }
        public StatementSyntax LeftStatement { get; set; }
        public StatementSyntax RightStatement { get; set; }

        public override void Accept(BaseSyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }

        public bool HasExpression => Expression != null;
        public bool HasLeftStatement => LeftStatement != null;
        public bool HasRightStatement => RightStatement != null;
        public bool HasOperator => !Operator.HasValue;
    }
}