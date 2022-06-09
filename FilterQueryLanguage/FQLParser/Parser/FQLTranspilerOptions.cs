using System.Collections.Generic;
using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.FQLParser.Parser
{
    public class FQLTranspilerOptions
    {
        public FQLTranspilerOptions(TranspilationTarget transpilationTarget, string query, IEnumerable<SyntaxVisitorOption> options)
        {
            Query = query;
            VisitorOptions = options;
            TranspilationTarget = transpilationTarget;
        }

        public TranspilationTarget TranspilationTarget {get;}
        public string Query { get; }
        public IEnumerable<SyntaxVisitorOption> VisitorOptions {get;}
    }
}