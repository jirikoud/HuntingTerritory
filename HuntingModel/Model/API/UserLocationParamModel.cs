using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class UserLocationParamModel
    {
        public int TrackId { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
    }

}
