using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator.Generators
{
    public class TerritoryListGenerator : IGenerator
    {
        public const string COLUMN_ID = "Id";
        public const string COLUMN_NAME = "Name";
        public const string COLUMN_USER_COUNT = "UserCount";
        public const string COLUMN_STEWARD = "Steward";
        public const string COLUMN_CONTACT_COUNT = "ContactCount";

        internal TerritoryFilter innerFilter;
        private string fulltext;
        private static List<QueryColumn> columnList;
        private int innerUserId;

        public static List<QueryColumn> QueryColumnList
        {
            get
            {
                if (columnList == null)
                {
                    columnList = new List<QueryColumn>();
                    columnList.Add(new QueryColumn(COLUMN_ID, "tr.[Id]"));
                    columnList.Add(new QueryColumn(COLUMN_NAME, "tr.[Name]"));
                    columnList.Add(new QueryColumn(COLUMN_USER_COUNT, "(SELECT COUNT(*) FROM [TerritoryUser] tu WHERE tu.[TerritoryId] = tr.[Id])"));
                    columnList.Add(new QueryColumn(COLUMN_STEWARD, "au.[Fullname]"));
                    columnList.Add(new QueryColumn(COLUMN_CONTACT_COUNT, "(SELECT COUNT(*) FROM [TerritoryUserContact] tc WHERE tc.[TerritoryId] = tr.[Id] AND tc.[AclUserId] = @UserId AND tc.[IsDeleted] = 0)"));
                }
                return columnList;
            }
        }

        public TerritoryListGenerator(TerritoryFilter filter, int userId)
        {
            this.innerFilter = filter;
            fulltext = ContextUtils.GetFulltextString(filter.Fulltext);
            this.innerUserId = userId;
        }

        public virtual List<ReturnField> ReturnFields
        {
            get
            {
                var returnFields = new List<ReturnField>();
                returnFields = QueryColumnList.Select(item =>
                    new ReturnField() { Name = item.Code, Definition = item.Query }).ToList();
                return returnFields;
            }
        }

        public virtual string InitLines
        {
            get
            {
                var init = string.Empty;
                return init;
            }
        }

        public virtual string From(bool isCount)
        {
            var from = @"
                    dbo.[Territory] tr
                    JOIN dbo.[AclUser] au ON au.[Id] = tr.[StewardId]  
                    ";
            if (string.IsNullOrWhiteSpace(fulltext) == false)
            {
                from += @"
				INNER JOIN CONTAINSTABLE(dbo.[Territory], (*), @Keywords) ft ON ft.[KEY] = tr.[Id]
                ";
            }
            return from;
        }

        public virtual List<SortQuery> Sort
        {
            get
            {
                var sortList = new List<SortQuery>();
                var sortField = new SortQuery("tr.[Id]");
                switch (innerFilter.SortField)
                {
                    case TerritoryFilter.SORT_NAME:
                        sortField = new SortQuery("tr.[Name]", innerFilter.SortIsDesc);
                        break;
                    case FulltextFilterBase.SORT_RANK:
                        sortField = new SortQuery("ft.[Rank]", innerFilter.SortIsDesc);
                        break;
                }
                sortList.Add(sortField);
                return sortList;
            }
        }

        public virtual List<string> WhereList
        {
            get
            {
                var whereList = new List<string>();
                whereList.Add(@"tr.[IsDeleted] = 0");
                if (innerFilter.IsContact)
                {
                    whereList.Add(@"tr.[IsPublic] = 1");
                    whereList.Add(@"tr.[StewardId] != @UserId");
                }
                return whereList;
            }
        }

        public virtual List<SqlParameter> QueryParams
        {
            get
            {
                var queryParams = new List<SqlParameter>();
                queryParams.Add(QueryGenerator.CreateParameter("@UserId", this.innerUserId));
                if (string.IsNullOrWhiteSpace(innerFilter.Fulltext) == false)
                {
                    queryParams.Add(QueryGenerator.CreateParameter("@Keywords", fulltext));
                }
                return queryParams;
            }
        }
    }
}
