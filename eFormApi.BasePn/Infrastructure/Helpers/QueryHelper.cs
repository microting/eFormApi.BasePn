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

namespace Microting.eFormApi.BasePn.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using Extensions;
    using System.Linq;

    public class QueryHelper
    {
        public static IQueryable<T> AddSortToQuery<T>(
            IQueryable<T> query,
            string sort,
            bool isSortDsc,
            IList<string> excludeSort = null)
        {
            var skipSort = false;

            if (excludeSort != null)
            {
                skipSort = excludeSort.Any(x => x == sort);
            }

            if (!string.IsNullOrEmpty(sort) && !skipSort)
            {
                if (isSortDsc)
                {
                    query = query
                        .CustomOrderByDescending(sort);
                }
                else
                {
                    query = query
                        .CustomOrderBy(sort);
                }
            }
            else
            {
                query = query
                        .CustomOrderBy("Id");
            }

            return query;
        }


        /// <summary>
        /// Adds a search for the specified field names to the query. Example of the final value:<br></br>
        /// <code>query.Where(x => x.Name.Contains(filter) || x.Description.Contains(filter))</code>
        /// </summary>
        /// <typeparam name="T">Type query</typeparam>
        /// <param name="query">The query to which you want to add a search for the specified fields</param>
        /// <param name="nameFields">Names of fields to add a search for</param>
        /// <param name="filter">Search value</param>
        /// <returns>Query with added search values for several fields</returns>
        public static IQueryable<T> AddFilterToQuery<T>(
            IQueryable<T> query,
            IEnumerable<string> nameFields,
            string filter)
        {
            if(!string.IsNullOrEmpty(filter))
            {
                query = query.CustomFiltering(nameFields, filter);
            }
            return query;
        }
    }
}