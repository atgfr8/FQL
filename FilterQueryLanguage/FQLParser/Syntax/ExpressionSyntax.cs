using System;
using System.Collections.Generic;
using FilterQueryLanguage.FQLParser.Visitors;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public class ExpressionSyntax : BaseSyntax
    {
        public string Field { get; set; }
        public FilterQueryOperator Operator { get; set; } 
        public string FieldValue { get; set; }

        public override void Accept(BaseSyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// This readonly method checks to see if the operator is:
        /// <para>greaterThan, greaterThanOrEqualTo, lessThan, or lessThanOrEqualTo </para>
        /// </summary>
        public bool OperatorIsGreaterThanLessThanEqualTo => 
            Operator == FilterQueryOperator.greaterThan ||
            Operator == FilterQueryOperator.greaterThanOrEqualTo ||
            Operator == FilterQueryOperator.lessThan ||
            Operator == FilterQueryOperator.lessThanOrEqualTo;
    }
}