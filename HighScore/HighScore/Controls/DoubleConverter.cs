using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace HighScore.Controls {
    public class DoubleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            double result;
            double val;
            if (value != null & parameter != null && double.TryParse(value.ToString(), out val) && double.TryParse(parameter.ToString(), out result)) {
                return val * result;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            double result;
            double val;
            if (value != null & parameter != null && double.TryParse(value.ToString(), out val) && double.TryParse(parameter.ToString(), out result)) {
                if (result == 0)
                    return 0;
                return val / result;
            }
            return 0.0;
        }
    }
}
