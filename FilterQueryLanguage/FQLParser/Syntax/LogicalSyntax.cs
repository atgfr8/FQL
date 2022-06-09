using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.FQLParser.Syntax 
{
    public class LogicalSyntax : BaseSyntax
    {
        public FilterQueryLogicalOperator Value {get;set;}

        public override void Accept(BaseSyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}