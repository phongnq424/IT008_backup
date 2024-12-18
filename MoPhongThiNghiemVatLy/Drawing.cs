using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net;

namespace MoPhongThiNghiemVatLy
{
    internal class Drawing
    {
        public static Dictionary<Point, List<Tuple<Point, Vector>>> Direction= new Dictionary<Point, List<Tuple<Point, Vector>>>();
        public static void AddSourceToCanvas(Canvas canvas, double m, double n)
        {
            // Lấy kích thước của canvas
            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;
            MainWindow.Instance.xA = centerX - 150 - m;
            MainWindow.Instance.xB = centerX + 150 + n;
            MainWindow.Instance.yA = centerY;
            MainWindow.Instance.yB = centerY;
            MainWindow.Instance.yB = centerY;
           
            var mainLine = new Line  // Vẽ đoạn thẳng nối A và B
            {
                X1 = MainWindow.Instance.xA,
                Y1 = MainWindow.Instance.yA,
                X2 = MainWindow.Instance.xB,
                Y2 = MainWindow.Instance.yB,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var ALine = new Line
            {
                X1 = MainWindow.Instance.xA,
                Y1 = MainWindow.Instance.yA,
                X2 = MainWindow.Instance.xA,
                Y2 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var BLine = new Line
            {
                X1 = MainWindow.Instance.xB,
                Y1 = MainWindow.Instance.yB,
                X2 = MainWindow.Instance.xB,
                Y2 = MainWindow.Instance.yB - MainWindow.Instance.lineHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var sourceLine1 = new Line
            {
                X1 = MainWindow.Instance.xA,
                Y1 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight,
                X2 = centerX - 15,
                Y2 = MainWindow.Instance.yB - MainWindow.Instance.lineHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var sourceLine2 = new Line
            {
                X1 = MainWindow.Instance.xB,
                Y1 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight,
                X2 = centerX + 15,
                Y2 = MainWindow.Instance.yB - MainWindow.Instance.lineHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var positiveLine = new Line
            {
                X1 = centerX - 15,
                Y1 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight - 20,
                X2 = centerX - 15,
                Y2 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight + 20,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var negativeLine = new Line
            {
                X1 = centerX + 15,
                Y1 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight - 10,
                X2 = centerX + 15,
                Y2 = MainWindow.Instance.yA - MainWindow.Instance.lineHeight + 10,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var positiveLabel = new TextBlock
            {
                Text = "+",
                FontWeight = FontWeights.Bold,
                FontSize = 20,
                Foreground = Brushes.Black
            };
            var negativeLabel = new TextBlock
            {
                Text = "-",
                FontWeight = FontWeights.Bold,
                FontSize = 20,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(positiveLabel, centerX - 40);
            Canvas.SetTop(positiveLabel, MainWindow.Instance.yA - MainWindow.Instance.lineHeight - 30);
            Canvas.SetLeft(negativeLabel, centerX + 27); 
            Canvas.SetTop(negativeLabel, MainWindow.Instance.yB - MainWindow.Instance.lineHeight - 25);

            canvas.Children.Add(mainLine);
            canvas.Children.Add(ALine);
            canvas.Children.Add(BLine);
            canvas.Children.Add(sourceLine1);
            canvas.Children.Add(sourceLine2);
            canvas.Children.Add(positiveLabel);
            canvas.Children.Add(negativeLabel);
            canvas.Children.Add(positiveLine);
            canvas.Children.Add(negativeLine);

            MainWindow.Instance.sourceElements.Add(mainLine);
            MainWindow.Instance.sourceElements.Add(ALine);
            MainWindow.Instance.sourceElements.Add(BLine);
            MainWindow.Instance.sourceElements.Add(sourceLine1);
            MainWindow.Instance.sourceElements.Add(sourceLine2);
            MainWindow.Instance.sourceElements.Add(positiveLabel);
            MainWindow.Instance.sourceElements.Add(negativeLabel);
            MainWindow.Instance.sourceElements.Add(positiveLine);
            MainWindow.Instance.sourceElements.Add(negativeLine);
            
            MainWindow.Instance.AB = mainLine;
            MainWindow.Instance.xPositive = sourceLine1.X2;
            MainWindow.Instance.yPositive = sourceLine1.Y2;
            MainWindow.Instance.xNegative = sourceLine2.X2;
            MainWindow.Instance.yNegative = sourceLine2.Y2;
            MainWindow.Instance.xStraightP = sourceLine1.X1;
            MainWindow.Instance.yStraightP = sourceLine1.Y1;
            MainWindow.Instance.xStraightN = sourceLine2.X1;
            MainWindow.Instance.yStraightN = sourceLine2.Y1;

            InitializeLightColorArray(100);
        }
        public static double ShowInputBoxForResistor(int resistorIndex)
        {
            double returnValue = 0;

            // Tạo và hiển thị cửa sổ nhập giá trị điện trở
            var inputWindow = new AddWindow.NhapGTDienTro
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = true,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false
            };
            // Kiểm tra kết quả nhập liệu
            if (inputWindow.ShowDialog() == true)
            {
                returnValue = inputWindow.ResistorValue; 
                MainWindow.Instance.resistorValues[resistorIndex] = returnValue; 
                UpdateResistorValuesDisplay(); 
            }
            return returnValue;
        }

        public static double[] ShowInputBoxForLight()
        {
            double[] returnValue = null;

            var inputWindow = new AddWindow.NhapGTBongDen
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = true,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false
            };
            // Kiểm tra kết quả nhập liệu
            if (inputWindow.ShowDialog() == true)
            {
                returnValue = new double[2];
                returnValue[0] = inputWindow.P;
                returnValue[1] = inputWindow.R;
                MainWindow.Instance.LightData.Add(returnValue[0], returnValue[1]);
                UpdateResistorValuesDisplay();
            }
            return returnValue;
        }
        public static int[] ShowInputForXoa()
        {
            int[] result = null;
            var inputWindow = new AddWindow.NhapThongTinDeXoa
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = true,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false
            };
            if (inputWindow.ShowDialog() == true)
            {
                result = inputWindow.Result;
            }
            return result;
        }

        public static void UpdateResistorValuesDisplay()
        {
            // Xóa các giá trị cũ trong sidebar
            MainWindow.Instance.Sidebar.Children.Clear();

            // Thêm tiêu đề
            MainWindow.Instance.Sidebar.Children.Add(new TextBlock
            {
                Text = "Giá trị R",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // Hiển thị các giá trị điện trở
            foreach (var kvp in MainWindow.Instance.resistorValues)
            {
                var resistorIndex = kvp.Key;
                var resistorValue = kvp.Value;

                var valueTextBlock = new TextBlock
                {
                    Text = $"R{resistorIndex} = {resistorValue} Ω",
                    FontSize = 14,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5, 0, 0, 5)
                };

                MainWindow.Instance.Sidebar.Children.Add(valueTextBlock);
            }
            UpdateLightValuesDisplay();
        }
        public static void UpdateLightValuesDisplay()
        {
            // Thêm tiêu đề
            MainWindow.Instance.Sidebar.Children.Add(new TextBlock
            {
                Text = "Công suất và điện trở các đèn",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 0, 0, 10)
            });

            int count = 0;

            // Hiển thị các giá trị điện trở và công suất
            foreach (var kvp in MainWindow.Instance.LightData)
            {
                count++;
                var Pvalue = kvp.Key;
                var Rvalue = kvp.Value;

                // Tạo Grid để căn chỉnh
                var grid = new Grid
                {
                    Margin = new Thickness(0, 0, 0, 5)
                };

                // Định nghĩa hai cột
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Cột 1
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Cột 2, rộng hơn để tạo khoảng cách

                // TextBlock cho giá trị P
                var PTextBlock = new TextBlock
                {
                    Text = $"P{count} = {Pvalue}",
                    FontSize = 14,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5, 0, 0, 0)
                };

                // TextBlock cho giá trị R
                var RTextBlock = new TextBlock
                {
                    Text = $"R{count} = {Rvalue} Ω",
                    FontSize = 14,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5, 0, 0, 0)
                };

                // Đặt PTextBlock vào cột 1
                Grid.SetColumn(PTextBlock, 0);
                // Đặt RTextBlock vào cột 2
                Grid.SetColumn(RTextBlock, 1);

                // Thêm các TextBlock vào Grid
                grid.Children.Add(PTextBlock);
                grid.Children.Add(RTextBlock);

                // Thêm Grid vào Sidebar
                MainWindow.Instance.Sidebar.Children.Add(grid);
            }
        }
        public static System.Windows.Threading.DispatcherTimer timer;
        public static void StopDrawingElectrons()
        {
            if (timer != null)
            {
                timer.Stop(); // Dừng timer khi cần
            }
        }
        public static void DrawRealElectron(Canvas CircuitCanvas, double spawnInterval)
        {
            var points = new List<Point>
            {
                new Point(MainWindow.Instance.xPositive, MainWindow.Instance.yPositive),   // Điểm đầu đoạn 1
                new Point(MainWindow.Instance.xStraightP, MainWindow.Instance.yStraightP), // Điểm cuối đoạn 1, đầu đoạn 2
                new Point(MainWindow.Instance.xA, MainWindow.Instance.yA),                 // Điểm cuối đoạn 2, đầu đoạn 3
                new Point(MainWindow.Instance.xB, MainWindow.Instance.yB),                 // Điểm cuối đoạn 3, đầu đoạn 4
                new Point(MainWindow.Instance.xStraightN, MainWindow.Instance.yStraightN), // Điểm cuối đoạn 4, đầu đoạn 5
                new Point(MainWindow.Instance.xNegative, MainWindow.Instance.yNegative)    // Điểm cuối đoạn 5
            };
            int insertIndex = 2; // Vị trí sau điểm xA (index 2)
            var seenXCoordinates = new HashSet<double>();
            var extraPoints = new List<Point>();
            for (int i = 0; i < MainWindow.Instance.beginEparallel.Count; i++)
            {
                var beginPoint = MainWindow.Instance.beginEparallel[i];
                var endPoint = MainWindow.Instance.endEparallel[i];

                // Kiểm tra xem tọa độ X của beginPoint đã có trong seenXCoordinates chưa
                if (!seenXCoordinates.Contains(beginPoint.X))
                {
                    // Nếu chưa có, thêm beginPoint và endPoint vào extraPoints
                    extraPoints.Add(beginPoint);
                    extraPoints.Add(endPoint);

                    // Thêm tọa độ X vào HashSet để theo dõi
                    seenXCoordinates.Add(beginPoint.X);
                }
            }
            points.InsertRange(insertIndex + 1, extraPoints);  // Thêm vào sau xA (index 2)

            double fixedSpeed = 150; // Tốc độ di chuyển cố định (pixel/giây)

            // Tính tổng chiều dài các đoạn
            double totalLength = 0;
            var segmentLengths = new List<double>();
            for (int i = 1; i < points.Count; i++)
            {
                var length = Math.Sqrt(Math.Pow(points[i].X - points[i - 1].X, 2) +
                                       Math.Pow(points[i].Y - points[i - 1].Y, 2));
                segmentLengths.Add(length);
                totalLength += length;
            }

            // Tạo Timer để liên tục sinh electron
            timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(spawnInterval) // Khoảng thời gian giữa mỗi electron
            };

            timer.Tick += (sender, e) =>
            {
                // Tạo electron chính
                var electronContainer = new Canvas
                {
                    Width = 10,
                    Height = 10,
                };
                var electron = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 2
                };
                electronContainer.Children.Add(electron);

                var startPoint = points[0];
                Canvas.SetLeft(electronContainer, startPoint.X);
                Canvas.SetTop(electronContainer, startPoint.Y);
                CircuitCanvas.Children.Add(electronContainer);

                // Tạo Storyboard cho electron chính
                var moveStoryboard = new Storyboard();
                double totalDelay = 0;
                for (int j = 1; j < points.Count; j++)
                { 
                    var fromPoint = points[j - 1];
                    var toPoint = points[j];
                   
                    var segmentTime = segmentLengths[j - 1] / fixedSpeed;

                    var moveXMain = new DoubleAnimation
                    {
                        From = fromPoint.X,
                        To = toPoint.X,
                        Duration = new Duration(TimeSpan.FromSeconds(segmentTime)),
                        BeginTime = TimeSpan.FromSeconds(totalDelay)
                    };
                    var moveYMain = new DoubleAnimation
                    {
                        From = fromPoint.Y,
                        To = toPoint.Y,
                        Duration = new Duration(TimeSpan.FromSeconds(segmentTime)),
                        BeginTime = TimeSpan.FromSeconds(totalDelay)
                    };
                    // Kiểm tra xem electron chính đến điểm nhánh song song
                    if (MainWindow.Instance.beginEparallel.Contains(fromPoint))
                    {
                        // Tính thời điểm electron chính chạm đến điểm nhánh
                        var parallelTriggerTime = TimeSpan.FromSeconds(totalDelay);

                        var parallelTrigger = new DispatcherTimer
                        {
                            Interval = parallelTriggerTime
                        };

                        parallelTrigger.Tick += (ssender, args) =>
                        {
                            parallelTrigger.Stop();
                            double targetX = fromPoint.X;
                            var keyPoints = MainWindow.Instance.fusion.Keys.Where(key => key.X == targetX).ToList();
                            // Kiểm tra xem từ điểm này có điểm song song trong fusion không
                            if (keyPoints.Any())
                            {
                                // Chỉ hiển thị MessageBox 3 lần tối đa
                                //if (MainWindow.Instance.MessageCount < 10)
                                //{
                                //    MainWindow.Instance.MessageCount++;

                                //    // Chuyển đổi các điểm thành chuỗi để hiển thị
                                //    string keyPointsStr = string.Join(", ", keyPoints.Select(p => $"({p.X}, {p.Y})"));
                                //    MessageBox.Show($"Key Points: ({fromPoint.X}, {fromPoint.Y})\nValue Points: {keyPointsStr}");
                                //}
                                // Tạo các electron trên nhánh song song
                                for (int i = 0; i < keyPoints.Count; i++)
                                {
                                    var parallelPoint = keyPoints[i];
                                    var intermediatePoint = new Point(toPoint.X, parallelPoint.Y);

                                    var parallelPoints = new List<Point>
                                    {
                                        fromPoint,  // Điểm bắt đầu
                                        parallelPoint, // Điểm song song từ fusion
                                        intermediatePoint,
                                        toPoint     // Điểm cuối
                                    };

                                    // Tạo electron song song
                                    var parallelElectron = new Canvas
                                    {
                                        Width = 10,
                                        Height = 10,
                                    };
                                    var parallelEllipse = new Ellipse
                                    {
                                        Width = 10,
                                        Height = 10,
                                        Fill = Brushes.Red,
                                        Stroke = Brushes.Yellow,
                                        StrokeThickness = 2
                                    };
                                    parallelElectron.Children.Add(parallelEllipse);
                                    CircuitCanvas.Children.Add(parallelElectron);

                                    // Tạo Storyboard cho electron song song
                                    var parallelStoryboard = new Storyboard();
                                    double parallelTotalDelay = 0;

                                    for (int k = 1; k < parallelPoints.Count; k++)
                                    {
                                        var pFrom = parallelPoints[k - 1];
                                        var pTo = parallelPoints[k];
                                        var pSegmentTime = Math.Sqrt(Math.Pow(pTo.X - pFrom.X, 2) + Math.Pow(pTo.Y - pFrom.Y, 2)) / fixedSpeed;

                                        var moveX = new DoubleAnimation
                                        {
                                            From = pFrom.X,
                                            To = pTo.X,
                                            Duration = new Duration(TimeSpan.FromSeconds(pSegmentTime)),
                                            BeginTime = TimeSpan.FromSeconds(parallelTotalDelay)
                                        };
                                        var moveY = new DoubleAnimation
                                        {
                                            From = pFrom.Y,
                                            To = pTo.Y,
                                            Duration = new Duration(TimeSpan.FromSeconds(pSegmentTime)),
                                            BeginTime = TimeSpan.FromSeconds(parallelTotalDelay)
                                        };
                                        parallelTotalDelay += pSegmentTime;

                                        Storyboard.SetTarget(moveX, parallelElectron);
                                        Storyboard.SetTargetProperty(moveX, new PropertyPath("(Canvas.Left)"));
                                        parallelStoryboard.Children.Add(moveX);

                                        Storyboard.SetTarget(moveY, parallelElectron);
                                        Storyboard.SetTargetProperty(moveY, new PropertyPath("(Canvas.Top)"));
                                        parallelStoryboard.Children.Add(moveY);
                                    }

                                    parallelStoryboard.Completed += (es, eargs) =>
                                    {
                                        CircuitCanvas.Children.Remove(parallelElectron);
                                    };
                                    parallelStoryboard.Begin();
                                }
                            }
                        };
                        parallelTrigger.Start();
                    }
                    else
                    {


                        Storyboard.SetTarget(moveXMain, electronContainer);
                        Storyboard.SetTargetProperty(moveXMain, new PropertyPath("(Canvas.Left)"));
                        moveStoryboard.Children.Add(moveXMain);

                        Storyboard.SetTarget(moveYMain, electronContainer);
                        Storyboard.SetTargetProperty(moveYMain, new PropertyPath("(Canvas.Top)"));
                        moveStoryboard.Children.Add(moveYMain);

                        totalDelay += segmentTime;
                    }
                }


                moveStoryboard.Completed += (s, args) =>
                {
                    CircuitCanvas.Children.Remove(electronContainer);
                };

                moveStoryboard.Begin();
            };

            timer.Start();
        }



        public static void DeleteComponentToDraw(Canvas CircuitCanvas)
        {
            CircuitCanvas.Children.Clear();
            MainWindow.Instance.sourceElements.Clear();
            MainWindow.Instance.gapRec.Clear();
            MainWindow.Instance.labels.Clear();
            MainWindow.Instance.seriesResistors.Clear();
            MainWindow.Instance.seriesLabels.Clear();
            MainWindow.Instance.parallelResistors.Clear();
            MainWindow.Instance.parallelLabels.Clear();
            MainWindow.Instance.connectingLines.Clear();
            MainWindow.Instance.LightBulbs.Clear();
            MainWindow.Instance.ampeList.Clear();
            MainWindow.Instance.voltmeterElements.Clear();
            MainWindow.Instance.indexDot.Clear();

            MainWindow.Instance.fusion.Clear();
        }
        public static void UpdateCircuitAfterAdd(Canvas CircuitCanva, int c)
        {
            MainWindow.Instance.m += 59;
            MainWindow.Instance.n += 59;
            if ((c * 40 - MainWindow.Instance.lineHeight) >= 0)
            {
                MainWindow.Instance.lineHeight += (c * 20);
            }
            UpdateCircuit(CircuitCanva, c);
        }
        public static void UpdateCircuitAfterDelete(Canvas CircuitCanva, int c)
        {
            MainWindow.Instance.m -= 59 * c;
            MainWindow.Instance.n -= 59 * c;
            if ((c * 40 - MainWindow.Instance.lineHeight) >= 0)
            {
                MainWindow.Instance.lineHeight += (c * 20);
            }
            UpdateCircuit(CircuitCanva, c);
        }
        public static void UpdateCircuit(Canvas CircuitCanvas, int c)
        {
            MainWindow.Instance.beginEparallel.Clear();
            MainWindow.Instance.endEparallel.Clear();
            MainWindow.Instance.fusion.Clear();
            MainWindow.Instance.isSave = false;
            DeleteComponentToDraw(CircuitCanvas);
            AddSourceToCanvas(CircuitCanvas, MainWindow.Instance.m, MainWindow.Instance.n);
            int totalUnits = MainWindow.Instance._seriesResistorCount.Count;
            // Tính khoảng cách giữa các điện trở
            double totalLength = Math.Sqrt(Math.Pow(MainWindow.Instance.xB - MainWindow.Instance.xA, 2) + Math.Pow(MainWindow.Instance.yB - MainWindow.Instance.yA, 2));
            double gap = totalLength / (totalUnits + 1);
            double angle = Math.Atan2(MainWindow.Instance.yB - MainWindow.Instance.yA, MainWindow.Instance.xB - MainWindow.Instance.xA); // Góc của đoạn thẳng

            // Thêm các điện trở mới
            int index = 1;  //đánh dấu 
            int num = 1; // dùng để đánh dấu label
            int i = 0;
            foreach (var kvp in MainWindow.Instance._seriesResistorCount)
            {
                double xStart = MainWindow.Instance.xA + index * gap * Math.Cos(angle);
                double yStart = MainWindow.Instance.yA + index * gap * Math.Sin(angle);

                if (kvp.Value == 1)
                {
                    Draw1Resistor(CircuitCanvas, xStart, yStart, gap, angle, num);
                    num++;
                }
                else if (kvp.Value == -1)
                {
                    DrawLight(CircuitCanvas, xStart, yStart, gap, angle);
                }
                else if (kvp.Value == -2)
                {
                    DrawAmpe(CircuitCanvas, xStart, yStart, gap, angle);
                }
                else if (kvp.Value == -3)
                {
                    bool state = MainWindow.Instance.isLightOn[i];
                    DrawKey(CircuitCanvas, xStart, yStart, gap, angle, state, i);
                    i++;
                }
                else
                {
                    DrawManyResistor(CircuitCanvas, xStart, yStart, gap, angle, kvp.Value, num);
                    if (kvp.Value % 2 == 0)
                    {
                        DrawLineForEven(CircuitCanvas, xStart, yStart, gap, angle, kvp.Value);
                    }
                    else
                    {
                        DrawLineForOdd(CircuitCanvas, xStart, yStart, gap, angle, kvp.Value);
                    }
                    num += kvp.Value;
                    MainWindow.Instance.indexofParallel.Add((index, kvp.Value));  //ở vị trí index có kvp.Value điện trở song song (tính index từ 0)
                }
                index++;
            }
            UpdateVol(CircuitCanvas);
            if (!MainWindow.Instance.isVoltmeterMode)
            {
                InvisibleDots();
            }
        }

        public static void UpdateVol(Canvas CircuitCanvas)
        {
            foreach (var element in MainWindow.Instance.voltmeterElements)
            {
                MainWindow.Instance.CircuitCanvas.Children.Remove(element);
            }
            MainWindow.Instance.voltNum = 1;
            DrawDotsAndVoltmeter();
            if (MainWindow.Instance.hadVoltmeter)
            {
                foreach (var pair in MainWindow.Instance.connectedDots)
                {
                    int firstDot = pair.Item1;
                    int secondDot = pair.Item2;
                    bool hasParallel = false;
                    int countParallelRes = 0;
                    foreach (var key in MainWindow.Instance.indexofParallel)
                    {
                        int check = key.Item1;
                        if ((check >= Math.Min(firstDot, secondDot)) &&
                                (check <= Math.Max(firstDot, secondDot)))
                        {
                            hasParallel = true;
                            countParallelRes = Math.Max(key.Item2, countParallelRes); // Lấy giá trị điện trở song song lớn nhất
                            Console.WriteLine($"Parallel found between {firstDot}-{secondDot} at index {check} with {key.Item2} resistors");
                        }
                    }
                    Dot firstdot = MainWindow.Instance.indexDot[firstDot];
                    Dot seconddot = MainWindow.Instance.indexDot[secondDot];
                    DrawVoltmeter(CircuitCanvas, firstdot.dot, seconddot.dot, hasParallel, countParallelRes);
                }
            }
        }

        public static void DrawDotsAndVoltmeter()
        {
            int totalUnits = MainWindow.Instance._seriesResistorCount.Values.Count;
            double totalLength = Math.Sqrt(Math.Pow(MainWindow.Instance.xB - MainWindow.Instance.xA, 2) + Math.Pow(MainWindow.Instance.yB - MainWindow.Instance.yA, 2));
            double gap = totalLength / (totalUnits + 1);
            double angle = Math.Atan2(MainWindow.Instance.yB - MainWindow.Instance.yA, MainWindow.Instance.xB - MainWindow.Instance.xA); // Góc của đoạn thẳng

            // Vẽ dấu chấm giữa các đoạn thẳng nối các điện trở
            for (int i = 0; i <= totalUnits; i++)  // Vẽ dấu chấm giữa các đoạn thẳng nối tiếp
            {
                double xStart = MainWindow.Instance.xA + i * gap * Math.Cos(angle);
                double yStart = MainWindow.Instance.yA + i * gap * Math.Sin(angle);
                double xEnd = MainWindow.Instance.xA + (i + 1) * gap * Math.Cos(angle);
                double yEnd = MainWindow.Instance.yA + (i + 1) * gap * Math.Sin(angle);
                double midX = (xStart + xEnd) / 2;
                double midY = (yStart + yEnd) / 2;

                // Vẽ dấu chấm tại điểm giữa đoạn thẳng
                // Tạo một dấu chấm (ellipse)
                Dot newDot = new Dot
                {
                    index = i, // Gán giá trị cho index
                    dot = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                        Stroke = Brushes.White,
                        StrokeThickness = 2,
                        Cursor = Cursors.Hand
                    }
                };

                // Đặt vị trí cho dấu chấm
                Canvas.SetLeft(newDot.dot, midX - 5); // Căn giữa dấu chấm theo chiều ngang
                Canvas.SetTop(newDot.dot, midY - 5);  // Căn giữa dấu chấm theo chiều dọc
                MainWindow.Instance.indexDot[i] = newDot;
                // Thêm sự kiện chuột để thay đổi màu sắc khi hover
                newDot.dot.MouseEnter += (s, e) =>
                {
                    newDot.dot.Fill = Brushes.Green; // Đổi màu khi ở chế độ vôn kế
                };

                newDot.dot.MouseLeave += (s, e) =>
                {
                    newDot.dot.Fill = Brushes.Black; // Trở lại màu đen khi rời chuột
                };

                // Thêm sự kiện khi nhấn chuột để chọn dấu chấm
                newDot.dot.MouseLeftButtonDown += (s, e) =>
                {
                    if (MainWindow.Instance.firstDot == null && MainWindow.Instance.secondDot == null)
                    {
                        MainWindow.Instance.firstDot = newDot;
                        MainWindow.Instance.firstDot.dot.Fill = Brushes.Green;
                        MessageBox.Show("Please choose the available second dot!");
                    }
                    else if (MainWindow.Instance.firstDot != null && MainWindow.Instance.secondDot == null)
                    {
                        MainWindow.Instance.secondDot = newDot;

                        // Sắp xếp thứ tự của dấu chấm để đảm bảo cặp (1, 2) và (2, 1) được coi là giống nhau
                        var dotPair = MainWindow.Instance.firstDot.index < MainWindow.Instance.secondDot.index ?
                            (MainWindow.Instance.firstDot.index, MainWindow.Instance.secondDot.index) :
                            (MainWindow.Instance.secondDot.index, MainWindow.Instance.firstDot.index);

                        // Kiểm tra nếu cặp này đã tồn tại trong HashSet hoặc chọn cùng một dot
                        if (MainWindow.Instance.connectedDots.Contains(dotPair) || MainWindow.Instance.secondDot == MainWindow.Instance.firstDot)
                        {
                            MessageBox.Show("Cặp này đã nối rồi! Chọn lại cả cặp đi.");
                            ResetSelectedDots();
                        }
                        else
                        {
                            ToolbarButton.BackUpComponent(); //backup trước khi add dotpair
                            MainWindow.Instance.voltNum++;
                            MainWindow.Instance.hadVoltmeter = true;
                            MainWindow.Instance.connectedDots.Add(dotPair);
                            MainWindow.Instance.secondDot.dot.Fill = Brushes.Green;                                               
                            UpdateVol(MainWindow.Instance.CircuitCanvas);

                            //Chỗ này implement add vol kế nè he
                            MainCircuit.AddVolmeter(MainCircuit.mainCircuit, dotPair.Item1 + 1, dotPair.Item2 + 1);
                            
                            ResetSelectedDots();
                        }
                    }
                };
                //MainWindow.Instance.dotDegrees[newDot.dot] = 0;
                // Thêm dấu chấm vào canvas
                MainWindow.Instance.CircuitCanvas.Children.Add(newDot.dot);
            }

        }

