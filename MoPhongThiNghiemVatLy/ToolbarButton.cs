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
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.ExceptionServices;
using MoPhongThiNghiemVatLy.AddWindow;

namespace MoPhongThiNghiemVatLy
{
    internal class ToolbarButton
    {
        public static void SourceBtn_Click(Canvas CircuitCanvas)
        {
            if (MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Đã có nguồn r cu!");
                return;
            }
            else
            {
                var inputBox = new TextBox
                {
                    Width = 60,
                    Height = 25,
                    Margin = new Thickness(10),
                    Text = "24" // Giá trị mặc định là 24
                };
                var dialog = new Window
                {
                    Title = "Nhập hiệu điện thế toàn mạch: ",
                    Content = inputBox,
                    Width = 200,
                    Height = 120,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                dialog.Content = inputBox;
                dialog.Loaded += (sender, e) => inputBox.Focus(); //focus tự động

                inputBox.KeyDown += (s, keyEventArgs) =>
                {
                    if (keyEventArgs.Key == Key.Enter)
                    {
                        if (double.TryParse(inputBox.Text, out double value) && value > 0)
                        {
                            Circuit.MainCurcuitVoltage = value;
                            Drawing.AddSourceToCanvas(CircuitCanvas, 0, 0);
                            MainWindow.Instance.isSourceAdded = true;
                            MainCircuit.indexOfNode += 1;
                            MainCircuit.mainCircuit.Add(new Node(MainCircuit.indexOfNode));
                            dialog.Close();
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng nhập số lớn hơn 0!");
                        }
                    }
                };
                dialog.ShowDialog();
            }
        }
        public static void MNTBtn_Click(Canvas CircuitCanvas)
        {
            if (!MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Nguồn đâu đm óc chó");
                return;
            }
            if (MainWindow.Instance.isVoltmeterMode)
            {
                MessageBox.Show("Tắt chế độ vôn kế giùm cái r làm gì thì làm!");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }    
            BackUpComponent(); // backup trước khi làm gì nhé

            MainWindow.Instance.seriesResistorCount++; // Tăng đt nối tiếp
            MainWindow.Instance.resistorCount++;
            MainWindow.Instance.index++;
            MainWindow.Instance._seriesResistorCount[MainWindow.Instance.index] = 1;
            MainWindow.Instance.resistorValues[MainWindow.Instance.resistorCount] = 0; 
            
            Drawing.UpdateCircuitAfterAdd(CircuitCanvas, 1);
            MainCircuit.AddSeries(MainCircuit.mainCircuit, Drawing.ShowInputBoxForResistor(MainWindow.Instance.resistorCount));        
            if (MainWindow.Instance.resistorCount == 1 && MainWindow.Instance.seriesResistorCount == 1)
            {
                MainWindow.Instance.checkDotForVolt = true;
            }
            else
            {
                MainWindow.Instance.checkDotForVolt = false;
            }
        }
        public static void MSSBtn_Click(Canvas CircuitCanvas)
        {
            if (!MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Nguồn đâu mà add song song z cu!");
                return;
            }
            if (MainWindow.Instance.isVoltmeterMode)
            {
                MessageBox.Show("Tắt chế độ vôn kế giùm cái r làm gì thì làm!");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }
            BackUpComponent(); // backup trước khi làm gì nhé

            var dialog = new ThemSLDienTro();
            bool? result = dialog.ShowDialog();

            if (result == true) 
            {
                int count = dialog.ParallelResistorCount;
                MainWindow.Instance.parallelResistorCount = count;
                MainWindow.Instance.resistorCount += count;
                MainWindow.Instance.index++;
                MainWindow.Instance._seriesResistorCount[MainWindow.Instance.index] = count;

                // Cập nhật mạch và hiển thị nhập giá trị từng điện trở
                Drawing.UpdateCircuitAfterAdd(CircuitCanvas, count);
                Drawing.ShowInputBoxForParallelResistors(count);
            }
        }
        public static void Đèn_Click(Canvas CircuitCanvas)
        {
            if (!MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Nguồn đâu mà add đèn z cu!");
                return;
            }
            if (MainWindow.Instance.isVoltmeterMode)
            {
                MessageBox.Show("Tắt chế độ vôn kế giùm cái r làm gì thì làm!");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }
            BackUpComponent(); // backup trước khi làm gì nhé
            MainWindow.Instance.index++;
            MainWindow.Instance._seriesResistorCount[MainWindow.Instance.index] = -1;
            Drawing.UpdateCircuitAfterAdd(CircuitCanvas, 0);
            MainCircuit.AddLight(MainCircuit.mainCircuit, Drawing.ShowInputBoxForLight());
        }
        public static void Ampe_Click(Canvas CircuitCanvas)
        {
            if (!MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Nguồn đâu mà add ampe z cu!");
                return;
            }
            if (MainWindow.Instance.isVoltmeterMode)
            {
                MessageBox.Show("Tắt chế độ vôn kế giùm cái r làm gì thì làm!");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }
            BackUpComponent(); // backup trước khi làm gì nhé
            MainWindow.Instance.index++;
            MainWindow.Instance._seriesResistorCount[MainWindow.Instance.index] = -2;
            Drawing.UpdateCircuitAfterAdd(CircuitCanvas, 0);
            MainCircuit.AddAmmeter(MainCircuit.mainCircuit);
        }
        public static void Khóa_Click(Canvas CircuitCanvas)
        {
            if (!MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Nguồn đâu mà add khóa z cu!");
                return;
            }
            if (MainWindow.Instance.isVoltmeterMode)
            {
                MessageBox.Show("Tắt chế độ vôn kế giùm cái r làm gì thì làm!");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }
            BackUpComponent(); // backup trước khi làm gì nhé
            MainWindow.Instance.index++;
            MainWindow.Instance._seriesResistorCount[MainWindow.Instance.index] = -3;
            MainWindow.Instance.isLightOn[MainWindow.Instance.numKey] = true;
            MainWindow.Instance.numKey++;
            Drawing.UpdateCircuitAfterAdd(CircuitCanvas, 0);
            if (MainWindow.Instance.numKey >= 1) MainWindow.Instance.isHaveKey = true;
            else MainWindow.Instance.isHaveKey = false;
            MainCircuit.AddSwitch(MainCircuit.mainCircuit);
        }
        public static void VoltmeterButton_Click(Canvas CircuitCanvas, object sender)
        {
            if (!MainWindow.Instance.isSourceAdded && MainWindow.Instance._seriesResistorCount.Count < 1)
            {
                MessageBox.Show("Đã vẽ mạch chó đâu mà add vôn kế!");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }
            MainWindow.Instance.isVoltmeterMode = !MainWindow.Instance.isVoltmeterMode;
            Button voltmeterButton = sender as Button;         
            var buttonBackground = voltmeterButton.Template.FindName("ButtonBackground", voltmeterButton) as Border;
            if (buttonBackground != null)
            {
                if (MainWindow.Instance.isVoltmeterMode)
                {
                    // Đổi nền sang xanh để báo hiệu chế độ vôn kế
                    buttonBackground.Background = new SolidColorBrush(Colors.LightGreen);
                    MessageBox.Show("Voltmeter mode: on. Please click 2 available dots in the model.");
                    Drawing.VisibleDots();
                    // Chỉ vẽ dots nếu chưa được vẽ
                    if (!MainWindow.Instance.areDotsDrawn)
                    {
                        Drawing.DrawDotsAndVoltmeter(); // Vẽ dấu chấm cho các điện trở
                        MainWindow.Instance.areDotsDrawn = true; // Đánh dấu dots đã được vẽ
                    }
                }
                else
                {
                    // Trở lại màu gốc (đen xám)
                    buttonBackground.Background = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                    MessageBox.Show("Voltmeter mode: off");
                    MainWindow.Instance.areDotsDrawn = false; // Reset cờ nếu chế độ tắt
                    Drawing.InvisibleDots();
                }
            }
        }

        public static Stack<StorageInformation> history = new Stack<StorageInformation>();
        public static Stack<StorageInformation> redoStack = new Stack<StorageInformation>();

        public static void BackUpComponent() //lưu lại data
        {
            history.Push(new StorageInformation(MainWindow.Instance._seriesResistorCount, MainWindow.Instance.connectedDots, MainWindow.Instance.m, MainWindow.Instance.n));
        }
        public static void UndoClick()
        {
            if (history.Count == 0)
            {
                MessageBox.Show("Bạn không thể thực hiện thao tác \"UNDO\" vì không còn thao tác cũ hơn !");
                return;
            }
            redoStack.Push(new StorageInformation(MainWindow.Instance._seriesResistorCount, MainWindow.Instance.connectedDots, MainWindow.Instance.m, MainWindow.Instance.n));
            history.Pop().ReturnValueOfCircuit();
            Drawing.UpdateCircuit(MainWindow.Instance.CircuitCanvas, 0);
        }
        public static void RedoClick()
        {
            if (redoStack.Count == 0)
            {
                MessageBox.Show("Bạn không thể thực hiện thao tác \"REDO\" vì không còn thao tác cũ hơn !");
                return;
            }
            BackUpComponent();
            redoStack.Pop().ReturnValueOfCircuit();
            Drawing.UpdateCircuit(MainWindow.Instance.CircuitCanvas, 0);
        }

        public static void Check_Click(Canvas CircuitCanvas, object sender)
        {
            if (!MainWindow.Instance.isSourceAdded && MainWindow.Instance._seriesResistorCount.Count < 1)
            {
                MessageBox.Show("Đã vẽ mạch chó đâu mà add vôn kế!");
                return;
            }
            foreach (var pair in MainWindow.Instance.isLightOn)
            {
                if (!pair.Value)
                {
                    MessageBox.Show("Mạch chưa đóng kín. Đóng khóa để đèn sáng hoặc ko thì sống trong bóng tối đi");
                    return;
                }
            }
            // Đổi trạng thái sáng/tối
            MainWindow.Instance.isLightMode = !MainWindow.Instance.isLightMode;
            Button lightButton = sender as Button;
            var buttonBackground = lightButton.Template.FindName("ButtonBackground", lightButton) as Border;
            if (buttonBackground != null)
            {
                buttonBackground.Background = MainWindow.Instance.isLightMode
                    ? new SolidColorBrush(Colors.LightGreen)
                    : new SolidColorBrush(Color.FromRgb(51, 51, 51));
            }

            // Điều khiển electron
            if (MainWindow.Instance.isLightMode) // Nếu bật chế độ sáng
            {
                Drawing.DrawRealElectron(CircuitCanvas, 0.5); // Gọi hàm bắt đầu tạo và animate electron
            }
            else // Nếu tắt chế độ sáng (chế độ tối)
            {
                Drawing.StopDrawingElectrons(); // Dừng timer (dừng tạo electron)
            }
        }

        public static void CalculateButton_Click(Canvas CircuitCanvas, object sender)
        {
            string res = "";
            Circuit.MainCircuitEROC = Circuit.EquivalentResistance(MainCircuit.mainCircuit);
            Circuit.MainCircuitAmperage = Circuit.MainCurcuitVoltage / Circuit.MainCircuitEROC;
            Circuit.CalculateForAllDetails(MainCircuit.mainCircuit, Circuit.MainCircuitAmperage);
            for (int i = 0; MainCircuit.mainCircuit.Count > i; i++)
            {
                res += MainCircuit.mainCircuit[i].ShowResult();
            }
            res += $"\n {MainCircuit.history.Count} ";
            res += $"\n {MainCircuit.redoStack.Count} ";
            MessageBox.Show(res);
        }
        public static void Xóa_Click(Canvas CircuitCanvas)
        {
            if (!MainWindow.Instance.isSourceAdded)
            {
                MessageBox.Show("Cậu ơi! Màn hình chưa có cái gì để xóa");
                return;
            }
            if (MainWindow.Instance.isLightMode)
            {
                MessageBox.Show("Điện thì cứ chạy mà hay muốn thêm quá! Giật điện đó ạ, tắt kiểm tra đèn đi");
                return;
            }
            if (MainWindow.Instance.isVoltmeterMode)
            {
                MessageBox.Show("Tắt chế độ vôn kế giùm cái r làm gì thì làm!");
                return;
            }
            BackUpComponent(); // backup trước khi xóa nhé
            var result = Drawing.ShowInputForXoa();
            if (result != null)
            {
                int targetKey = 0;
                int type = result[0];
                int index = result[1];
                int count = 0;
                if (type == -4)
                {
                    var tempList = MainWindow.Instance.connectedDots.ToList();
                    if (index > 0 && index <= tempList.Count)
                    {
                        tempList.RemoveAt(index - 1);
                    }

                    MainWindow.Instance.connectedDots = new HashSet<(int, int)>(tempList);
                    Drawing.UpdateCircuitAfterDelete(MainWindow.Instance.CircuitCanvas, 0);
                }    
                foreach (var kvp in MainWindow.Instance._seriesResistorCount)
                {
                    if (type == 1 && kvp.Value >= 1) count += kvp.Value;
                    else if (type == kvp.Value) count++;

                    if (count >= index)
                    {
                        targetKey = kvp.Key;
                        break;
                    }
                }
                if (targetKey == 0) return;
                if (type >= 1)
                {
                    for (int i = index; i < MainWindow.Instance.resistorValues.Count(); i++)
                        MainWindow.Instance.resistorValues[i] = MainWindow.Instance.resistorValues[i + 1];
                    MainWindow.Instance.resistorValues.Remove(MainWindow.Instance.resistorValues.Count());
                    MainWindow.Instance.resistorCount--;
                }
                if (type == 1) MainWindow.Instance.seriesResistorCount--;
                if (MainWindow.Instance._seriesResistorCount[targetKey] > 1)
                {
                    MainWindow.Instance._seriesResistorCount[targetKey]--;
                    Drawing.UpdateCircuitAfterDelete(MainWindow.Instance.CircuitCanvas, 0);
                }
                else
                {
                    XoaPhanTu(targetKey);
                    Drawing.UpdateCircuitAfterDelete(MainWindow.Instance.CircuitCanvas, 1);
                }
                Drawing.UpdateResistorValuesDisplay();
                MainCircuit.RemoveDetail(MainCircuit.mainCircuit, result[0], result[1]);
            }
        }
        private static void XoaPhanTu(int key)
        {
            var tempList = MainWindow.Instance.connectedDots.ToList(); // Chuyển HashSet thành List
            for (int i = 0; i < tempList.Count; i++)
            {
                var item = tempList[i]; // Lấy giá trị tuple (int, int)

                // Kiểm tra và giảm giá trị nếu cần
                if (item.Item1 >= key) item.Item1--;
                if (item.Item2 >= key) item.Item2--;

                tempList[i] = item;
            }
            // Cập nhật lại HashSet với giá trị đã thay đổi
            MainWindow.Instance.connectedDots = new HashSet<(int, int)>(tempList);
            MainWindow.Instance.connectedDots = MainWindow.Instance.connectedDots
            .Where(pair => pair.Item1 != pair.Item2 && pair.Item1 >= 0 && pair.Item2 >= 0) // Lọc các phần tử thỏa mãn điều kiện
            .ToHashSet(); // Cập nhật lại HashSet
            for (int i = key; i < MainWindow.Instance._seriesResistorCount.Count(); i++)
                MainWindow.Instance._seriesResistorCount[i] = MainWindow.Instance._seriesResistorCount[i + 1];
            MainWindow.Instance._seriesResistorCount.Remove(MainWindow.Instance._seriesResistorCount.Count());
            MainWindow.Instance.index--;

        }     
    }
}
