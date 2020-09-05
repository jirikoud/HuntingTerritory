using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class OptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public OptionModel(Option option)
        {
            this.Id = option.Id;
            this.Name = option.Name;
            this.Order = option.Order;
        }
    }
}
