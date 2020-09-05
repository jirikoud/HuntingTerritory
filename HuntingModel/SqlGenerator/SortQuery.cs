using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuntingModel.SqlGenerator
{
    public class SortQuery
    {
        public List<string> QueryList { get; set; }
        public bool IsDesc { get; set; }

        public SortQuery(string query, bool isDesc = false, params string[] queries)
        {
            this.QueryList = new List<string>() { query };
            if (queries != null)
            {
                this.QueryList.AddRange(queries);
            }
            this.IsDesc = isDesc;
        }

        public override string ToString()
        {
            var list = this.IsDesc ? this.QueryList.Select(item => item + " DESC") : this.QueryList;
            return string.Join(",", list);
        }
    }
}
