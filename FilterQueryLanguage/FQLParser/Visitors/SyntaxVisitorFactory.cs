using System;
using FilterQueryLanguage.FQLParser.Parser;

namespace FilterQueryLanguage.FQLParser.Visitors
{
    public static class SyntaxVisitorFactory
    {
        public static ISyntaxVisitor GetSyntaxVisitor(TranspilationTarget targetLanguage)
        {
            switch (targetLanguage)
            {
                case TranspilationTarget.MsSql:
                    return new MsSqlSyntaxVisitor();
                case TranspilationTarget.ElasticSearch:
                    return new ElasticSearchSyntaxVisitor();
                case TranspilationTarget.MySql:
                    return new MySqlSyntaxVisitor();
            }
            throw new ApplicationException("Invalid target language spcecified");
        }
    }
}