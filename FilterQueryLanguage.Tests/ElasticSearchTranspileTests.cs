using FilterQueryLanguage.FQLParser.Parser;

namespace FilterQueryLanguage.Test
{
    public class ElasticSearchTranspileTests
    {
        //beware, we dont have an elastic search setup to actually verify that this query works on it
        [Fact]
        public void TestParserTranspileToES()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.ElasticSearch, "((((Name notEqual 'Bob') and (Name notEqual 'Mike'))  and (((Name notEqual 'James') or (Name notEqual 'Brennan')))))", null);
            var test = parser.Transpile(options);
            Assert.NotEmpty(test);
        }

        [Fact]
        public void TestParserDateComparissionWithOrTranspileToES()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.ElasticSearch, "((Date gte '01/26/1990') and (Test notEqual 'Mike') or (Date lte '01/26/1970'))", null);
            var test = parser.Transpile(options);
            Assert.NotEmpty(test);
        }

        [Fact]
        public void TestParserDateComparissionWithAndTranspileToES()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.ElasticSearch, "((Date gte '01/26/1990') and (Test notEqual 'Mike') and (Date lte '01/26/1970'))", null);
            var test = parser.Transpile(options);
            Assert.NotEmpty(test);
        }
    }
}