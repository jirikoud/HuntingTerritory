using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.ViewModel.QuestionModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class QuestionContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<Question> GetList(HuntingEntities dataContext, Questionnaire questionnaire)
        {
            try
            {
                var questionList = questionnaire.Questions.
                    Where(item => item.IsDeleted == false).
                    OrderBy(item => item.Order).
                    ToList();
                return questionList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetQuestionList");
                return null;
            }
        }

        public static Question GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var question = dataContext.Questions.FirstOrDefault(item => item.Id == id && item.IsDeleted == false);
                return question;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
                return null;
            }
        }

        public static int? Update(HuntingEntities dataContext, Question updateItem, QuestionUpdateModel model, AclUser user)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new Question()
                    {
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                        QuestionnaireId = model.QuestionnaireId,
                    };
                    var maxOrder = dataContext.Questions.Where(item => item.QuestionnaireId == model.QuestionnaireId).Max(item => (int?)item.Order);
                    updateItem.Order = (maxOrder.HasValue ? maxOrder.Value + 1 : 1);
                    dataContext.Questions.Add(updateItem);
                }

                updateItem.Name = model.Name;
                updateItem.Description = model.Description;
                updateItem.QuestionTypeEx = model.QuestionType;

                if (model.QuestionType == QuestionTypeEnum.Checkbox || model.QuestionType == QuestionTypeEnum.CheckboxList)
                {
                    updateItem.IsRequired = false;
                }
                else
                {
                    updateItem.IsRequired = model.IsRequired;
                }

                int order = 1;
                var removeList = updateItem.Options.Where(item => item.IsDeleted == false).ToList();
                if (model.QuestionType == QuestionTypeEnum.CheckboxList || model.QuestionType == QuestionTypeEnum.DropDown)
                {
                    foreach (var optionModel in model.OptionList)
                    {
                        if (optionModel.Id.HasValue)
                        {
                            var option = removeList.FirstOrDefault(item => item.Id == optionModel.Id);
                            if (option != null)
                            {
                                removeList.Remove(option);
                                option.Name = optionModel.Name;
                                option.Order = order;
                                option.SysEditor = user.Id;
                                option.SysUpdated = DateTime.Now;
                                order++;
                            }
                        }
                        else
                        {
                            var option = new Option()
                            {
                                Name = optionModel.Name,
                                Order = order,
                                SysCreated = DateTime.Now,
                                SysCreator = user.Id,
                            };
                            updateItem.Options.Add(option);
                            order++;
                        }
                    }
                }
                foreach (var option in removeList)
                {
                    option.IsDeleted = true;
                    option.SysEditor = user.Id;
                    option.SysUpdated = DateTime.Now;
                }

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

        public static bool Delete(HuntingEntities dataContext, Question updateItem, AclUser user)
        {
            try
            {
                updateItem.IsDeleted = true;
                updateItem.SysEditor = user.Id;
                updateItem.SysUpdated = DateTime.Now;
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Delete");
                return false;
            }
        }

        public static bool Move(HuntingEntities dataContext, Questionnaire questionnaire, int questionId, bool isMoveUp, AclUser user)
        {
            try
            {
                Question selectedQuestion = null;
                Question otherQuestion = null;
                var questionList = questionnaire.Questions.Where(item => item.IsDeleted == false).OrderBy(item => item.Order).ToList();
                for (int index = 0; index < questionList.Count; index++)
                {
                    questionList[index].Order = index + 1;
                    if (questionList[index].Id == questionId)
                    {
                        selectedQuestion = questionList[index];
                        if (isMoveUp == true && index > 0)
                        {
                            otherQuestion = questionList[index - 1];
                        }
                        if (isMoveUp == false && index < questionList.Count - 1)
                        {
                            otherQuestion = questionList[index + 1];
                        }
                    }
                }
                if (selectedQuestion != null && otherQuestion != null)
                {
                    int order = selectedQuestion.Order;
                    selectedQuestion.Order = otherQuestion.Order;
                    otherQuestion.Order = order;
                }

                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Move");
                return false;
            }
        }

        public static void OptionTableAction(HuntingEntities dataContext, QuestionUpdateModel model, string formAction, int? index)
        {
            if (formAction == "add")
            {
                model.OptionList.Add(new OptionUpdateModel());
            }
            if (formAction == "move-up" && index.HasValue && index.Value > 0 && index < model.OptionList.Count)
            {
                var option = model.OptionList[index.Value];
                model.OptionList[index.Value] = model.OptionList[index.Value - 1];
                model.OptionList[index.Value - 1] = option;
            }
            if (formAction == "move-down" && index.HasValue && index.Value >= 0 && index < model.OptionList.Count - 1)
            {
                var option = model.OptionList[index.Value];
                model.OptionList[index.Value] = model.OptionList[index.Value + 1];
                model.OptionList[index.Value + 1] = option;
            }
            if (formAction == "delete" && index.HasValue && index.Value >= 0 && index < model.OptionList.Count)
            {
                model.OptionList.RemoveAt(index.Value);
            }
            if (model.OptionList.Count > 0)
            {
                model.OptionList.First().IsFirst = true;
                model.OptionList.Last().IsLast = true;
            }
        }
    }
}
