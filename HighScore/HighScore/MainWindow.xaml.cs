using HighScore.Data;
using HighScore.Logic;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HighScore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            contentControl.DataContext = this;

            var textBox = new TextBox();
            textBox.Text = "Default";
            ContentControl = textBox;

            var context = new HighScoreContext();
            context.DaySelected = new DelegateCommand<CalendarItem>((c) => {
                Label label = new Label();
                label.Content = c.Date.ToString();
                ContentControl = label;
            });


            DataContext = context;
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(MainWindow));

        public Control ContentControl { get; set; }
    }
}
