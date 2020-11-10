using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class EmailInfo
    {
        public long Id { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int RetryCount { get; set; }
        public int SendState { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? SendDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
        public DateTime? SysUpdated { get; set; }
        public long? SysEditor { get; set; }
    }
}
