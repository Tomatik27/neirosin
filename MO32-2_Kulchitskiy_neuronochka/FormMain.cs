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


        public FormMain()
        {
            InitializeComponent();
            inputPixels = new double[15]; // инициализация массива
            network = new Network();
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
        }

        private void buttonSaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = CorrectAnswer.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void buttonSaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = CorrectAnswer.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(inputPixels);
            labelOutput.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelProbability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
        }

    }
}
