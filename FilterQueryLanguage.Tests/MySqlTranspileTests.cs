using System.Collections.Generic;
using FilterQueryLanguage.FQLParser;
using FilterQueryLanguage.FQLParser.Parser;
using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.Test
{
    public class MySqlTranspileTests
    {
        [Fact]
        public void TestParserSimpleExpression()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "(Name notEqual 'Bob')", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE Name <> 'Bob';";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestParserExpressionWithTwoAnds()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "(Name notEqual 'Bob')  and (Name notEqual 'James') and (Name notEqual 'Brennan')", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE Name <> 'Bob' and Name <> 'James' and Name <> 'Brennan';";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestParserExpressionWithTwoParanthesis()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "((Name notEqual 'Bob'))", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE Name <> 'Bob';";
            Assert.Equal(whatItShouldbe, test);
        }


        [Fact]
        public void TestParserExpressionWithTwoAndsAndParanthesis()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "(Name notEqual 'Bob')  and ((Name notEqual 'James') and (Name notEqual 'Brennan'))", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE Name <> 'Bob' and (Name <> 'James' and Name <> 'Brennan');";
            Assert.Equal(whatItShouldbe, test);
        }


        [Fact]
        public void TestParserExpressionWithTwoAndsAndMoreParanthesis()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "(((Name notEqual 'Bob')  and (((Name notEqual 'James') and (Name notEqual 'Brennan')))))", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE Name <> 'Bob' and (Name <> 'James' and Name <> 'Brennan');";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestParserExpressionWithOneAndOneOrAndMoreParanthesis()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "(((Name notEqual 'Bob')  and (((Name notEqual 'James') or (Name notEqual 'Brennan')))))", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE Name <> 'Bob' and (Name <> 'James' or Name <> 'Brennan');";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestParserComplexStatementsWithManyParanthesis()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MySql, "((((Name notEqual 'Bob') and (Name notEqual 'Mike'))  and (((Name notEqual 'James') or (Name notEqual 'Brennan')))))", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE (Name <> 'Bob' and Name <> 'Mike') and (Name <> 'James' or Name <> 'Brennan');";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestDateComparisionWithGTEandLTE()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MsSql, "(ModifiedDate gte '01/26/1990') and (ModifiedDate lte '01/26/2022')", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE ModifiedDate >= '01/26/1990' and ModifiedDate <= '01/26/2022';";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestDateComparisionWithGTandLT()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MsSql, "(ModifiedDate gt '01/26/1990') and (ModifiedDate lt '01/26/2022')", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE ModifiedDate > '01/26/1990' and ModifiedDate < '01/26/2022';";
            Assert.Equal(whatItShouldbe, test);
        }

        [Fact]
        public void TestDateComparisionWithGTandLTE()
        {
            var parser = new FQLParser.Parser.FQLTranspiler();
            var options = new FQLTranspilerOptions(TranspilationTarget.MsSql, "(ModifiedDate gt '01/26/1990') and (ModifiedDate lte '01/26/2022')", new List<SyntaxVisitorOption> { new SyntaxVisitorOption { Name = "TableName", Value = "AspNet.Users" } });
            var test = parser.Transpile(options);
            var whatItShouldbe = "SELECT * FROM AspNet.Users WHERE ModifiedDate > '01/26/1990' and ModifiedDate <= '01/26/2022';";
            Assert.Equal(whatItShouldbe, test);
        }
    }
}