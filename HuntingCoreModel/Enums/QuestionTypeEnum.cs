using HuntingCoreModel.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace HuntingCoreModel.Enums
{
    public enum QuestionTypeEnum
    {
        Checkbox,
        CheckboxList,
        DropDown,
        Number,
        TextArea,
        TextBox,
    }

    public class QuestionTypeEnumConvertor
    {
        private static object lockObject = new object();
        private static Dictionary<int, List<Tuple<string, string>>> selectDictionary = new Dictionary<int, List<Tuple<string, string>>>();

        public static string GetString(QuestionTypeEnum state)
        {
            switch (state)
            {
                case QuestionTypeEnum.Checkbox: return QuestionnaireRes.QUESTION_TYPE_CHECKBOX;
                case QuestionTypeEnum.CheckboxList: return QuestionnaireRes.QUESTION_TYPE_CHECKBOX_LIST;
                case QuestionTypeEnum.DropDown: return QuestionnaireRes.QUESTION_TYPE_DROPDOWN;
                case QuestionTypeEnum.Number: return QuestionnaireRes.QUESTION_TYPE_NUMBER;
                case QuestionTypeEnum.TextArea: return QuestionnaireRes.QUESTION_TYPE_TEXTAREA;
                case QuestionTypeEnum.TextBox: return QuestionnaireRes.QUESTION_TYPE_TEXTBOX;
            }
            return null;
        }

        private static List<Tuple<string, string>> GetSelectListInner()
        {
            var list = new List<Tuple<string, string>>();
            list.Add(Tuple.Create(QuestionTypeEnum.Checkbox.ToString(), QuestionnaireRes.QUESTION_TYPE_CHECKBOX));
            list.Add(Tuple.Create(QuestionTypeEnum.CheckboxList.ToString(), QuestionnaireRes.QUESTION_TYPE_CHECKBOX_LIST));
            list.Add(Tuple.Create(QuestionTypeEnum.DropDown.ToString(), QuestionnaireRes.QUESTION_TYPE_DROPDOWN));
            list.Add(Tuple.Create(QuestionTypeEnum.Number.ToString(), QuestionnaireRes.QUESTION_TYPE_NUMBER));
            list.Add(Tuple.Create(QuestionTypeEnum.TextArea.ToString(), QuestionnaireRes.QUESTION_TYPE_TEXTAREA));
            list.Add(Tuple.Create(QuestionTypeEnum.TextBox.ToString(), QuestionnaireRes.QUESTION_TYPE_TEXTBOX));
            return list;
        }

        public static SelectList GetSelectList(QuestionTypeEnum? selected, int languageId)
        {
            lock (lockObject)
            {
                if (selectDictionary.ContainsKey(languageId) == false)
                {
                    selectDictionary.Add(languageId, GetSelectListInner());
                }
                var selectList = new SelectList(selectDictionary[languageId], "Item1", "Item2", selected);
                return selectList;
            }
        }
    }

}