        public static void InvisibleDots()
        {
            // Lặp qua tất cả các Dot trong indexDot
            foreach (var key in MainWindow.Instance.indexDot.ToList()) // Sử dụng ToList() để tránh lỗi khi thay đổi dictionary trong vòng lặp
            {
                // Xóa Dot khỏi Canvas
                MainWindow.Instance.CircuitCanvas.Children.Remove(key.Value.dot);

                // Xóa Dot khỏi indexDot
                MainWindow.Instance.indexDot.Remove(key.Key);
            }
        }

        public static void VisibleDots()
        {
            foreach (var key in MainWindow.Instance.indexDot)
            {
                key.Value.dot.Visibility = Visibility.Visible;
            }
        }

        public static void DrawVoltmeter(Canvas Canvas, Ellipse dot1, Ellipse dot2, bool hasParallel, int countParallelRes)
        {
            double x1 = Canvas.GetLeft(dot1) + dot1.Width / 2;
            double y1 = Canvas.GetTop(dot1) + dot1.Height / 2;
            double x2 = Canvas.GetLeft(dot2) + dot2.Width / 2;
            double y2 = Canvas.GetTop(dot2) + dot2.Height / 2;

            double realHeight = 40;
            if (hasParallel)
            {
                realHeight += (countParallelRes * 20);
            }
            double midX = (x1 + x2) / 2; // tọa độ
            double midY = y2 + realHeight * MainWindow.Instance.voltNum;
            MainWindow.Instance.voltNum++;
            // Kích thước điện trở
            double VoltWidth = 60;
            double VoltHeight = 30;

            var line1 = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x1 + 15,
                Y2 = midY,
                Stroke = Brushes.Orange,
                StrokeThickness = 2
            };

