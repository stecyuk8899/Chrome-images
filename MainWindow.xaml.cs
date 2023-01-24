using System;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Diagnostics;

namespace Chrome_images
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ClearEvent(object sender, RoutedEventArgs e)
        {
            size.Text = "";
            categories.SelectedIndex = 0;
        }
        private async void DownloadEvent(object sender, RoutedEventArgs e)
        {
            if (CheckSize())
            {
                string url = "";
                if (categories.SelectedIndex != 0)
                    url = $"https://source.unsplash.com/random/{size.Text}/?{categories.SelectedItem.ToString()}&1";
                else
                    url = $"https://source.unsplash.com/random/{size.Text}/";
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "JPEG files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp";
                dialog.FilterIndex = 0;
                if ((bool)dialog.ShowDialog())
                {
                    HttpClient client = new HttpClient();
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);

                        using (var stream = File.Create(dialog.FileName))
                        {
                            await response.Content.CopyToAsync(stream);
                        }
                        MessageBox.Show("Завантаження відбулось успішно!", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
                        history.Items.Add($"{DateTime.Now} - {dialog.FileName} - Збережено успішно!");
                        Process.Start(dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка : {ex.Message}", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool CheckSize()
        {
            if (string.IsNullOrWhiteSpace(size.Text))
                return false;
            return true;
        }
    }
}
