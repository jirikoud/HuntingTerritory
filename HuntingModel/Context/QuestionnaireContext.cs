using HuntingModel.Database;
using HuntingModel.ViewModel.QuestionnaireModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.Context
{
    public class QuestionnaireContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<Questionnaire> GetList(HuntingEntities dataContext, MapItemType mapItemType)
        {
            try
            {
                var itemTypeList = dataContext.Questionnaires.
                    Where(item => item.MapItemTypeId == mapItemType.Id && item.IsDeleted == false).
                    OrderBy(item => item.Name).
                    ToList();
                return itemTypeList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetList");
                return null;
            }
        }

        public static SelectList GetSelectList(HuntingEntities dataContext, MapItemType mapItemType, int? questionnaireId)
        {
            try
            {
                var questionnaireList = GetList(dataContext, mapItemType);
                var selectList = new SelectList(questionnaireList, "Id", "Name", questionnaireId);
                return selectList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetSelectList");
                return null;
            }
        }

        public static Questionnaire GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var questionnaire = dataContext.Questionnaires.FirstOrDefault(item => item.Id == id && item.IsDeleted == false);
                return questionnaire;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
                return null;
            }
        }

        public static int? Update(HuntingEntities dataContext, Questionnaire updateItem, QuestionnaireUpdateModel model, AclUser user)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new Questionnaire()
                    {
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                        MapItemTypeId = model.MapItemTypeId,
                    };
                    dataContext.Questionnaires.Add(updateItem);
                }

                updateItem.Name = model.Name;
                updateItem.Description = model.Description;
                updateItem.SysEditor = user.Id;
                updateItem.SysUpdated = DateTime.Now;

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Update");
                return null;
            }
        }

        public static bool Delete(HuntingEntities dataContext, Questionnaire updateItem, AclUser user)
        {
            try
            {
                updateItem.IsDeleted = true;
                updateItem.SysEditor = user.Id;
                updateItem.SysUpdated = DateTime.Now;
                foreach (var question in updateItem.Questions)
                {
                    question.IsDeleted = true;
                    question.SysEditor = user.Id;
                    question.SysUpdated = DateTime.Now;
                }
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Delete");
                return false;
            }
        }
    }
}
