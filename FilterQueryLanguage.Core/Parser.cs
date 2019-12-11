using System;
using System.Collections.Generic;
using System.Linq;
using FilterQueryLanguage.Core.Models;
using FilterQueryLanguage.Core.Tokens;

namespace FilterQueryLanguage.Core
{
    internal class Parser
    {
        private Stack<Token> _tokenSequence;
        private Token _lookaheadFirst;
        private Token _lookaheadSecond;
        private IList<QueryTree> _queryTree;

        public QueryTree Parse(List<Token> tokens)
        {
            LoadSequenceStack(tokens);
            PrepareLookaheads();
            _queryTree = new List<QueryTree>();

            Find();
            DiscardToken(TokenType.SequenceTerminator);

            var qtToReturn = new QueryTree();
            foreach (var qt in _queryTree)
            {
                qtToReturn.Insert(qt.GetRoot());
            }

            return qtToReturn;
        }

        private void Find()
        {
            DiscardToken(TokenType.Find);
            FindCondition();
        }

        private void FindCondition()
        {
            var queryTree = new QueryTree();

            if (_lookaheadFirst.TokenType == TokenType.OpenParenthesis)
            {
                DiscardToken();
                SubQueryCondition(queryTree);
                DiscardToken();
                _queryTree.Add(queryTree);
            }

            var op = GetLogicalOperator(_lookaheadFirst);
            if (op != null)
            {
                var singleNodeQT = new QueryTree();
                singleNodeQT.Insert(op.Value);
                _queryTree.Add(singleNodeQT);
                DiscardToken();
                FindCondition();
            }
        }

        private FilterQueryLogicalOperator? GetLogicalOperator(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.And:
                    return FilterQueryLogicalOperator.and;
                case TokenType.Or:
                    return FilterQueryLogicalOperator.or;
                default:
                    return null;
            }
        }

        private void SubQueryCondition(QueryTree queryTree)
        {
            PredicateCondition(queryTree);
            if (_lookaheadFirst.TokenType == TokenType.And)
            {
                queryTree.Insert(FilterQueryLogicalOperator.and);
                DiscardToken();
                SubQueryCondition(queryTree);
            }

            if (_lookaheadFirst.TokenType == TokenType.Or)
            {
                queryTree.Insert(FilterQueryLogicalOperator.or);
                DiscardToken();
                SubQueryCondition(queryTree);
            }
        }

        private void PredicateCondition(QueryTree queryTree)
        {
            var predicate = new PredicateModel();
            if (_lookaheadFirst.Value == null) throw new ApplicationException("Illegal query detected, enclose strings with a beginning ' and ending '");
            predicate.Field = _lookaheadFirst.Value;
            DiscardToken();
            predicate.Operator = GetOperator(_lookaheadFirst);
            DiscardToken();
            predicate.Value = _lookaheadFirst.Value;
            DiscardToken();
            queryTree.Insert(predicate);
        }

        private void LoadSequenceStack(List<Token> tokens)
        {
            _tokenSequence = new Stack<Token>();
            int count = tokens.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                _tokenSequence.Push(tokens[i]);
            }
        }

        private FilterQueryOperator GetOperator(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Contains:
                    return FilterQueryOperator.contains;
                case TokenType.StartsWith:
                    return FilterQueryOperator.startsWith;
                case TokenType.Equal:
                    return FilterQueryOperator.equal;
                case TokenType.NotEqual:
                    return FilterQueryOperator.notEqual;
                default:
                    throw new ApplicationException($"Expected contains or startsWith or equals but found {token.Value}");
            }
        }

        private void PrepareLookaheads()
        {
            _lookaheadFirst = _tokenSequence.Pop();
            _lookaheadSecond = _tokenSequence.Pop();
        }

        private Token ReadToken(TokenType tokenType)
        {
            if (_lookaheadFirst.TokenType != tokenType)
                throw new ApplicationException($"Expected {tokenType.ToString().ToUpper()} but found: {_lookaheadFirst.Value}");

            return _lookaheadFirst;
        }

        private void DiscardToken()
        {
            _lookaheadFirst = _lookaheadSecond.Clone();

            if (_tokenSequence.Any())
                _lookaheadSecond = _tokenSequence.Pop();
            else
                _lookaheadSecond = new Token(TokenType.SequenceTerminator, string.Empty);
        }

        private void DiscardToken(TokenType tokenType)
        {
            if (_lookaheadFirst.TokenType != tokenType)
                throw new ApplicationException($"Expected {tokenType.ToString().ToUpper()} but found: {_lookaheadFirst.Value}");

            DiscardToken();
        }

    }
}