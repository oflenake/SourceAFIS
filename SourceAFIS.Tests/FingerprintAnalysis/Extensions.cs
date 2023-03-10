using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using NUnit.Framework;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.WindowsAPI;

namespace SourceAFIS.Tests.FingerprintAnalysis
{
    public static class Extensions
    {
        public static void SelectSlowly(this ComboBox combo, string text)
        {
            if (combo.GetSelectedItemText() != text)
            {
                combo.Click();
                Thread.Sleep(300);
                combo.Items.Where(item => item.Text == text).First().Click();
                Common.Wait(() => text == combo.GetSelectedItemText());
            }
        }

        public static string GetSelectedItemText(this ComboBox combo)
        {
            AutomationElement element = combo.AutomationElement;
            AutomationPattern automationPattern = element.GetSupportedPatterns().Where(
                p => p.ProgrammaticName == "SelectionPatternIdentifiers.Pattern").First();
            SelectionPattern selectionPattern = element.GetCurrentPattern(automationPattern) as SelectionPattern;
            return selectionPattern.Current.GetSelection()[0].Current.Name;
        }

        public static IUIItem GetChecked(this UIItemContainer container, SearchCriteria criteria)
        {
            IUIItem result = container.Get(criteria);
            Assert.IsNotNull(result);
            return result;
        }

        public static T GetChecked<T>(this UIItemContainer container, SearchCriteria criteria)
            where T : UIItem
        {
            T result = container.Get<T>(criteria);
            Assert.IsNotNull(result);
            return result;
        }

        public static T GetPatient<T>(this UIItemContainer container, SearchCriteria criteria)
            where T : UIItem
        {
            Common.Wait(() => container.Get<T>(criteria) != null);
            return container.GetChecked<T>(criteria);
        }

        public static Button GetPanelButton(this IUIItem panel, SearchCriteria criteria)
        {
            AutomationElement element = panel.GetElement(criteria);
            Assert.IsNotNull(element);
            return new Button(element, panel.ActionListener);
        }

        public static Label GetPanelLabel(this IUIItem panel, SearchCriteria criteria)
        {
            AutomationElement element = panel.GetElement(criteria);
            Assert.IsNotNull(element);
            return new Label(element, panel.ActionListener);
        }

        public static void PasteText(this Window window, string text)
        {
            Clipboard.SetText(text);
            window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            window.Keyboard.Enter("v");
            window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }
    }
}
