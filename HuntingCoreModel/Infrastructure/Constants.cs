using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingCoreModel.Infrastructure
{
    public class Constants
    {
        public const int DEFAULT_LIST_PAGE_SIZE = 10;
        public const int MAX_LIST_PAGE_SIZE = 100;
        public const int MAX_SEARCH_PAGE_SIZE = 100;

        public const int PAGER_LINK_COUNT = 5;
        public const string ACTION_STATE_COOKIE = "action-state";
        public const string COOKIE_VALUE_ACTION_TYPE = "type";
        public const string COOKIE_VALUE_MESSAGE = "message";
    }
}
