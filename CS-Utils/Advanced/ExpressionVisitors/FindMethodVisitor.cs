﻿namespace CS_Utils.Advanced.ExpressionVisitors;
public sealed class FindMethodVisitor : ExpressionVisitorBase
{
    public FindMethodVisitor(Expression expression)
    {
        Visit(expression);
    }

    public MethodInfo? Method { get; private set; }

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
        Method = m.Method;
        return m;
    }
}