using System;

namespace LiveChartTest
{
    class DataUnit
    {
        public DataUnit(DateTime parseTime, double parseValue)
        {
            this.Data = parseTime;
            this.Value = parseValue;
        }

        public DateTime Data { get; set; }
        public double Value { get; set; }
    }
}
