using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HighScore.Controls
{
    public class Calendar : ListBox
    {
        static Calendar() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
        }

        public Calendar() {
            ItemDoubleClick = new RelayCommand(new Action(() => { }));
        }

        public static readonly DependencyProperty ItemDoubleClickProperty = DependencyProperty.Register("ItemDoubleClick", typeof(ICommand), typeof(Calendar));

        public ICommand ItemDoubleClick
        {
            get { return (ICommand)GetValue(ItemDoubleClickProperty); }
            set { SetValue(ItemDoubleClickProperty, value); }
        }

    }
}
