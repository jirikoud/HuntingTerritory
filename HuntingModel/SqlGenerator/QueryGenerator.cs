using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace HuntingModel.SqlGenerator
{
    public class QueryGenerator
    {
        public static SqlParameter CreateParameter(string name, object value)
        {
            SqlParameter sqlParam;
            if (value == null)
            {
                sqlParam = new SqlParameter(name, DBNull.Value);
            }
            else
            {
                sqlParam = new SqlParameter(name, value);
            }
            return sqlParam;
        }

        private static string GetSortQuery(List<SortQuery> sortList)
        {
            var sortQuery = string.Join(",", sortList);
            return sortQuery;
        }

        private static string GetFilterString(IGenerator generator, int pageSize)
        {
            string returnString = string.Join(", ", generator.ReturnFields.Select(item => string.Format("[Inner].[{0}]", item.Name)));
            string selectString = string.Join(", ", generator.ReturnFields.Select(item => string.Format("{0} AS [{1}]", item.Definition, item.Name)));
            string whereString = string.Join(" AND ", generator.WhereList);
            string command = string.Format(@"
                {0}
                DECLARE @PageSize int = {1};
			    SELECT TOP (@PageSize) {2} FROM 
			    (
				    SELECT {3}, row_number() OVER (ORDER BY {4}) AS [Row_Number] 
				    FROM 
                    {5}
				    WHERE {6}
    			) AS [Inner]
	    		WHERE [Inner].[Row_Number] > @SkipCount 
		    	ORDER BY [Row_Number]",
                generator.InitLines, pageSize, returnString, selectString, GetSortQuery(generator.Sort), generator.From(false), whereString);
            return command;
        }

        private static string GetLinkedString(IGenerator generator)
        {
            string selectString = string.Join(", ", generator.ReturnFields.Select(item => string.Format("{0} AS [{1}]", item.Definition, item.Name)));
            string whereString = string.Join(" AND ", generator.WhereList);
            string command = string.Format(@"
                {0}
				SELECT {1}
				FROM 
                {2}
				WHERE {3}
		    	ORDER BY {4}",
                generator.InitLines, selectString, generator.From(false), whereString, GetSortQuery(generator.Sort));
            return command;
        }

        private static string GetTotalCountString(IGenerator generator)
        {
            string whereString = string.Join(" AND ", generator.WhereList);
            string command = string.Format(@"
                {0}
			    SELECT COUNT(*) 
                FROM {1}
				WHERE {2}",
                generator.InitLines, generator.From(true), whereString);
            return command;
        }

        public static List<T> ReadPagedList<T>(DbContext context, IGenerator generator, int pageIndex, int pageSize = Constants.DEFAULT_LIST_PAGE_SIZE)
        {
            var queryParams = generator.QueryParams;
            var paging = ContextUtils.GetPaging(pageIndex, pageSize, false);
            queryParams.Add(new SqlParameter("@SkipCount", paging.SkipCount));
            var query = GetFilterString(generator, paging.PageSize);
            var list = (context as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<T>(query, queryParams.ToArray());
            return list.ToList();
        }

        public static List<T> ReadList<T>(DbContext context, IGenerator generator)
        {
            var queryParams = generator.QueryParams;
            var query = GetLinkedString(generator);
            var list = (context as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<T>(query, queryParams.ToArray());
            return list.ToList();
        }

        public static int GetTotalCount(DbContext context, IGenerator generator)
        {
            var queryParams = generator.QueryParams;
            var query = GetTotalCountString(generator);
            var result = (context as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<int>(query, queryParams.ToArray());
            var count = result.FirstOrDefault();
            return count;
        }

    }
}
