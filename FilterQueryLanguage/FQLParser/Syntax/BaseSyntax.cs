using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public abstract class BaseSyntax
    {
        public abstract void Accept(BaseSyntaxVisitor visitor);
    }
}