using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace LiveChartTest
{
    public partial class Form1 : Form
    {
        DataPreparation dataPreparation = new DataPreparation();
        Import imp;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            button2.Visible = false;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            var path = openFileDialog1.FileName;
            label1.Text = path;
            openFileDialog1.Dispose();

            try
            {
                imp = new Import(path);
                imp.PrepareText();
                dataPreparation.parseData(imp.text);
                this.comboBox1.DataSource = dataPreparation.header.Where(x => !x.Contains("Date")).ToList();
                comboBox1.Visible = true;
            }
            catch (Exception error) 
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Path.ChangeExtension(imp.Path,"csv");
            var newFileName =
                Path.GetFileNameWithoutExtension(imp.Path)
                + dataPreparation.RemoveInvalidChars(comboBox1.SelectedItem.ToString())
                + ".csv";
            var fullPath = Path.Combine(Path.GetDirectoryName(imp.Path), newFileName);

            using (var streamWrite = new System.IO.StreamWriter(fullPath)) {
                streamWrite.WriteLine("Data," + comboBox1.SelectedItem.ToString());
                foreach (var line in dataPreparation.dataToPrint) {
                    streamWrite.WriteLine(line.Key + "," + line.Value.ToString().Replace(",","."));
                }
            }
            MessageBox.Show("Save in file " + newFileName);

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;
            var selected = comboBox1.SelectedItem.ToString();
            try
            {
                dataPreparation.switchChart(selected);
                printData();
                button2.Visible = true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void printData()
        {
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Time",
                Labels = dataPreparation.dataToPrint.Keys.Select(x => x.ToShortTimeString()).ToList(),
                
            });
            cartesianChart1.Series.Clear();
            var chart = new ChartValues<double>();
            double[] doubleArray = new double[dataPreparation.dataToPrint.Values.Count];
            dataPreparation.dataToPrint.Values.ToArray().CopyTo(doubleArray, 0);
            chart.AddRange(doubleArray);
            SeriesCollection series = new SeriesCollection();
            series.Add(new LineSeries()
            {
                Title = "Value",
                Values = chart,
                AllowDrop = true,
                PointGeometry = null
            });
            cartesianChart1.Hoverable = false;
            cartesianChart1.DisableAnimations = true;
            //cartesianChart1.DataTooltip = null;
            cartesianChart1.Series = series;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            cartesianChart1.Height = this.DisplayRectangle.Height - cartesianChart1.Location.Y;
            cartesianChart1.Width = this.DisplayRectangle.Width - cartesianChart1.Location.X;
        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
