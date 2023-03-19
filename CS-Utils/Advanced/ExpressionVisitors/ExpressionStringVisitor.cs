using System.Text;

namespace CS_Utils.Advanced.ExpressionVisitors;
public class ExpressionStringVisitor : ExpressionVisitor
{
    private readonly StringBuilder _sb = new();

    public static string ConvertToString(Expression expression)
    {
        var visitor = new ExpressionStringVisitor();
        visitor.Visit(expression);
        return visitor._sb.ToString();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sb.Append("(");
        Visit(node.Left);
        _sb.Append(GetOperator(node.NodeType));
        Visit(node.Right);
        _sb.Append(")");
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _sb.Append(node.Value);
        return node;
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        _sb.Append(node.Parameters[0].Name);
        _sb.Append(" => ");
        Visit(node.Body);
        return node;
    }

    private static string GetOperator(ExpressionType nodeType)
    {
        return nodeType switch
        {
            ExpressionType.Add => " + ",
            ExpressionType.AndAlso => " && ",
            ExpressionType.Divide => " / ",
            ExpressionType.Equal => " == ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.GreaterThanOrEqual => " >= ",
            ExpressionType.LessThan => " < ",
            ExpressionType.LessThanOrEqual => " <= ",
            ExpressionType.Multiply => " * ",
            ExpressionType.NotEqual => " != ",
            ExpressionType.OrElse => " || ",
            ExpressionType.Subtract => " - ",
            _ => throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null),
        };
    }
}