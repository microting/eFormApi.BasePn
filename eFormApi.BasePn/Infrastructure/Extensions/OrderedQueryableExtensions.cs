/*
The MIT License (MIT)
Copyright (c) 2007 - 2021 Microting A/S
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Microting.eFormApi.BasePn.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public static class OrderedQueryableExtensions
    {
        public static IOrderedQueryable<TSource> CustomOrderBy<TSource>(
            this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);
            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] {arg});

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2; //overload that has 2 parameters
                }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            var genericMethod = method
                .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>) genericMethod
                .Invoke(genericMethod, new object[] {query, selector});
            return newQuery;
        }

        public static IOrderedQueryable<TSource> CustomOrderByDescending<TSource>(
            this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);
            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] {arg});

            //Get System.Linq.Queryable.OrderByDescending() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2; //overload that has 2 parameters
                }).Single();
            //The linq's OrderByDescending<TSource, TKey> has two generic types, which provided here
            var genericMethod = method
                .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderByDescending(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>) genericMethod
                .Invoke(genericMethod, new object[] {query, selector});
            return newQuery;
        }

        /// <summary>
        /// Adds a search for the specified field names to the query. Example of the final value:<br></br>
        /// <code>query.Where(x => x.Name.Contains(filter) || x.Description.Contains(filter))</code>
        /// </summary>
        /// <typeparam name="TSource">Type query</typeparam>
        /// <param name="query">The query to which you want to add a search for the specified fields</param>
        /// <param name="propertyNames">Names of fields to add a search for</param>
        /// <param name="filter">Search value</param>
        /// <returns>Query with added search values for several fields</returns>
        public static IQueryable<TSource> CustomFiltering<TSource>(
            this IQueryable<TSource> query, IEnumerable<string> propertyNames, string filter)
        {
            var expressions = new List<Expression>();

            // create parametr with name p and type TSourse
            var parameter = Expression.Parameter(typeof(TSource), "x");

            // get method **Contains()** on type **string**
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });


            // get method **ToUpper()** on type **string**
            var toUpperMethod = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);

            // get method **ToString()** on type **DateTime?**
            var nullebleDateTimeToStringMethod = typeof(DateTime?).GetMethod("ToString", Type.EmptyTypes);

            // get method **ToString()** on type **DateTime**
            var dateTimeToStringMethod = typeof(DateTime).GetMethod("ToString", Type.EmptyTypes);

            // get method **ToString()** on type **int?**
            var nullebleIntToStringMethod = typeof(int?).GetMethod("ToString", Type.EmptyTypes);

            // get method **ToString()** on type **int**
            var intToStringMethod = typeof(int).GetMethod("ToString", Type.EmptyTypes);

            // foreach on all fields on entity type and take only string type fields and fields, whose names were transmitted
            foreach (var prop in typeof(TSource).GetProperties().Where(x =>
                         propertyNames.Contains(x.Name) && (x.PropertyType == typeof(string) ||
                                                            x.PropertyType == typeof(int?) ||
                                                            x.PropertyType == typeof(DateTime?) ||
                                                            x.PropertyType == typeof(int) ||
                                                            x.PropertyType == typeof(DateTime))))
            {
                // x => x.propName
                var memberExpression = Expression.PropertyOrField(parameter, prop.Name);

                // if type prop not string - need to call ToString()
                MethodCallExpression toStringCall;
                if (prop.PropertyType == typeof(DateTime))
                {
                    // x.propName.ToString()
                    toStringCall = Expression.Call(memberExpression,dateTimeToStringMethod);
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    // x.propName.ToString()
                    toStringCall = Expression.Call(memberExpression, nullebleDateTimeToStringMethod);
                }
                else if (prop.PropertyType == typeof(int?))
                {
                    // x.propName.ToString()
                    toStringCall = Expression.Call(memberExpression, nullebleIntToStringMethod);
                }
                else if (prop.PropertyType == typeof(int))
                {
                    // x.propName.ToString()
                    toStringCall = Expression.Call(memberExpression, intToStringMethod);
                }
                else
                {
                    // prop is string. not need call ToString()
                    toStringCall = null;
                }

                // string valueExpression = filter;
                var valueExpression = Expression.Constant(filter.ToUpper(), typeof(string));

                // x.propName(x.propName is string ? none : .ToString()).ToUpper()
                var toUpperExpression = Expression.Call(toStringCall == null ? memberExpression : toStringCall, toUpperMethod);

                // x.propName(x.propName is string ? none : .ToString()).ToUpper().Contains(filter.ToUpper())
                var containsExpression = Expression.Call(toUpperExpression, containsMethod, valueExpression);

                expressions.Add(containsExpression);
            }

            // if not need filtration - return
            if (expressions.Count == 0)
            {
                return query;
            }

            // assing first expression
            var orExpression = expressions[0];

            // and start from 1 add contains like x.Name.Contains(filter) || x.Description.Contains(filter) || ...
            for (var i = 1; i < expressions.Count; i++)
            {
                orExpression = Expression.OrElse(orExpression, expressions[i]);
            }

            // x.Name or x.Description - **example**
            // x => x.Name.Contains(filter) || x.Description.Contains(filter) || ...
            var expression = Expression.Lambda<Func<TSource, bool>>(
                orExpression, parameter);

            // query.Where(x => x.Name.Contains(filter) || x.Description.Contains(filter) || ...)
            return query.Where(expression);

        }

    }
}