            var line2 = new Line
            {
                X1 = x2,
                Y1 = y2,
                X2 = x2 - 15,
                Y2 = midY,
                Stroke = Brushes.Orange,
                StrokeThickness = 2
            };

            var voltmeterLine = new Line
            {
                X1 = x1 + 15,
                Y1 = midY,
                X2 = x2 - 15,
                Y2 = midY,
                Stroke = Brushes.Orange,
                StrokeThickness = 2
            };

            var volt = new Rectangle
            {
                Width = VoltWidth,
                Height = VoltHeight,
                Stroke = Brushes.Black,
                Fill = Brushes.LightGreen,
                StrokeThickness = 2
            };

            Canvas.SetLeft(volt, midX - VoltWidth / 2);
            Canvas.SetTop(volt, midY - VoltHeight / 2);

            // Tạo nhãn cho điện trở, đặt ở giữa điện trở
            var label = new TextBlock
            {
                Text = "V",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = Brushes.Black,
                TextAlignment = TextAlignment.Center
            };

            // Tính vị trí nhãn để nó nằm bên trong điện trở
            Canvas.SetLeft(label, midX - 6);
            Canvas.SetTop(label, midY - 10);

            MainWindow.Instance.CircuitCanvas.Children.Add(line1);
            MainWindow.Instance.CircuitCanvas.Children.Add(line2);
            MainWindow.Instance.CircuitCanvas.Children.Add(voltmeterLine);
            MainWindow.Instance.CircuitCanvas.Children.Add(volt);
            MainWindow.Instance.CircuitCanvas.Children.Add(label);
            MainWindow.Instance.voltmeterElements.Add(line1);
            MainWindow.Instance.voltmeterElements.Add(line2);
            MainWindow.Instance.voltmeterElements.Add(voltmeterLine);
            MainWindow.Instance.voltmeterElements.Add(volt);
            MainWindow.Instance.voltmeterElements.Add(label);
        }



