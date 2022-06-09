using FilterQueryLanguage.FQLParser.Syntax;
using System;
using System.Collections.Generic;

namespace FilterQueryLanguage.FQLParser.Visitors
{
    public interface ISyntaxVisitor 
    {
        string GetQuery(IEnumerable<SyntaxVisitorOption> options);
        void Visit(StatementSyntax node);
        void Visit(ExpressionSyntax node);
    }

    public abstract class BaseSyntaxVisitor : ISyntaxVisitor
    {
        protected virtual void DefaultVisit(BaseSyntax node)
        {
        }

        public virtual void Visit(ExpressionSyntax node)
        {
            DefaultVisit(node);
        }

        public virtual void Visit(StatementSyntax node)
        {
            DefaultVisit(node);
        }

        public virtual void Visit(LogicalSyntax node)
        {
            DefaultVisit(node);
        }

        public abstract string GetQuery(IEnumerable<SyntaxVisitorOption> options);
    }
}