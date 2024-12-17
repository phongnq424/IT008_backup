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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MoPhongThiNghiemVatLy
{
    internal class MenuStrip
    {

        private MainWindow mainWindow;
        public MenuStrip(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        //DEFINE
        private string currentFilePath = string.Empty;
        private string currentPicturesPath = string.Empty;

        public void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
                Title = "Save Image File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveCanvasAsImage(MainWindow.Instance.CircuitCanvas, filePath);
                currentPicturesPath = filePath; 
            }
        }

        public void SaveCanvasAsImage(Canvas canvas, string filePath)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)canvas.ActualWidth,
                (int)canvas.ActualHeight,
                96, 
                96,
                PixelFormats.Pbgra32);
            renderTargetBitmap.Render(canvas);
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                pngEncoder.Save(fileStream);
            }

            MessageBox.Show("Canvas saved as image!");
        }

        public void New_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.isSave == false)
            {
                MessageBoxResult result = MessageBox.Show(
                    "muốn lưu lại kết quả không nhóc?",
                    "Thông báo",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile_Click(sender, e);
                    CreateNew();
                }
                else if (result == MessageBoxResult.No)
                {
                    CreateNew();
                }
            }
            else
            {
                CreateNew();
            }

        }

        public void CreateNew()
        {
            MainWindow newWindow = new MainWindow();
            // Đóng cửa sổ hiện tại
            Application.Current.MainWindow.Close();

            // Đặt cửa sổ mới làm cửa sổ chính
            Application.Current.MainWindow = newWindow;

            newWindow.Show();

        }
        
        public void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.isSourceAdded == true)
            {
                // Hiển thị MessageBox để hỏi người dùng có muốn lưu không
                MessageBoxResult result = MessageBox.Show(
                    "Muốn lưu lại kết quả không nhóc?",
                    "Thông báo",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile_Click(sender, e);
                    CreateNew();
                }
                else if (result == MessageBoxResult.No)
                {
                    // Nếu người dùng chọn No, không lưu, tiếp tục tạo mới
                    CreateNew();
                }
            }
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Open JSON File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                LoadToFile(filePath);
                currentFilePath = filePath;
                // Đọc nội dung file JSON (bạn có thể sử dụng Json.NET hoặc các phương thức khác để parse JSON)
                MessageBox.Show($"File opened: {filePath}");
            }
        }
        public void LoadToFile(string filePath)
        {

            string jsonContent = File.ReadAllText(filePath);
            dynamic circuitData = JsonConvert.DeserializeObject(jsonContent);
            try
            {
                // Khôi phục biến toàn cục
                MainWindow.Instance.checkDotForVolt = (bool)circuitData.GlobalVariables.checkDotForVolt;
                MainWindow.Instance.isSourceAdded = (bool)circuitData.GlobalVariables.isSourceAdded;
                MainWindow.Instance.lineHeight = (double)circuitData.GlobalVariables.lineHeight;
                MainWindow.Instance.m = (double)circuitData.GlobalVariables.m;
                MainWindow.Instance.n = (double)circuitData.GlobalVariables.n;
                MainWindow.Instance.xA = (double)circuitData.GlobalVariables.xA;
                MainWindow.Instance.yA = (double)circuitData.GlobalVariables.yA;
                MainWindow.Instance.xB = (double)circuitData.GlobalVariables.xB;
                MainWindow.Instance.yB = (double)circuitData.GlobalVariables.yB;
                MainWindow.Instance.lightAcp = (bool)circuitData.GlobalVariables.lightAcp;

                // Khôi phục đoạn thẳng AB
                if (circuitData.LineAB != null)
                {
                    MainWindow.Instance.AB = new Line
                    {
                        X1 = (double)circuitData.LineAB.x1,
                        Y1 = (double)circuitData.LineAB.y1,
                        X2 = (double)circuitData.LineAB.x2,
                        Y2 = (double)circuitData.LineAB.y2
                    };
                }

                MainWindow.Instance.InorderToCal = ((JArray)circuitData.CircuitComponents.InorderToCal).ToObject<List<char>>();
               
                // Khôi phục resistorValues
                if (circuitData.CircuitComponents.resistorValues is JObject jResistorValues)
                {
                    MainWindow.Instance.resistorValues = jResistorValues
                        .Properties()
                        .ToDictionary(p => int.Parse(p.Name), p => (double)p.Value);
                }
                else if (circuitData.CircuitComponents.resistorValues is JArray jArrayResistorValues)
                {
                    MainWindow.Instance.resistorValues = jArrayResistorValues
                        .ToObject<Dictionary<int, double>>();
                }

                // Khôi phục seriesResistorCount
                if (circuitData.CircuitComponents._seriesResistorCount is JObject jSeriesResistorCount)
                {
                    MainWindow.Instance._seriesResistorCount = jSeriesResistorCount
                        .Properties()
                        .ToDictionary(p => int.Parse(p.Name), p => (int)p.Value);
                }
                else if (circuitData.CircuitComponents._seriesResistorCount is JArray jArraySeriesResistorCount)
                {
                    MainWindow.Instance._seriesResistorCount = jArraySeriesResistorCount
                        .ToObject<Dictionary<int, int>>();
                }
                if (circuitData.GlobalVariables?.isLightOn != null)
                {
                    if (circuitData.GlobalVariables.isLightOn is JObject jIsLightOn)
                    {
                        MainWindow.Instance.isLightOn = jIsLightOn
                            .Properties()
                            .ToDictionary(p => int.Parse(p.Name), p => (bool)p.Value);
                    }
                    else if (circuitData.GlobalVariables.isLightOn is JArray jArrayIsLightOn)
                    {
                        MainWindow.Instance.isLightOn = jArrayIsLightOn
                            .ToObject<Dictionary<int, bool>>();
                    }
                    else
                    {
                        MainWindow.Instance.isLightOn = new Dictionary<int, bool>();
                    }
                }
                else
                {
                    MainWindow.Instance.isLightOn = new Dictionary<int, bool>();
                }
                MainWindow.Instance.index = MainWindow.Instance._seriesResistorCount.Count();
                int MaxHeight = 1;
                foreach (var kvp in MainWindow.Instance._seriesResistorCount)
                    if (kvp.Value > MaxHeight)
                        MaxHeight = kvp.Value;
                MainWindow.Instance.hadVoltmeter = (bool)circuitData.GlobalVariables.hadVoltmeter;
                MainWindow.Instance.resistorCount = (int)circuitData.CircuitComponents.resistorCount;
                MainWindow.Instance.seriesResistorCount = (int)circuitData.CircuitComponents.seriesResistorCount;
                MainWindow.Instance.parallelResistorCount = (int)circuitData.CircuitComponents.parallelResistorCount;
                if (circuitData.GlobalVariables.connectedDots is JArray connectedDotsArray)
                {
                    MainWindow.Instance.connectedDots = new HashSet<(int, int)>(
                        connectedDotsArray.Select(pair => (
                            (int)pair["Item1"],  // Lấy giá trị Item1
                            (int)pair["Item2"]   // Lấy giá trị Item2
                        ))
                    );
                }
                else
                {
                    MainWindow.Instance.connectedDots = new HashSet<(int, int)>();
                }

                // Khôi phục UI Elements
                MainWindow.Instance.labels = ((JArray)circuitData.UIElements.labels)
                    .Select(label => new TextBlock { Text = (string)label })
                     .ToList();

                MainWindow.Instance.sourceElements = ((JArray)circuitData.UIElements.sourceElements)
                    .Select(name => CreateUIElementByName((string)name))
                    .ToList();

                MainWindow.Instance.LightBulbs = ((JArray)circuitData.UIElements.LightBulbs)
                    .Select(name => CreateUIElementByName((string)name))
                    .ToList();

                MainWindow.Instance.ampeList = ((JArray)circuitData.UIElements.ampeList)
                    .Select(name => CreateUIElementByName((string)name))
                    .ToList();

                // Load các biến từ MainCircuitData
                dynamic mainCircuitData = circuitData.MainCircuitData;

                // Khôi phục danh sách mainCircuit
                MainCircuit.mainCircuit = new List<CircuitDiagram>();
                foreach (var item in (IEnumerable<dynamic>)mainCircuitData.mainCircuit)
                {
                    MainCircuit.mainCircuit.Add(CreateCircuitDiagramFromJson(item));
                }

                // Khôi phục danh sách removedCircuit
                MainCircuit.removedCircuit = new List<CircuitDiagram>();
                foreach (var item in (IEnumerable<dynamic>)mainCircuitData.removedCircuit)
                {
                    MainCircuit.removedCircuit.Add(CreateCircuitDiagramFromJson(item));
                }
                // Khôi phục các chỉ số
                MainCircuit.indexOfNode = mainCircuitData.indexOfNode;
                MainCircuit.indexOfRes = mainCircuitData.indexOfRes;
                MainCircuit.indexOfVol = mainCircuitData.indexOfVol;
                MainCircuit.indexOfAmpe = mainCircuitData.indexOfAmpe;
                MainCircuit.indexOfSwitch = mainCircuitData.indexOfSwitch;
                MainCircuit.indexOfLight = mainCircuitData.indexOfLight;
                // Cập nhật giao diện canvas
                Drawing.UpdateCircuitAfterAdd(MainWindow.Instance.CircuitCanvas, MaxHeight);
                MainWindow.Instance.isSave = true;
                Drawing.UpdateResistorValuesDisplay();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in LoadToFile: {ex.Message}");
            }
        }
        private CircuitDiagram CreateCircuitDiagramFromJson(dynamic json)
        {
            int type = json.type ?? -1; // Kiểm tra null cho type

            switch (type)
            {
                case DEFINE.TYPE_Res:
                    return new Resistor(
                        (double)json.resistance,
                        (int)(json.indexer ?? 0) // Kiểm tra null và gán giá trị mặc định 0
                    );

                case DEFINE.TYPE_Ampe:
                    return new Ammeter((int)(json.indexer ?? 0)); // Kiểm tra null và gán giá trị mặc định 0

                case DEFINE.TYPE_Light:
                    return new Light(
                        (int)(json.indexer ?? 0),
                        (double)(json.resistance ?? 0), // Kiểm tra null cho resistance
                        (double)(json.eropc ?? 0) // Kiểm tra null cho eropc
                    );

                case DEFINE.TYPE_Node:
                    return new Node((int)(json.indexer ?? 0)); // Kiểm tra null cho indexer

                case DEFINE.TYPE_Vol:
                    return new Volmeter(
                        (int)(json.nodeLeft ?? 0), // Kiểm tra null cho nodeLeft
                        (int)(json.nodeRight ?? 0), // Kiểm tra null cho nodeRight
                        (int)(json.indexer ?? 0) // Kiểm tra null cho indexer
                    );

                default:
                    throw new InvalidOperationException($"Unknown circuit diagram type: {type}");
            }
        }


        private UIElement CreateUIElementByName(string name)
        {
            switch (name)
            {
                case "Rectangle": return new Rectangle();
                case "TextBlock": return new TextBlock();
                // Thêm các loại UIElement khác nếu cần
                default: return new UIElement();
            }
        }
        public void SaveToFile(string filePath)
        {
            if (MainWindow.Instance.isVoltmeterMode == true)
            {
                MessageBox.Show("Tắt vôn kế đi không lưu lỗi đó ạ!");
                return;
            }
            MainWindow.Instance.isSave = true;
            try
            {
                var circuitData = new
                {
                    GlobalVariables = new
                    {
                        checkDotForVolt = MainWindow.Instance.checkDotForVolt,
                        isSourceAdded = MainWindow.Instance.isSourceAdded,
                        lineHeight = MainWindow.Instance.lineHeight,
                        m = MainWindow.Instance.m,
                        n = MainWindow.Instance.n,
                        xA = MainWindow.Instance.xA,
                        yA = MainWindow.Instance.yA,
                        xB = MainWindow.Instance.xB,
                        yB = MainWindow.Instance.yB,
                        lightAcp = MainWindow.Instance.lightAcp,

                        // Lưu các biến mới
                        isVoltmeterMode = MainWindow.Instance.isVoltmeterMode,
                        areDotsDrawn = MainWindow.Instance.areDotsDrawn,
                        firstDot = MainWindow.Instance.firstDot,
                        secondDot = MainWindow.Instance.secondDot,
                        connectedDots = MainWindow.Instance.connectedDots.ToList(),
                        dotDegrees = MainWindow.Instance.dotDegrees.Select(kv => new { dot = kv.Key, degree = kv.Value }).ToList(),
                        indexDot = MainWindow.Instance.indexDot.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
                        voltNum = MainWindow.Instance.voltNum,
                        indexofParallel = MainWindow.Instance.indexofParallel.ToList(),
                        addedHeight = MainWindow.Instance.addedHeight,
                        hadVoltmeter = MainWindow.Instance.hadVoltmeter,
                        isLightOn = MainWindow.Instance.isLightOn,
                        numKey = MainWindow.Instance.numKey,
                        isKey = MainWindow.Instance.isKey
                    },
                    LineAB = MainWindow.Instance.AB != null ? new
                    {
                        x1 = MainWindow.Instance.AB.X1,
                        y1 = MainWindow.Instance.AB.Y1,
                        x2 = MainWindow.Instance.AB.X2,
                        y2 = MainWindow.Instance.AB.Y2
                    } : null,
                    Dots = new
                    {
                        dotIndex = MainWindow.Instance.dotIndex,
                        checkDots = MainWindow.Instance.checkDots
                    },
                    CircuitComponents = new
                    {
                        InorderToCal = MainWindow.Instance.InorderToCal,
                        resistorValues = MainWindow.Instance.resistorValues,
                        _seriesResistorCount = MainWindow.Instance._seriesResistorCount,
                        resistorCount = MainWindow.Instance.resistorCount,
                        seriesResistorCount = MainWindow.Instance.seriesResistorCount,
                        parallelResistorCount = MainWindow.Instance.parallelResistorCount,
                        seriesResistors = MainWindow.Instance.seriesResistors.Select((r, i) => new
                        {
                            index = i,
                            value = MainWindow.Instance.resistorValues.TryGetValue(i, out var resValue) ? resValue : 0,
                            label = MainWindow.Instance.seriesLabels.TryGetValue(i, out var label) ? label.Text : null
                        }),
                        parallelResistors = MainWindow.Instance.parallelResistors.Select((r, i) => new
                        {
                            index = i,
                            value = MainWindow.Instance.resistorValues.TryGetValue(i, out var parValue) ? parValue : 0,
                            label = MainWindow.Instance.parallelLabels.TryGetValue(i, out var label) ? label.Text : null
                        }),
                        connectingLines = MainWindow.Instance.connectingLines.Select(l => new
                        {
                            x1 = l.X1,
                            y1 = l.Y1,
                            x2 = l.X2,
                            y2 = l.Y2
                        })
                    },
                    UIElements = new
                    {
                        labels = MainWindow.Instance.labels.Select(l => l.Text).ToList(),
                        sourceElements = MainWindow.Instance.sourceElements.Select(e => e.GetType().Name).ToList(),
                        LightBulbs = MainWindow.Instance.LightBulbs.Select(e => e.GetType().Name).ToList(),
                        ampeList = MainWindow.Instance.ampeList.Select(e => e.GetType().Name).ToList()
                    },
                    MainCircuitData = new
                    {
                        mainCircuit = MainCircuit.mainCircuit.Select(c => new
                        {
                            indexer = c.Indexer,
                            resistance = c.Resistance,
                            voltage = c.Voltage,
                            amperage = c.Amperage,
                            type = c.Type,
                            eropc = c.Eropc
                        }).ToList(),
                        removedCircuit = MainCircuit.removedCircuit.Select(c => new
                        {
                            indexer = c.Indexer,
                            resistance = c.Resistance,
                            voltage = c.Voltage,
                            amperage = c.Amperage,
                            type = c.Type,
                            eropc = c.Eropc
                        }).ToList(),
                        indexOfNode = MainCircuit.indexOfNode,
                        indexOfRes = MainCircuit.indexOfRes,
                        indexOfVol = MainCircuit.indexOfVol,
                        indexOfAmpe = MainCircuit.indexOfAmpe,
                        indexOfSwitch = MainCircuit.indexOfSwitch,
                        indexOfLight = MainCircuit.indexOfLight
                    }
                };

                // Serialize the circuit data to JSON with formatting
                string jsonContent = JsonConvert.SerializeObject(circuitData, Formatting.Indented);

                // Save the JSON content to the specified file path
                File.WriteAllText(filePath, jsonContent);

                MessageBox.Show("Circuit successfully saved to file.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving circuit data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            // Nếu file đã có đường dẫn (đã được lưu trước đó), chỉ cần lưu lại
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                SaveToFile(currentFilePath);
            }
            else
            {
                SaveAsFile_Click(sender, e); // Gọi Save As để người dùng chọn đường dẫn lưu file
            }
        }

        public void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save JSON File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                // Ghi nội dung vào file đã chọn
                SaveToFile(filePath);
                currentFilePath = filePath; // Lưu đường dẫn của file đã lưu
            }
        }
        public void Undo_Click(object sender, RoutedEventArgs e)
        {

        }
        public void Redo_Click(object sender, RoutedEventArgs e)
        {

        }
        public void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please contact the boss Nguyen to know more about the application!");

        }
    }
}
