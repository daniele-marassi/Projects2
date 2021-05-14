using System.Windows;
using System.Windows.Controls;
using MVVMDemo.ViewModels;
using MVVMDemo.Views;
using static MVVMDemo.ViewModels.ToolBarHandler;
using System;

namespace MVVMDemo.TemplateSelectors
{
    public class ToolBarTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            TypeItem typeItem = (TypeItem) Enum.Parse(typeof(TypeItem), item.GetType().GetProperty("Type").GetValue(item, null).ToString());

            if (typeItem == TypeItem.Button)
            {
                return ButtonTemplate;
            }
            else if (typeItem == TypeItem.Textbox)
            {
                return TextBoxTemplate;
            }
            else if (typeItem == TypeItem.Separator)
            {
                return SeparatorTemplate;
            }

            return null;
        }

        public DataTemplate ButtonTemplate
        {
            get;
            set;
        }

        public DataTemplate TextBoxTemplate
        {
            get;
            set;
        }

        public DataTemplate SeparatorTemplate
        {
            get;
            set;
        }
    }
}
