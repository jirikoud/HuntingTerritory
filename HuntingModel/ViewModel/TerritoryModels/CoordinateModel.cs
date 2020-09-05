using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class CoordinateModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public CoordinateModel()
        {
        }

        public CoordinateModel(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", Lat.ToString(CultureInfo.InvariantCulture), Lng.ToString(CultureInfo.InvariantCulture));
        }

        public string ToLatLngString()
        {
            return string.Format("\tnew google.maps.LatLng({0},{1})", Lat.ToString(CultureInfo.InvariantCulture), Lng.ToString(CultureInfo.InvariantCulture));
        }

    }

}
