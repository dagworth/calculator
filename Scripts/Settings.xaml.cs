using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace final_calculator {

    public partial class Settings : Window {
        public Calculator? main_window;
        public Settings() {
            InitializeComponent();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }
        private void Close_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void DebugChanged(object sender, RoutedEventArgs e) {
            CheckBox checkBox = (CheckBox)sender;
            main_window!.data!.debug = checkBox.IsChecked;
        }

        private void PrefixChanged(object sender, RoutedEventArgs e) {
            CheckBox checkBox = (CheckBox)sender;
            main_window!.data!.prefix = checkBox.IsChecked;
        }

        private void theme_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
