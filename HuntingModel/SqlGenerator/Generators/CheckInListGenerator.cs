using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator.Generators
{
    public class CheckInListGenerator : IGenerator
    {
        public const string COLUMN_ID = "Id";
        public const string COLUMN_USER_NAME = "UserName";
        public const string COLUMN_CHECKIN_TIME = "CheckInTime";
        public const string COLUMN_QUESTIONNAIRE = "QuestionnaireName";

        internal CheckInFilter innerFilter;
        private string fulltext;
        private static List<QueryColumn> columnList;

        public static List<QueryColumn> QueryColumnList
        {
            get
            {
                if (columnList == null)
                {
                    columnList = new List<QueryColumn>();
                    columnList.Add(new QueryColumn(COLUMN_ID, "che.[Id]"));
                    columnList.Add(new QueryColumn(COLUMN_USER_NAME, "au.[Email]"));
                    columnList.Add(new QueryColumn(COLUMN_CHECKIN_TIME, "che.[CheckTime]"));
                    columnList.Add(new QueryColumn(COLUMN_QUESTIONNAIRE, "qu.[Name]"));
                }
                return columnList;
            }
        }

        public CheckInListGenerator(CheckInFilter filter)
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
                    dbo.[CheckIn] che
                    INNER JOIN dbo.[AclUser] au ON au.[Id] = che.[SysCreator]
                    LEFT JOIN dbo.[Questionnaire] qu ON qu.[Id] = che.[QuestionnaireId]
                    ";
            if (string.IsNullOrWhiteSpace(fulltext) == false)
            {
                from += @"
				INNER JOIN CONTAINSTABLE(dbo.[CheckIn], (*), @Keywords) ft ON ft.[KEY] = che.[Id]
                ";
            }
            return from;
        }

        public virtual List<SortQuery> Sort
        {
            get
            {
                var sortList = new List<SortQuery>();
                var sortField = new SortQuery("che.[Id]");
                switch (innerFilter.SortField)
                {
                    case CheckInFilter.SORT_CHECKIN_TIME:
                        sortField = new SortQuery("che.[CheckTime]", innerFilter.SortIsDesc);
                        break;
                    case CheckInFilter.SORT_QUESTIONNAIRE:
                        sortField = new SortQuery("qu.[Name]", innerFilter.SortIsDesc);
                        break;
                    case CheckInFilter.SORT_USER_NAME:
                        sortField = new SortQuery("au.[Fullname]", innerFilter.SortIsDesc);
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
                whereList.Add(@"che.[IsDeleted] = 0");
                if (innerFilter.MapItemId.HasValue)
                {
                    whereList.Add(string.Format(@"che.[MapItemId] = {0}", (int)innerFilter.MapItemId.Value));
                }
                if (innerFilter.QuestionnaireId.HasValue)
                {
                    whereList.Add(string.Format(@"che.[QuestionnaireId] = {0}", (int)innerFilter.QuestionnaireId.Value));
                }
                if (innerFilter.AclUserId.HasValue)
                {
                    whereList.Add(string.Format(@"che.[SysCreator] = {0}", (int)innerFilter.AclUserId.Value));
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
