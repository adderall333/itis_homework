using System;
using System.Linq.Expressions;

namespace MyExpressionVisitor
{
    public static class MyExpressionVisitor
    {
        public static Expression Visit(dynamic node)
        {
            return InnerVisit(node);
        }

        private static Expression InnerVisit(ConstantExpression node)
        {
            return node;
        }
        
        private static Expression InnerVisit(ParameterExpression node)
        {
            return node;
        }
        
        private static Expression InnerVisit(BinaryExpression node)
        {
            // Walk children in evaluation order: left, conversion, right
            return ValidateBinary(
                node,
                node.Update(
                    Visit(node.Left),
                    VisitAndConvert(node.Conversion),
                    Visit(node.Right)
                )
            );
        }

        //ну, и так далее...
        private static Expression InnerVisit(Expression node)
        {
            throw new NotImplementedException();
        }
        
        
        //чтобы всё работало, надо было добавить это:
        private static BinaryExpression ValidateBinary(BinaryExpression before, BinaryExpression after)
        {
            if (before != after && before.Method == null)
            {
                if (after.Method != null)
                {
                    //throw Error.MustRewriteWithoutMethod(after.Method, nameof(VisitBinary));
                    throw new Exception();
                }

                ValidateChildType(before.Left.Type, after.Left.Type);
                ValidateChildType(before.Right.Type, after.Right.Type);
            }
            return after;
        }
        
        private static void ValidateChildType(Type before, Type after)
        {
            if (before.IsValueType)
            {
                if (before == after)
                {
                    // types are the same value type
                    return;
                }
            }
            else if (!after.IsValueType)
            {
                // both are reference types
                return;
            }

            // Otherwise, it's an invalid type change
            //throw Error.MustRewriteChildToSameType(before, after, methodName);
            throw new Exception();
        }
        
        private static T VisitAndConvert<T>(T node) where T : Expression
        {
            if (node == null)
            {
                return null;
            }
            node = Visit(node) as T;
            if (node == null)
            {
                //throw Error.MustRewriteToSameNode(callerName, typeof(T), callerName);
                throw new Exception();
            }
            return node;
        }
    }
}