using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace HighScore.Controls {
    public class DataGridSuggestTextColumn : DataGridTextColumn {

        private BindingBase itemsSource;
        public BindingBase ItemsSource {
            get {
                return itemsSource;
            }
            set {
                if (itemsSource == value)
                    return;

                itemsSource = value;
            }
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs) {
            ComboBox comboBox = editingElement as ComboBox;
            if (comboBox == null)
                return (object)null;

            var t = comboBox.FindChild("PART_EditableTextBox", typeof(TextBox)) as TextBox;
            string text = comboBox.Text;
            if (t != null)
                t.Focus();
            TextCompositionEventArgs compositionEventArgs = editingEventArgs as TextCompositionEventArgs;
            if (compositionEventArgs != null) {
                string str = this.ConvertTextForEdit(compositionEventArgs.Text);
                comboBox.Text = str;

                if (t != null)
                    t.Select(str.Length, 0);

            } else if (!(editingEventArgs is MouseButtonEventArgs) || (t != null && !PlaceCaretOnTextBox(t, Mouse.GetPosition((IInputElement)t)))) {
                t.SelectAll();
            }
            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        private static bool PlaceCaretOnTextBox(TextBox textBox, Point position) {
            int characterIndexFromPoint = textBox.GetCharacterIndexFromPoint(position, false);
            if (characterIndexFromPoint < 0)
                return false;
            textBox.Select(characterIndexFromPoint, 0);
            return true;
        }

        internal void ApplyBinding(DependencyObject target, DependencyProperty property) {
            BindingBase binding = this.Binding;
            if (binding != null)
                BindingOperations.SetBinding(target, property, binding);
            else
                BindingOperations.ClearBinding(target, property);
        }

        private string ConvertTextForEdit(string s) {
            if (s == "\b")
                s = string.Empty;
            return s;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem) {
            ComboBox comboBox = new ComboBox();
            comboBox.IsEditable = true;
            comboBox.IsDropDownOpen = false;
            ApplyBinding(comboBox, ComboBox.TextProperty);
            if(itemsSource != null)
                BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, itemsSource);
            return comboBox;
        }
    }
}
