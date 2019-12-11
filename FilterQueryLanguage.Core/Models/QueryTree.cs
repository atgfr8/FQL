namespace FilterQueryLanguage.Core.Models
{
     public class QueryTree
    {
        private QueryNode _root;
        public QueryTree()
        {
            _root = null;
        }

        public void Insert(FilterQueryLogicalOperator node)
        {
            if (_root == null)
            {
                _root = new QueryNode(node);
                return;
            }
            //otherwise recurse down
            InsertRec(_root, new QueryNode(node));
        }

        public void Insert(PredicateModel node)
        {
            if (_root == null)
            {
                _root = new QueryNode(node);
                return;
            }
            //otherwise recurse down
            InsertRec(_root, new QueryNode(node));
        }

        public void Insert(QueryNode node)
        {
            if (_root == null)
            {
                _root = node;
                return;
            }
            //otherwise recurse down
            InsertRec(_root, node);
        }

        private void InsertRec(QueryNode root, QueryNode newNode)
        {
            if (root == null)
            {
                root = newNode;
            }

            if (root.LogicalOperatorNode && newNode.LogicalOperatorNode && root.Left != null && root.Right != null)
            {
                _root = newNode;
                _root.Left = root;
            }

            if (root.LogicalOperatorNode && newNode.LogicalOperatorNode && root.Left != null && root.Right == null)
            {
                _root.Right = newNode;
            }

            if (root.PredicateNode && newNode.LogicalOperatorNode)
            {
                InsertRec(newNode, root);
            }


            if (root.LogicalOperatorNode && newNode.PredicateNode && root.Left == null)
            {
                root.Left = newNode;
                _root = root;
                return;
            }

            if (root.LogicalOperatorNode && newNode.PredicateNode && root.Left != null && root.Right == null)
            {
                root.Right = newNode;
                return;
            }

        }

        public QueryNode GetRoot()
        {
            return _root;
        }
    }

    public class QueryNode
    {
        public FilterQueryLogicalOperator? Operator { get; set; }
        public PredicateModel Predicate { get; set; }
        public QueryNode Right { get; set; }
        public QueryNode Left { get; set; }

        public bool LogicalOperatorNode => Operator.HasValue;
        public bool PredicateNode => Predicate != null;

        public QueryNode(FilterQueryLogicalOperator op)
        {
            Operator = op;
        }

        public QueryNode(PredicateModel predicate)
        {
            Predicate = predicate;
        }
    }
}