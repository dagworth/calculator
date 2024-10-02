using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Text.Json;
using final_calculator.Scripts;

namespace final_calculator
{
    public partial class Calculator : Window
    {
        private Window? settings_window;
        public UserData? data;
        private static string saveDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json");
        public Calculator()
        {
            if(File.Exists(saveDataPath)) {
                string jsonData = File.ReadAllText(saveDataPath);
                data = JsonSerializer.Deserialize<UserData>(jsonData);

            } else {
                data = new UserData();
            }
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            if(settings_window != null && settings_window.IsLoaded) {
                settings_window.Close();
            }
            string jsonData = JsonSerializer.Serialize(data);
            File.WriteAllText(saveDataPath, jsonData);
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e) {

            if(settings_window == null || !settings_window.IsLoaded) {
                Settings settings = new Settings();
                settings.main_window = this;
                if (data!.debug == true) {
                    settings.debug.IsChecked = true;
                } else {
                    settings.debug.IsChecked = false;
                }
                if (data!.prefix == true!) {
                    settings.prefix.IsChecked = true;
                } else {
                    settings.prefix.IsChecked = false;
                }
                settings.theme.Text = data.theme;
                settings.Show();
                settings_window = settings;
                settings_window.Topmost = true;
            } else {
                settings_window.Close();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            string response = EquationHandler.solve(text, data ?? new UserData());
            Response.Text = response;
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(settings_window == null) {
                this.Topmost = true;
            } else {
                settings_window.Topmost = true;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }
    }
}