        public static void ResetSelectedDots()
        {
            MainWindow.Instance.firstDot = null;
            MainWindow.Instance.secondDot = null;
        }

        public static void DrawLineForEven(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle, int n)
        {
            // Khoảng cách dọc giữa các điện trở song song
            double verticalGap = 50;

            //Tìm mối nối
            double startX = xCenter - 50;
            double endX = xCenter + 50;

            //thêm vô vẽ animation (điểm đầu cụm)
            MainWindow.Instance.beginEparallel.Add(new Point(startX, yCenter));
            //thêm vô vẽ animation (điểm cuối cụm)
            MainWindow.Instance.endEparallel.Add(new Point(endX, yCenter));
            double length = (n - 1) * verticalGap;
            //Vẽ 2 đường thẳng lên tất cả điện trở(cách điện trở 1 khoảng)
            var DuongTruoc = new Line
            {
                X1 = startX,
                Y1 = yCenter - length / 2, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                X2 = startX,
                Y2 = yCenter + length / 2, // Điểm dưới cùng của đoạn thẳng
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            CircuitCanvas.Children.Add(DuongTruoc);
            MainWindow.Instance.connectingLines.Add(DuongTruoc);

            var DuongSau = new Line
            {
                X1 = endX,
                Y1 = yCenter - length / 2, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                X2 = endX,
                Y2 = yCenter + length / 2, // Điểm dưới cùng của đoạn thẳng
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            CircuitCanvas.Children.Add(DuongSau);
            MainWindow.Instance.connectingLines.Add(DuongSau);

            var gapRectangle = new Rectangle
            {
                Width = Math.Abs(endX - startX) - 2, // Độ rộng (cách một chút để không chạm hai đường thẳng)
                Height = 20, // Độ dày của đoạn bị che (phải lớn hơn hoặc bằng đoạn thẳng chính giữa)
                Fill = Brushes.White,
                StrokeThickness = 0
            };

            // Đặt vị trí của `gapRectangle` ở giữa
            Canvas.SetLeft(gapRectangle, startX + 1); // Tọa độ x (bắt đầu từ `startX`)
            Canvas.SetTop(gapRectangle, yCenter - 10); // Tọa độ y (trung tâm cụm điện trở)
            CircuitCanvas.Children.Add(gapRectangle);
            MainWindow.Instance.gapRec.Add(gapRectangle);

            int distance = 0;
            MainWindow.Instance.beginEparallel.Add(new Point(startX, yCenter));
            //thêm vô vẽ animation (điểm cuối cụm)
            MainWindow.Instance.endEparallel.Add(new Point(endX, yCenter));
            //Vẽ các line từ đường đến điện trở
            while (distance <= length)
            {
                var DuongFront = new Line
                {
                    X1 = startX,
                    Y1 = yCenter - length / 2 + distance, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                    X2 = xCenter - 30,
                    Y2 = yCenter - length / 2 + distance, // Điểm dưới cùng của đoạn thẳng
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                CircuitCanvas.Children.Add(DuongFront);
                MainWindow.Instance.connectingLines.Add(DuongFront);

                var DuongBehind = new Line
                {
                    X1 = endX,
                    Y1 = yCenter - length / 2 + distance, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                    X2 = xCenter + 30,
                    Y2 = yCenter - length / 2 + distance, // Điểm dưới cùng của đoạn thẳng
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                CircuitCanvas.Children.Add(DuongBehind);
                MainWindow.Instance.connectingLines.Add(DuongBehind);
                distance += 50;

                List<Point> tmp = new List<Point>();
                List<Point> temp = new List<Point>();
                //thêm vô vẽ animation (điểm đầu đoạn)
                var keyPoint = new Point(DuongFront.X1, DuongFront.Y1);
                var valuePoint = new Point(DuongBehind.X1, DuongBehind.Y1);
                //MessageBox.Show($"{DuongBehind.Y1} + {DuongBehind.Y2}");
                if (MainWindow.Instance.fusion.ContainsKey(keyPoint))
                {
                    MainWindow.Instance.fusion[keyPoint].Add(valuePoint);
                }
                else
                {
                    // Nếu keyPoint chưa tồn tại, thêm key mới và khởi tạo danh sách
                    MainWindow.Instance.fusion[keyPoint] = new List<Point> { valuePoint };
                }
            }
        }

        public static void DrawLineForOdd(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle, int n)
        {
            // Khoảng cách dọc giữa các điện trở song song
            double verticalGap = 50;

            //Tìm mối nối
            double startX = xCenter - 50;
            double endX = xCenter + 50;

            //thêm vô vẽ animation (điểm đầu cụm)
            MainWindow.Instance.beginEparallel.Add(new Point(startX, yCenter));
            //thêm vô vẽ animation (điểm cuối cụm)
            MainWindow.Instance.endEparallel.Add(new Point(endX, yCenter));

            double length = (n - 1) * verticalGap;
            //Vẽ 2 đường thẳng lên tất cả điện trở (cách điện trở 1 khoảng)
            var DuongTruoc = new Line
            {
                X1 = startX,
                Y1 = yCenter - length / 2, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                X2 = startX,
                Y2 = yCenter + length / 2, // Điểm dưới cùng của đoạn thẳng
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            CircuitCanvas.Children.Add(DuongTruoc);
            MainWindow.Instance.connectingLines.Add(DuongTruoc);

            var DuongSau = new Line
            {
                X1 = endX,
                Y1 = yCenter - length / 2, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                X2 = endX,
                Y2 = yCenter + length / 2, // Điểm dưới cùng của đoạn thẳng
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            CircuitCanvas.Children.Add(DuongSau);
            MainWindow.Instance.connectingLines.Add(DuongSau);

            int distance = 0;
            //Vẽ các line từ đường đến điện trở
            while (distance <= length)
            {
                var DuongFront = new Line
                {
                    X1 = startX,
                    Y1 = yCenter - length / 2 + distance, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                    X2 = xCenter - 30,
                    Y2 = yCenter - length / 2 + distance, // Điểm dưới cùng của đoạn thẳng
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                CircuitCanvas.Children.Add(DuongFront);
                MainWindow.Instance.connectingLines.Add(DuongFront);

                var DuongBehind = new Line
                {
                    X1 = endX,
                    Y1 = yCenter - length / 2 + distance, // Điểm trên cùng của đoạn thẳng (giảm dần theo chiều dài cụm)
                    X2 = xCenter + 30,
                    Y2 = yCenter - length / 2 + distance, // Điểm dưới cùng của đoạn thẳng
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                CircuitCanvas.Children.Add(DuongBehind);
                MainWindow.Instance.connectingLines.Add(DuongBehind);
                distance += 50;


                var keyPoint = new Point(DuongFront.X1, DuongFront.Y1);
                var valuePoint = new Point(DuongBehind.X1, DuongBehind.Y1);

                // Nếu keyPoint đã tồn tại, thêm valuePoint vào danh sách
                if (MainWindow.Instance.fusion.ContainsKey(keyPoint))
                {
                    MainWindow.Instance.fusion[keyPoint].Add(valuePoint);
                }
                else
                {
                    // Nếu keyPoint chưa tồn tại, thêm key mới và khởi tạo danh sách
                    MainWindow.Instance.fusion[keyPoint] = new List<Point> { valuePoint };
                }
            }
        }

        public static void Draw1Resistor(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle, int index)
        {
            // Kích thước điện trở
            double resistorWidth = 60;
            double resistorHeight = 30;

            // Tạo hình chữ nhật đại diện cho điện trở
            var resistor = new Rectangle
            {
                Width = resistorWidth,
                Height = resistorHeight,
                Stroke = Brushes.Black,
                Fill = Brushes.LightYellow,
                StrokeThickness = 2
            };

            // Đặt vị trí cho điện trở
            Canvas.SetLeft(resistor, xCenter - resistorWidth / 2);
            Canvas.SetTop(resistor, yCenter - resistorHeight / 2);

            // Tạo nhãn cho điện trở, đặt ở giữa điện trở
            var label = new TextBlock
            {
                Text = "R" + (index),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = Brushes.Black,
                TextAlignment = TextAlignment.Center
            };

            // Tính vị trí nhãn để nó nằm bên trong điện trở
            Canvas.SetLeft(label, xCenter - 9); // Căn giữa theo chiều ngang
            Canvas.SetTop(label, yCenter - 7);  // Căn giữa theo chiều dọc

            // Thêm vào canvas
            CircuitCanvas.Children.Add(resistor);
            CircuitCanvas.Children.Add(label);

            // Lưu lại phần tử
            MainWindow.Instance.seriesResistors.Add(resistor);
            MainWindow.Instance.seriesLabels[MainWindow.Instance._seriesResistorCount.Count + 1] = label;

            MainWindow.Instance.labels.Add(label); // Lưu nhãn vào danh sách
        }

        public static void DrawManyResistor(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle, int n, int index)
        {
            // Kích thước điện trở
            double resistorWidth = 60;
            double resistorHeight = 30;
            double verticalGap = 50; // Khoảng cách giữa các điện trở song song

            for (int i = 0; i < n; i++)
            {
                double yOffset = (i - (n - 1) / 2.0) * verticalGap; // Tính vị trí dọc

                // Tạo điện trở
                var resistor = new Rectangle
                {
                    Width = resistorWidth,
                    Height = resistorHeight,
                    Stroke = Brushes.Black,
                    Fill = Brushes.LightBlue,
                    StrokeThickness = 2
                };

                // Đặt vị trí
                Canvas.SetLeft(resistor, xCenter - resistorWidth / 2);
                Canvas.SetTop(resistor, yCenter + yOffset - resistorHeight / 2);

                // Tạo nhãn cho từng điện trở
                var label = new TextBlock
                {
                    Text = "R" + (index),
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    Foreground = Brushes.Black,
                    TextAlignment = TextAlignment.Center
                };

                // Đặt vị trí nhãn nằm giữa điện trở
                Canvas.SetLeft(label, xCenter - 9); // Căn giữa theo chiều ngang
                Canvas.SetTop(label, yCenter + yOffset - 7); // Căn giữa theo chiều dọc

                // Thêm vào canvas
                CircuitCanvas.Children.Add(resistor);
                CircuitCanvas.Children.Add(label);

                // Lưu lại phần tử
                MainWindow.Instance.parallelResistors.Add(resistor);
                MainWindow.Instance.parallelLabels[MainWindow.Instance._seriesResistorCount.Count + 1 + i] = label;
                index++;
                MainWindow.Instance.labels.Add(label); // Lưu nhãn vào danh sách
            }
        }

        public static void DrawLight(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle)
        {
            const int circleRadius = 20;
            var circle = new Ellipse
            {
                Width = circleRadius * 2,
                Height = circleRadius * 2,
                Stroke = Brushes.Black,
                Fill = Brushes.Transparent,
                StrokeThickness = 2
            };

            // Đặt vị trí cho hình tròn
            Canvas.SetLeft(circle, xCenter - circleRadius);
            Canvas.SetTop(circle, yCenter - circleRadius);

            // Tạo dấu gạch chéo đầu tiên (dốc lên)
            var diagonal1 = new Line
            {
                X1 = xCenter - circleRadius + 5,
                Y1 = yCenter + circleRadius - 5,
                X2 = xCenter + circleRadius - 5,
                Y2 = yCenter - circleRadius + 5,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Tạo dấu gạch chéo thứ hai (dốc xuống)
            var diagonal2 = new Line
            {
                X1 = xCenter - circleRadius + 5,
                Y1 = yCenter - circleRadius + 5,
                X2 = xCenter + circleRadius - 5,
                Y2 = yCenter + circleRadius - 5,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Thêm vào canvas
            CircuitCanvas.Children.Add(circle);
            CircuitCanvas.Children.Add(diagonal1);
            CircuitCanvas.Children.Add(diagonal2);

            MainWindow.Instance.LightBulbs.Add(circle);
            MainWindow.Instance.LightBulbs.Add(diagonal1);
            MainWindow.Instance.LightBulbs.Add(diagonal2);
        }

        public static void DrawAmpe(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle)
        {
            // Kích thước điện trở
            double AmpeWidth = 60;
            double AmpeHeight = 30;

            // Tạo hình chữ nhật đại diện cho điện trở
            var ampe = new Rectangle
            {
                Width = AmpeWidth,
                Height = AmpeHeight,
                Stroke = Brushes.Black,
                Fill = Brushes.LightGreen,
                StrokeThickness = 2
            };

            // Đặt vị trí cho điện trở
            Canvas.SetLeft(ampe, xCenter - AmpeWidth / 2);
            Canvas.SetTop(ampe, yCenter - AmpeHeight / 2);

            // Tạo nhãn cho điện trở, đặt ở giữa điện trở
            var label = new TextBlock
            {
                Text = "A",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = Brushes.Black,
                TextAlignment = TextAlignment.Center
            };

            // Tính vị trí nhãn để nó nằm bên trong điện trở
            Canvas.SetLeft(label, xCenter - 6); // Căn giữa theo chiều ngang
            Canvas.SetTop(label, yCenter - 9);  // Căn giữa theo chiều dọc

            // Thêm vào canvas
            CircuitCanvas.Children.Add(ampe);
            CircuitCanvas.Children.Add(label);

            // Lưu lại phần tử
            MainWindow.Instance.ampeList.Add(ampe);
            MainWindow.Instance.labels.Add(label); // Lưu nhãn vào danh sách
        }

        public static void DrawKey(Canvas CircuitCanvas, double xCenter, double yCenter, double gap, double angle, bool state, int index)
        {
            MainWindow.Instance.isLightOn[index] = state; //true: đóng
            var keyContainer = new Canvas
            {
                Width = 60, // Kích thước đủ chứa các phần tử
                Height = 60,
            };
            var dot1 = new Ellipse
            {
                Width = 13,  // Đường kính của dấu chấm
                Height = 13,
                Fill = Brushes.Orange, // Màu sắc của dấu chấm
                Stroke = Brushes.Red, // Màu viền của dấu chấm
                StrokeThickness = 3, // Độ dày viền
            };

            var dot2 = new Ellipse
            {
                Width = 13,  // Đường kính của dấu chấm
                Height = 13,
                Fill = Brushes.Orange, // Màu sắc của dấu chấm
                Stroke = Brushes.Red, // Màu viền của dấu chấm
                StrokeThickness = 3, // Độ dày viền
            };

            Canvas.SetLeft(dot1, xCenter - 20);
            Canvas.SetTop(dot1, yCenter - dot1.Width / 2);
            Canvas.SetLeft(dot2, xCenter + 20);
            Canvas.SetTop(dot2, yCenter - dot2.Width / 2);

            // Tạo hình chữ nhật đại diện cho điện trở
            var KeyLineWhenOff = new Line
            {
                X1 = xCenter - 11,
                Y1 = yCenter - 5,
                X2 = xCenter + 27,
                Y2 = yCenter - 25,
                Stroke = Brushes.Red,
                StrokeThickness = 3,
                Visibility = state ? Visibility.Collapsed : Visibility.Visible,
            };

            var KeyLineWhenOn = new Line
            {
                X1 = xCenter - 11,
                Y1 = yCenter - 5,
                X2 = xCenter + 24,
                Y2 = yCenter - 5,
                Stroke = Brushes.Red,
                StrokeThickness = 3,
                Visibility = state ? Visibility.Visible : Visibility.Collapsed,

            };

            var gapLine = new Line
            {
                X1 = xCenter - 7.2,
                Y1 = yCenter,
                X2 = xCenter + 20.2,
                Y2 = yCenter,
                Stroke = Brushes.White,
                StrokeThickness = 3
            };

            // Tạo nhãn cho điện trở, đặt ở giữa điện trở
            var label = new TextBlock
            {
                Text = "K",
                FontWeight = FontWeights.Bold,
                FontSize = 20,
                Foreground = Brushes.Red,
                TextAlignment = TextAlignment.Center
            };

            Canvas.SetLeft(label, xCenter);
            Canvas.SetTop(label, yCenter + 8);

            keyContainer.Children.Add(dot1);
            keyContainer.Children.Add(dot2);
            keyContainer.Children.Add(KeyLineWhenOn);
            keyContainer.Children.Add(KeyLineWhenOff);
            keyContainer.Children.Add(label);
            keyContainer.Children.Add(gapLine);

            // Thêm vào canvas
            CircuitCanvas.Children.Add(keyContainer);

            // Sự kiện khi nhấn vào toàn bộ khóa
            keyContainer.MouseLeftButtonDown += (s, e) =>
            {

                MainWindow.Instance.isLightOn[index] = !MainWindow.Instance.isLightOn[index];
                MainWindow.Instance.isInteractingWithKey = true;

                KeyLineWhenOn.Visibility = MainWindow.Instance.isLightOn[index] ? Visibility.Visible : Visibility.Collapsed;
                KeyLineWhenOff.Visibility = MainWindow.Instance.isLightOn[index] ? Visibility.Collapsed : Visibility.Visible;

                // Animation cho sự thay đổi trạng thái
                var animation = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(0.2),
                    AutoReverse = true
                };
                KeyLineWhenOn.BeginAnimation(UIElement.OpacityProperty, animation);
                KeyLineWhenOff.BeginAnimation(UIElement.OpacityProperty, animation);
            };
            // Hiệu ứng chớp khi di chuột vào bất kỳ phần tử nào của khóa
            keyContainer.MouseEnter += (s, e) =>
            {
                dot1.Fill = Brushes.Green;
                dot2.Fill = Brushes.Green;
                KeyLineWhenOn.Stroke = Brushes.Green;
                KeyLineWhenOff.Stroke = Brushes.Green;
                MainWindow.Instance.isInteractingWithKey = true;
            };

            keyContainer.MouseLeave += (s, e) =>
            {
                MainWindow.Instance.isInteractingWithKey = false;
                dot1.Fill = Brushes.Orange;
                dot2.Fill = Brushes.Orange;
                KeyLineWhenOn.Stroke = Brushes.Red;
                KeyLineWhenOff.Stroke = Brushes.Red;
                MainWindow.Instance.isInteractingWithKey = false;
            };
            keyContainer.MouseLeftButtonUp += (sender, args) =>
            {
                MainWindow.Instance.isInteractingWithKey = false; // Khôi phục trạng thái pan/zoom
            };
        }

        public static void ShowInputBoxForParallelResistors(int n)
        {
            MainCircuit.BackUpBeforeEachBahavior();
            for (int i = 1; i <= n; i++)
            {
                MainCircuit.AddParallel(MainCircuit.mainCircuit, ShowInputBoxForResistor(MainWindow.Instance.resistorCount - n + i));
            }
            MainCircuit.indexOfNode++;
            MainCircuit.mainCircuit.Add(new Node(MainCircuit.indexOfNode));
        }

        public static void InitializeLightColorArray(int size)
        {
            for (int i = 0; i < size; i++)
            {
                MainWindow.Instance.LightColorArray.Add(Brushes.Transparent);
            }
        }
    }
}
