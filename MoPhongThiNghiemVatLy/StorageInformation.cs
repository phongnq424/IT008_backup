using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoPhongThiNghiemVatLy
{
    internal class StorageInformation
    {
        public Dictionary<int, int> data;
        public HashSet<(int, int)> dots;
        public double m;
        public double n;

        public StorageInformation(Dictionary<int, int> _data, HashSet<(int, int)> _dots, double _m, double _n) 
        {
            data = _data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            dots = _dots.ToHashSet();
            m = _m;
            n = _n;
        }

        public void ReturnValueOfCircuit()
        {
            MainWindow.Instance._seriesResistorCount = data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            MainWindow.Instance.connectedDots = dots;
            MainWindow.Instance.m = m;
            MainWindow.Instance.n = n;
        }
    }
}
