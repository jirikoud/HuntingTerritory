using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;

namespace HuntingModel.SqlGenerator
{
    public interface IGenerator
    {
        string InitLines { get; }
        List<ReturnField> ReturnFields { get; }
        List<string> WhereList { get; }
        List<SortQuery> Sort { get; }
        List<SqlParameter> QueryParams { get; }

        string From(bool isCount);
    }
}
