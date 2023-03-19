﻿namespace CS_Utils.Advanced.ExpressionVisitors;
public sealed class ExpressionWriterVisitor : ExpressionVisitor
{
    private readonly TextWriter _writer;

    public ExpressionWriterVisitor(TextWriter writer)
    {
        _writer = writer;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        _writer.Write(GetOperator(node.NodeType));
        Visit(node.Right);
        return node;
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        Visit(node.Test);
        _writer.Write('?');
        Visit(node.IfTrue);
        _writer.Write(':');
        Visit(node.IfFalse);
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        WriteConstantValue(node.Value);
        return node;
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        _writer.Write('(');
        _writer.Write(string.Join(',', node.Parameters.Select(param => param.Name)));
        _writer.Write(')');
        _writer.Write("=>");
        Visit(node.Body);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        // Closures are represented as a constant object with fields representing each closed over value.
        // This gets and prints the value of that closure.
        if (node.Member is FieldInfo fieldInfo && node.Expression is ConstantExpression constExpr)
            WriteConstantValue(fieldInfo.GetValue(constExpr.Value));
        else
        {
            Visit(node.Expression);
            _writer.Write('.');
            _writer.Write(node.Member.Name);
        }
        return node;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        _writer.Write(node.Name);
        return node;
    }

    private static string GetOperator(ExpressionType type)
    {
        return type switch
        {
            ExpressionType.Equal => "==",
            ExpressionType.Not => "!",
            ExpressionType.NotEqual => "!==",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.Or => "|",
            ExpressionType.OrElse => "||",
            ExpressionType.And => "&",
            ExpressionType.AndAlso => "&&",
            ExpressionType.Add => "+",
            ExpressionType.AddAssign => "+=",
            ExpressionType.Subtract => "-",
            ExpressionType.SubtractAssign => "-=",
            _ => "???",
        };
    }

    private void WriteConstantValue(object? obj)
    {
        switch (obj)
        {
            case string str:
                _writer.Write('"');
                _writer.Write(str);
                _writer.Write('"');
                break;

            default:
                _writer.Write(obj);
                break;
        }
    }
}