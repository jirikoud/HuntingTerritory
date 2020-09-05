using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class MapItemTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<QuestionnaireModel> QuestionnaireList { get; set; }
        public List<MapItemModel> MapItemList { get; set; }

        public MapItemTypeModel(MapItemType mapItemType)
        {
            this.Id = mapItemType.Id;
            this.Name = mapItemType.Name;
            this.Description = mapItemType.Description;

            var questionnaireList = mapItemType.Questionnaires.Where(item => item.IsDeleted == false).ToList();
            this.QuestionnaireList = questionnaireList.ConvertAll(item => new QuestionnaireModel(item));

            var mapItemList = mapItemType.MapItems.Where(item => item.IsDeleted == false).ToList();
            this.MapItemList = mapItemList.ConvertAll(item => new MapItemModel(item));
        }
    }
}
