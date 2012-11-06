using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HighScore.Controls {
    public class LoadingCircle : Control {
        static LoadingCircle() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingCircle), new FrameworkPropertyMetadata(typeof(LoadingCircle)));
        }

        public LoadingCircle() {
            BackgroundColor = Colors.Black;
            Size = 5;
        }

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(LoadingCircle));
        public static readonly DependencyProperty HighlightColorProperty = DependencyProperty.Register("HighlightColor", typeof(Color), typeof(LoadingCircle));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(LoadingCircle));

        public Color BackgroundColor {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public Color HighlightColor {
            get { return (Color)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        public double Size {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
    }
}
