using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Database
{
    partial class EmailInfo
    {
        public SendStateEnum SendStateEx
        {
            get
            {
                return (SendStateEnum)this.SendState;
            }
            set
            {
                this.SendState = (int)value;
            }
        }
    }
}
