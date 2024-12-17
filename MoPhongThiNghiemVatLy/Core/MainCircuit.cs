using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace MoPhongThiNghiemVatLy
{
    internal class MainCircuit
    {
        public static List<CircuitDiagram> removedCircuit = new List<CircuitDiagram>(); // cái này hổng có dùng nữa
        public static int indexOfNode = 0;
        public static int indexOfRes = 0;
        public static int indexOfVol = 0;
        public static int indexOfAmpe = 0;
        public static int indexOfSwitch = 0;
        public static int indexOfLight = 0;

        public static List<CircuitDiagram> mainCircuit = new List<CircuitDiagram>();
        public static Stack<List<CircuitDiagram>> history = new Stack<List<CircuitDiagram>>();  // Lịch sử các hành động (cho undo)
        public static Stack<List<CircuitDiagram>> redoStack = new Stack<List<CircuitDiagram>>();  // Lịch sử các hành động đã undo (cho redo)
        public static void AddSeries(List<CircuitDiagram> list, double value)
        {
            BackUpBeforeEachBahavior();
            indexOfRes += 1;
            list.Add(new Resistor(value, indexOfRes));
            indexOfNode += 1;
            list.Add(new Node(indexOfNode));
        }
        public static void AddParallel(List<CircuitDiagram> list, double value)
        {
            indexOfRes += 1;
            list.Add(new Resistor(value, indexOfRes));
        }
        public static void AddVolmeter(List<CircuitDiagram> list, int l, int r)
        {
            BackUpBeforeEachBahavior();
            int index = 0;
            for (int i = 0; i < list.Count; i++)
            {

                if (list[i].Type == DEFINE.TYPE_Node)
                {
                    if (list[i].Indexer == l)
                    {
                        index = i + 1;
                    }
                }
            }
            indexOfVol += 1;
            list.Insert(index, new Volmeter(l, r, indexOfVol));
            MessageBox.Show($"Đã thêm vol kế thứ {indexOfVol}. Mặc tại node: {l} và {r}");
        }
        public static void AddAmmeter(List<CircuitDiagram> list)
        {
            BackUpBeforeEachBahavior();
            indexOfAmpe += 1;
            list.Add(new Ammeter(indexOfAmpe));
            indexOfNode += 1;
            list.Add(new Node(indexOfNode));
        }
        public static void AddLight(List<CircuitDiagram> list, double[] data)
        {
            BackUpBeforeEachBahavior();
            indexOfLight += 1;
            list.Add(new Light(indexOfLight, data ));
            indexOfNode += 1;
            list.Add(new Node(indexOfNode));
        }
        public static void AddSwitch(List<CircuitDiagram> list)
        {
            BackUpBeforeEachBahavior();
            indexOfSwitch += 1;
            list.Add(new Switch(indexOfSwitch));
            indexOfNode += 1;
            list.Add(new Node(indexOfNode));
        }

        public static void RemoveDetail(List<CircuitDiagram> list, int type, int index)
        {
            if (type == DEFINE.TYPE_Switch && type == DEFINE.TYPE_Node) return;
            BackUpBeforeEachBahavior();
            // case xóa vol kế
            if (type == DEFINE.TYPE_Vol)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Type == type && list[i].Indexer == index)
                    {
                        list.RemoveAt(i);
                        ReplaceInformation(list, type);
                        return;
                    }
                }
            } // return ngay vì không cần hiệu chỉnh gì nữa cả
            bool wasDeleted = false; // check có xóa node ko. nếu có thì sẽ phải hiệu chỉnh l-r props của vol kế
            int indexOfNodeHadDeleted = 0; // index của node bị xóa nếu có. 
            bool wasRemoved = false; // đã xóa thành công chi tiết. break ngay
            for (int i = 0; i < list.Count; i++)
            {
                if (wasRemoved) break;
                if (list[i].Type == DEFINE.TYPE_Node && i + 1 == list.Count)
                    break;
                //Case này là case cho những cái nối tiếp
                if (list[i].Type == DEFINE.TYPE_Node && i + 2 < list.Count)
                {
                    if ((list[i].Type == DEFINE.TYPE_Node) && (list[i + 1].Type == type) && (list[i + 2].Type == DEFINE.TYPE_Node))
                    {
                        if (list[i+1].Indexer == index)
                        {
                            i = i + 1;
                            list.RemoveAt(i); // delete detail
                            wasDeleted = true;
                            indexOfNodeHadDeleted = list[i].Indexer;
                            list.RemoveAt(i); // xóa node bị thừa đi
                            ReplaceInformation(list, type); // sửa lại các index cần thiết
                            break;
                        }
                    }
                }

                // case này là case có vol kế nhiều và liêc tục
                if (list[i].Type == DEFINE.TYPE_Node && i + 3 < list.Count)
                {
                    for (int j = i+1; j < list.Count; j++)
                    {
                        if (list[j].Type == DEFINE.TYPE_Vol)
                        {
                            if (list[j+1].Type == type && list[j+1].Indexer == index)
                            {
                                list.RemoveAt(j+1);
                                if (list[j+1].Type == DEFINE.TYPE_Node)
                                {
                                    indexOfNodeHadDeleted = list[j+1].Indexer;
                                    list.RemoveAt(j+1);
                                    wasDeleted = true;
                                }
                                ReplaceInformation(list, type); // sửa lại các index cần thiết
                                break;
                            }
                        }
                    }
                }
                //case này là case ko có j cần quan tâm
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[j].Type == type && list[j].Indexer == index)
                    {
                        list.RemoveAt(j);
                        ReplaceInformation(list, type); // sửa lại các index cần thiết
                        wasRemoved = true;
                        break;
                    }
                }
            }
            if (wasDeleted)
            {
                ModifyAllVolAfterErasingNode(list, indexOfNodeHadDeleted);
            }
        }
        private static void ModifyAllVolAfterErasingNode(List<CircuitDiagram> list, int index)
        {
            string res = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type != DEFINE.TYPE_Vol) continue;
                int[] pos = list[i].LeftRight();
                int l = pos[0];
                int r = pos[1];
                res += l + " " + r;
                if (index <= l)
                {
                    l -= 1; r -= 1;
                }
                else if (index <= r)
                    r -= 1;
                // sau hiệu chỉnh 
                if (l <= 0 || l >= r)
                {
                    RemoveVolmeterIfCantModify(list, DEFINE.TYPE_Vol, list[i].Indexer);
                }
                else
                    list[i] = new Volmeter(l, r, list[i].Indexer);
            }
            ReplaceInformation(list, DEFINE.TYPE_Vol);
            MessageBox.Show(res);
            return;
        }
        private static void RemoveVolmeterIfCantModify(List<CircuitDiagram> list, int type, int index)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == type && list[i].Indexer == index)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }

        private static void ReplaceInformation(List<CircuitDiagram> list, int type)
        {
            indexOfNode = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == DEFINE.TYPE_Node)
                {
                    indexOfNode++;
                    list[i].Indexer = indexOfNode;
                }
            }
            int newIndex = 0;
            switch (type)
            {
                case DEFINE.TYPE_Res:
                    indexOfRes -= 1;
                    break;
                case DEFINE.TYPE_Ampe:
                    indexOfAmpe -= 1;
                    break;
                case DEFINE.TYPE_Vol:
                    indexOfVol -= 1;
                    break;
                case DEFINE.TYPE_Light:
                    indexOfLight -= 1;
                    break;
                case DEFINE.TYPE_Switch:
                    indexOfSwitch -= 1;
                    break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == type)
                {
                    newIndex++;
                    list[i].Indexer = newIndex;
                }
            }
        }

        public static void BackUpBeforeEachBahavior()
        {
            history.Push(mainCircuit.ToList());
        }
        public static void Undo()
        {
            if (history.Count == 0) return;
            redoStack.Push(mainCircuit.ToList());
            mainCircuit.Clear();
            mainCircuit = history.Pop();
            RefillIndexOfNode(mainCircuit);
            ShowListCurrent();
        }
        public static void Redo()
        {
            if (redoStack.Count == 0) return;
            BackUpBeforeEachBahavior();
            mainCircuit.Clear();
            mainCircuit = redoStack.Pop();
            ShowListCurrent();
        }
        private static void RefillIndexOfNode(List<CircuitDiagram> list)
        {
            indexOfNode = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == DEFINE.TYPE_Node)
                {
                    indexOfNode++;
                    list[i].Indexer = indexOfNode;
                }
            }
        }
        private static void CopyList(List<CircuitDiagram> list1, List<CircuitDiagram> list2)
        {
            list1.Clear();
            for (int i = 0; i < list2.Count; i++)
            {
                list1.Add(list2[0]);
                list2.RemoveAt(0);
            }
        }
        public static void ShowListCurrent()
        {
            MessageBox.Show($"{mainCircuit.Count}");
        }

    }
}
