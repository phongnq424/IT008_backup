﻿using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;

namespace MoPhongThiNghiemVatLy.AddWindow
{
    /// <summary>
    /// Interaction logic for ThemSLDienTro.xaml
    /// </summary>
    public partial class ThemSLDienTro : Window
    {
        public ThemSLDienTro()
        {
            InitializeComponent();
            ErrorSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
        }
        public int ParallelResistorCount { get; private set; }
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnSaveButtonClick(sender, e);
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào
            if (int.TryParse(QuantityTextBox.Text, out int count) && count > 0)
            {
                ParallelResistorCount = count;
                DialogResult = true; // Đóng cửa sổ và trả kết quả
            }
            else
            {
                ErrorSnackbar.MessageQueue.Enqueue("Vui lòng nhập số nguyên lớn hơn 0!");

                // Thay đổi màu viền TextBox để người dùng biết có lỗi
                QuantityTextBox.BorderBrush = Brushes.Red;
                QuantityTextBox.Background = Brushes.LightPink;
                QuantityTextBox.Focus();
            }
        }
        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            // Khôi phục lại màu sắc của TextBox khi người dùng bắt đầu nhập
            QuantityTextBox.BorderBrush = Brushes.Transparent;  // Hoặc bạn có thể dùng màu viền mặc định
            QuantityTextBox.Background = Brushes.White; // Hoặc màu nền mặc định
        }
    }
}
