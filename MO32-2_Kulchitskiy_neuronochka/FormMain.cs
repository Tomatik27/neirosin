using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MO32_2_Kulchitskiy_neuronochka.NeuroNet;

namespace MO32_2_Kulchitskiy_neuronochka
{
    public partial class FormMain : Form
    {

        private double[] inputPixels;
        private Network network;

        // UI для отображения шансов по каждой цифре
        private Label[] digitLabels;
        private Label[] percentLabels;
        private const int StatsRows = 10;


        public FormMain()
        {
            InitializeComponent();
            inputPixels = new double[15]; // инициализация массива
            network = new Network();
            CreateStatsPanel();
        }

        // создаём панель: 10 строк «цифра | процент»
        private void CreateStatsPanel()
        {
            digitLabels = new Label[StatsRows];
            percentLabels = new Label[StatsRows];

            int startX = 191;   // правее поля с пикселями
            int startY = 80;    // ниже labelOutput / labelProbability
            int rowHeight = 22;
            int rowHeightPx = 18;

            for (int i = 0; i < StatsRows; i++)
            {
                int y = startY + i * rowHeight;

                // цифра
                digitLabels[i] = new Label();
                digitLabels[i].Text = i.ToString();
                digitLabels[i].TextAlign = ContentAlignment.MiddleCenter;
                digitLabels[i].Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                digitLabels[i].Location = new Point(startX, y);
                digitLabels[i].Size = new Size(20, rowHeightPx);

                // проценты
                percentLabels[i] = new Label();
                percentLabels[i].Text = "0.00 %";
                percentLabels[i].Location = new Point(startX + 28, y);
                percentLabels[i].Size = new Size(70, rowHeightPx);

                Controls.Add(digitLabels[i]);
                Controls.Add(percentLabels[i]);
            }
        }

        // обновляем проценты
        private void UpdateStats()
        {
            double max = network.Fact.Max();
            for (int i = 0; i < StatsRows; i++)
            {
                double p = network.Fact[i];
                percentLabels[i].Text = (100 * p).ToString("0.00") + " %";
                digitLabels[i].ForeColor = (p == max) ? Color.Green : Color.Black;
                percentLabels[i].ForeColor = (p == max) ? Color.Green : Color.Black;
            }
        }

        private void on_button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.White)
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 1d;

            }
            else
            {
                ((Button)sender).BackColor = Color.White;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }

            Recognize();
        }

        private void Recognize()
        {
            network.ForwardPass(inputPixels);
            labelOutput.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelProbability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
            UpdateStats();
        }

        private void buttonSaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = CorrectAnswer.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void buttonSaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = CorrectAnswer.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            Recognize();
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            network.Train(network);

            for (int i = 0; i < network.E_error_avr.Length; i++)
            {
                chartEavr.Series[0].Points.AddY(network.E_error_avr[i]);
            }

            MessageBox.Show("Обучение завершено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            network.Test();

            for (int i = 0; i < network.E_error_avr.Length; i++)
            {
                chartEavr.Series[0].Points.AddY(network.E_error_avr[i]);
            }
            MessageBox.Show("Тестирование завершено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}