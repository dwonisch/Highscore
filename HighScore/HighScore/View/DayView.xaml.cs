using HighScore.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace HighScore.View {
    /// <summary>
    /// Interaction logic for DayView.xaml
    /// </summary>
    public partial class DayView : UserControl {
        public DayView() {
            InitializeComponent();
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }

        private bool tabPressed;

        private void data_RowEditEnding_1(object sender, DataGridRowEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                if (tabPressed && e.Row.Item == data.Items[data.Items.Count - 2]) {
                    var rowToSelect = data.Items[data.Items.Count - 1];
                    int rowIndex = data.Items.IndexOf(rowToSelect);
                    this.Dispatcher.BeginInvoke(new DispatcherOperationCallback((param) => {
                        tabPressed = false;
                        var cell = DataGridHelper.GetCell(data, rowIndex, 0);
                        cell.Focus();
                        data.BeginEdit();
                        return null;
                    }), DispatcherPriority.Background, new object[] { null });
                }
            }
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e) {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Shift) {
                var item = data.CurrentCell.Item;
                var index = data.Items.IndexOf(item);
                if(index >= data.Items.Count -2){
                    if(data.Columns.IndexOf(data.CurrentCell.Column) == data.Columns.Count -1)
                        tabPressed = true;
                }
            }
        }
    }
}
