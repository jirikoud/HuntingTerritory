using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.SqlGenerator;
using HuntingModel.SqlGenerator.Generators;
using HuntingModel.ViewModel.CheckInModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.Context
{
    public class CheckInContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static void ProcessQuestionnaireAnswers(HuntingEntities dataContext, CheckIn updateItem, CheckInUpdateModel model)
        {
            var questionDict = dataContext.Questions.
                Where(item => item.QuestionnaireId == model.QuestionnaireId && item.IsDeleted == false).
                ToDictionary(item => item.Id, item => item);
            foreach (var questionModel in model.QuestionList)
            {
                var question = questionDict[questionModel.Id];
                if (question != null)
                {
                    if (question.QuestionTypeEx == QuestionTypeEnum.Checkbox)
                    {
                        if (questionModel.BoolValue == true)
                        {
                            var answer = new Answer()
                            {
                                BoolValue = questionModel.BoolValue,
                                QuestionId = question.Id,
                            };
                            updateItem.Answers.Add(answer);
                        }
                    }
                    if (question.QuestionTypeEx == QuestionTypeEnum.CheckboxList)
                    {
                        if (questionModel.CheckBoxList != null)
                        {
                            foreach (var checkBoxModel in questionModel.CheckBoxList)
                            {
                                if (checkBoxModel.BoolValue == true)
                                {
                                    var answer = new Answer()
                                    {
                                        OptionId = checkBoxModel.Id,
                                        QuestionId = question.Id,
                                    };
                                    updateItem.Answers.Add(answer);
                                }
                            }
                        }
                    }
                    if (question.QuestionTypeEx == QuestionTypeEnum.DropDown)
                    {
                        if (questionModel.OptionId.HasValue)
                        {
                            var answer = new Answer()
                            {
                                OptionId = questionModel.OptionId,
                                QuestionId = question.Id,
                            };
                            updateItem.Answers.Add(answer);
                        }
                    }
                    if (question.QuestionTypeEx == QuestionTypeEnum.Number)
                    {
                        if (string.IsNullOrWhiteSpace(questionModel.StringValue) == false)
                        {
                            var answer = new Answer()
                            {
                                FloatValue = ContextUtils.ParseFloatString(questionModel.StringValue),
                                QuestionId = question.Id,
                            };
                            updateItem.Answers.Add(answer);
                        }
                    }
                    if (question.QuestionTypeEx == QuestionTypeEnum.TextArea)
                    {
                        if (string.IsNullOrWhiteSpace(questionModel.StringValue) == false)
                        {
                            var answer = new Answer()
                            {
                                StringValue = questionModel.StringValue,
                                QuestionId = question.Id,
                            };
                            updateItem.Answers.Add(answer);
                        }
                    }
                    if (question.QuestionTypeEx == QuestionTypeEnum.TextBox)
                    {
                        if (string.IsNullOrWhiteSpace(questionModel.StringValue) == false)
                        {
                            var answer = new Answer()
                            {
                                StringValue = questionModel.StringValue,
                                QuestionId = question.Id,
                            };
                            updateItem.Answers.Add(answer);
                        }
                    }
                }
            }
        }

        public static int GetTotalCount(HuntingEntities dataContext, CheckInFilter filter)
        {
            try
            {
                var generator = new CheckInListGenerator(filter);
                var totalCount = QueryGenerator.GetTotalCount(dataContext, generator);
                return totalCount;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetTotalCount");
                return 0;
            }
        }

        public static ItemListHolder<CheckInListItem> GetList(HuntingEntities dataContext, CheckInFilter filter, int pageIndex)
        {
            try
            {
                var generator = new CheckInListGenerator(filter);
                var list = QueryGenerator.ReadPagedList<CheckInListItem>(dataContext, generator, pageIndex, Constants.DEFAULT_LIST_PAGE_SIZE);
                var itemHolder = new ItemListHolder<CheckInListItem>(list);
                itemHolder.TotalCount = GetTotalCount(dataContext, filter);
                return itemHolder;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetList");
                return new ItemListHolder<CheckInListItem>();
            }
        }

        public static CheckIn GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var questionnaire = dataContext.CheckIns.FirstOrDefault(item => item.Id == id && item.IsDeleted == false);
                return questionnaire;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
                return null;
            }
        }

        public static int? Update(HuntingEntities dataContext, CheckIn updateItem, CheckInUpdateModel model, AclUser user, Language language)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new CheckIn()
                    {
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                        MapItemId = model.MapItemId,
                    };
                    dataContext.CheckIns.Add(updateItem);
                }
                if (model.CheckDateTime == null)
                {
                    model.CheckDateTime = ContextUtils.ParseDateTimeString(model.CheckTime, language).Value;
                }

                updateItem.CheckTime = model.CheckDateTime.Value;
                updateItem.QuestionnaireId = model.QuestionnaireId;
                updateItem.Note = model.Note;

                updateItem.SysEditor = user.Id;
                updateItem.SysUpdated = DateTime.Now;

                dataContext.Answers.RemoveRange(updateItem.Answers);
                if (model.QuestionnaireId.HasValue)
                {
                    ProcessQuestionnaireAnswers(dataContext, updateItem, model);
                }

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Update");
                return null;
            }
        }

        public static bool Delete(HuntingEntities dataContext, CheckIn updateItem, AclUser user)
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

        public static void Validate(HuntingEntities dataContext, CheckInUpdateModel model, ModelStateDictionary modelState)
        {
            if (model.QuestionnaireId.HasValue && model.QuestionList != null)
            {
                var questionDict = dataContext.Questions.Where(item => item.QuestionnaireId == model.QuestionnaireId.Value).ToDictionary(item => item.Id, item => item);
                for (int index = 0; index < model.QuestionList.Count; index++)
                {
                    var questionModel = model.QuestionList[index];
                    if (questionDict.ContainsKey(questionModel.Id))
                    {
                        var question = questionDict[questionModel.Id];
                        if (question.IsRequired)
                        {
                            switch (question.QuestionTypeEx)
                            {
                                case QuestionTypeEnum.DropDown:
                                    if (questionModel.OptionId == null)
                                    {
                                        modelState.AddModelError(string.Format("QuestionList[{0}].OptionId", index), ValidationRes.VALIDATION_REQUIRED);
                                    }
                                    break;
                                case QuestionTypeEnum.Number:
                                case QuestionTypeEnum.TextArea:
                                case QuestionTypeEnum.TextBox:
                                    if (string.IsNullOrWhiteSpace(questionModel.StringValue))
                                    {
                                        modelState.AddModelError(string.Format("QuestionList[{0}].StringValue", index), ValidationRes.VALIDATION_REQUIRED);
                                    }
                                    break;
                            }
                        }
                        if (question.QuestionTypeEx == QuestionTypeEnum.Number)
                        {
                            if (string.IsNullOrWhiteSpace(questionModel.StringValue) == false)
                            {
                                var doubleValue = ContextUtils.ParseFloatString(questionModel.StringValue);
                                if (doubleValue == null)
                                {
                                    modelState.AddModelError(string.Format("QuestionList[{0}].StringValue", index), ValidationRes.VALIDATION_DOUBLE);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
