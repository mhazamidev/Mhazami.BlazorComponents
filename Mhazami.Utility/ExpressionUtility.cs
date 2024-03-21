using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mhazami.Utility
{
   
  

    public class PredicateBuilder<T>
    {
        private Expression<Func<T, bool>> Expression;

        public void And(Expression<Func<T, bool>> func)
        {
            if (Expression == null)
                Expression = func;
            else
            {

                var invokedExpr = System.Linq.Expressions.Expression.Invoke(Expression, func.Parameters);
                Expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>
                    ((System.Linq.Expressions.Expression.AndAlso(func.Body, invokedExpr)), func.Parameters);

            }
        }

        public void Or(Expression<Func<T, bool>> func)
        {
            if (Expression == null)
                Expression = func;
            else
            {
                
                var invokedExpr = System.Linq.Expressions.Expression.Invoke(Expression, func.Parameters);
                Expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>
                    ((System.Linq.Expressions.Expression.OrElse(func.Body, invokedExpr)), func.Parameters);
            }
        }

        public IEnumerable<T> Where(IEnumerable<T> source)
        {
            var expression = GetExpression();
            return source.Where(expression.Compile());
        }

        public int Count(IEnumerable<T> source)
        {
            var expression = GetExpression();
            return source.Count(expression.Compile());
        }

        public T FirstOrDefault(IEnumerable<T> source)
        {
            var expression = GetExpression();
            return source.FirstOrDefault(expression.Compile());
        }
        public Expression<Func<T, bool>> GetExpression()
        {
            return Expression;
        }
       

      
    }


   


}


