using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator.Generators
{
    public class AclUserListGenerator : IGenerator
    {
        public const string COLUMN_ID = "Id";
        public const string COLUMN_EMAIL = "Email";
        public const string COLUMN_FULLNAME = "Fullname";
        public const string COLUMN_IS_DISABLED = "IsDisabled";
        public const string COLUMN_CURRENT_TERRITORY_COUNT = "CurrentTerritoryCount";
        public const string COLUMN_MAX_TERRITORY_COUNT = "MaxTerritoryCount";
        public const string COLUMN_ACOCUNT_TYPE = "AccountType";

        internal AclUserFilter innerFilter;
        private string fulltext;
        private static List<QueryColumn> columnList;

        public static List<QueryColumn> QueryColumnList
        {
            get
            {
                if (columnList == null)
                {
                    columnList = new List<QueryColumn>();
                    columnList.Add(new QueryColumn(COLUMN_ID, "au.[Id]"));
                    columnList.Add(new QueryColumn(COLUMN_EMAIL, "au.[Email]"));
                    columnList.Add(new QueryColumn(COLUMN_FULLNAME, "au.[Fullname]"));
                    columnList.Add(new QueryColumn(COLUMN_IS_DISABLED, "au.[IsDisabled]"));
                    columnList.Add(new QueryColumn(COLUMN_CURRENT_TERRITORY_COUNT, "(SELECT COUNT(*) FROM [Territory] tr WHERE tr.[IsDeleted] = 0 AND tr.[StewardId] = au.[Id])"));
                    columnList.Add(new QueryColumn(COLUMN_MAX_TERRITORY_COUNT, "au.[MaxTerritoryCount]"));
                    columnList.Add(new QueryColumn(COLUMN_ACOCUNT_TYPE, "au.[AccountType]"));
                }
                return columnList;
            }
        }

        public AclUserListGenerator(AclUserFilter filter)
        {
            this.innerFilter = filter;
            fulltext = ContextUtils.GetFulltextString(filter.Fulltext);
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
                    dbo.[AclUser] au  
                    ";
            if (string.IsNullOrWhiteSpace(fulltext) == false)
            {
                from += @"
				INNER JOIN CONTAINSTABLE(dbo.[AclUser], (*), @Keywords) ft ON ft.[KEY] = au.[Id]
                ";
            }
            return from;
        }

        public virtual List<SortQuery> Sort
        {
            get
            {
                var sortList = new List<SortQuery>();
                var sortField = new SortQuery("au.[Id]");
                switch (innerFilter.SortField)
                {
                    case AclUserFilter.SORT_ACCOUNT_TYPE:
                        sortField = new SortQuery("au.[AccountType]", innerFilter.SortIsDesc);
                        break;
                    case AclUserFilter.SORT_EMAIL:
                        sortField = new SortQuery("au.[Email]", innerFilter.SortIsDesc);
                        break;
                    case AclUserFilter.SORT_FULLNAME:
                        sortField = new SortQuery("au.[Fullname]", innerFilter.SortIsDesc);
                        break;
                    case AclUserFilter.SORT_IS_DISABLED:
                        sortField = new SortQuery("au.[IsDisabled]", innerFilter.SortIsDesc);
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
                whereList.Add(@"au.[IsDeleted] = 0");
                if (innerFilter.AccoutType.HasValue)
                {
                    whereList.Add(string.Format(@"au.[AccountType] = {0}", (int)innerFilter.AccoutType.Value));
                }
                return whereList;
            }
        }

        public virtual List<SqlParameter> QueryParams
        {
            get
            {
                var queryParams = new List<SqlParameter>();
                if (string.IsNullOrWhiteSpace(innerFilter.Fulltext) == false)
                {
                    queryParams.Add(QueryGenerator.CreateParameter("@Keywords", fulltext));
                }
                return queryParams;
            }
        }
    }
}
