using FilterQueryLanguage.FQLParser.Parser;
using FilterQueryLanguage.FQLParser.Syntax;
using Sprache;

namespace FilterQueryLanguage.Test;

public class GrammarTests
{
    [Fact]
    public void TestSimpleExpression()
    {
        const string expressionToTest = "( Name notEqual 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Null(letsGo.Operator);
        Assert.Equal("James", letsGo.Expression.FieldValue);
        Assert.Equal("Name", letsGo.Expression.Field);
    }

    [Fact]
    public void Test2SimpleExpressionWithAnd()
    {
        const string expressionToTest = "( Name notEqual 'James' ) and ( Name notEqual 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Equal("and", letsGo.Operator.ToString());
        Assert.Equal("James", letsGo.LeftStatement.Expression.FieldValue);
        Assert.Equal("Name", letsGo.RightStatement.Expression.Field);
    }

    [Fact]
    public void Test2SimpleExpressionWithOr()
    {
        const string expressionToTest = "( Name notEqual 'James' ) or ( Name notEqual 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Equal("or", letsGo.Operator.ToString());
    }


    [Fact]
    public void Test3SimpleExpressionWithOr()
    {
        const string expressionToTest = "( Name notEqual 'James' ) or ( Name notEqual 'James' ) or ( Name notEqual 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Equal("or", letsGo.Operator.ToString());
        Assert.Equal("or", letsGo.LeftStatement.Operator.ToString());
    }

    [Fact]
    public void TestAndLogical()
    {
        const string expressionToTest = "and";
        Assert.Equal(FilterQueryLogicalOperator.and, new FQLGrammar().LogicalType.Parse(expressionToTest).Value);
    }

    [Fact]
    public void TestOrLogical()
    {
        const string expressionToTest = "or";
        Assert.Equal(FilterQueryLogicalOperator.or, new FQLGrammar().LogicalType.Parse(expressionToTest).Value);
    }

    [Fact]
    public void TestSimpleExpressionWithGreaterThanOperator()
    {
        const string expressionToTest = "( Name gt 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Null(letsGo.Operator);
        Assert.Equal("James", letsGo.Expression.FieldValue);
        Assert.Equal("Name", letsGo.Expression.Field);
        Assert.Equal(FilterQueryOperator.greaterThan, letsGo.Expression.Operator);
    }

    [Fact]
    public void TestSimpleExpressionWithGreaterThanOrEqualOperator()
    {
        const string expressionToTest = "( Name gte 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Null(letsGo.Operator);
        Assert.Equal("James", letsGo.Expression.FieldValue);
        Assert.Equal("Name", letsGo.Expression.Field);
        Assert.Equal(FilterQueryOperator.greaterThanOrEqualTo, letsGo.Expression.Operator);
    }

    [Fact]
    public void TestSimpleExpressionWithLessThanOperator()
    {
        const string expressionToTest = "( Name lt 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Null(letsGo.Operator);
        Assert.Equal("James", letsGo.Expression.FieldValue);
        Assert.Equal("Name", letsGo.Expression.Field);
        Assert.Equal(FilterQueryOperator.lessThan, letsGo.Expression.Operator);
    }

    [Fact]
    public void TestSimpleExpressionWithLessThanOrEqualToOperator()
    {
        const string expressionToTest = "( Name lte 'James' )";
        var letsGo = new FQLGrammar().SyntaxTree.Parse(expressionToTest);
        Assert.Null(letsGo.Operator);
        Assert.Equal("James", letsGo.Expression.FieldValue);
        Assert.Equal("Name", letsGo.Expression.Field);
        Assert.Equal(FilterQueryOperator.lessThanOrEqualTo, letsGo.Expression.Operator);
    }
}