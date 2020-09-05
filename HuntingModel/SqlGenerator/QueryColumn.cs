using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuntingModel.SqlGenerator
{
    public class QueryColumn
    {
        public string Code { get; set; }
        public string Query { get; set; }

        public QueryColumn()
        {
        }

        public QueryColumn(string code, string query)
        {
            this.Code = code;
            this.Query = query;
        }
    }
}
