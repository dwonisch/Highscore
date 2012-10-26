using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HighScore
{
    public class Calendar : ListBox
    {
        static Calendar() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
        }

        public static readonly DependencyProperty itemDoubleClickProperty = DependencyProperty.Register("ItemDoubleClick", typeof(ICommand), typeof(Calendar));
        public ICommand ItemDoubleClick { get; set; }
    }
}
