using Sprache;

namespace FilterQueryLanguage.FQLParser.Parser
{
    public interface IFQLTranspiler
    {
        string Transpile(FQLTranspilerOptions options);
    }

    public class FQLTranspiler : IFQLTranspiler
    {
        public string Transpile(FQLTranspilerOptions options)
        {
            var grammar = new FQLGrammar();
            var logical = grammar.SyntaxTree.Parse(options.Query);
            var visitor = Visitors.SyntaxVisitorFactory.GetSyntaxVisitor(options.TranspilationTarget);
            visitor.Visit(logical);
            return visitor.GetQuery(options.VisitorOptions);
        }
    }
}