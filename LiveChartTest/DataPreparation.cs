using System;
using System.Collections.Generic;
using System.IO;

namespace LiveChartTest
{
    class DataPreparation
    {
        private const char koniecLini = ';';
        private const char separatorKomorki = '@';

        private List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
        private List<string> rawData;
        public List<string> header;
        public Dictionary<DateTime, double> dataToPrint = new Dictionary<DateTime, double>();

        public void parseData(string inputData)
        {
            header = null;
            rawData = new List<string>(inputData.Split(koniecLini));
            data = new List<Dictionary<string, string>>();
            foreach (var item in rawData)
            {
                if (item == "") continue;
                if (header == null) { header = new List<string>(item.Split(separatorKomorki)); continue; }
                //add missing data or split 
                data.Add(PrepareRowData(item.Split(separatorKomorki)));
            }
        }
        private Dictionary<string, string> PrepareRowData(string[] row)
        {
            var temp = new Dictionary<string, string>();
            int i = 0;
            foreach (var item in header)
            {
                temp.Add(item, row[i]);
                i++;
            }
            return temp;
        }

        internal void switchChart(string chartType)
        {
            dataToPrint.Clear();
            dataToPrint = new Dictionary<DateTime, double>();
            foreach (var item in data)
            {
                string time = "";
                string value = "";

                item.TryGetValue(header.ToArray()[0], out time);
                item.TryGetValue(chartType, out value);

                var parseTime = DateTime.Parse(time);
                var parseValue = Double.Parse(value.Replace(".", ","));

                dataToPrint.Add(parseTime, parseValue);
            }
        }
        public string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
