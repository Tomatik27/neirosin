namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class Network
    {
        // слои
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(34, 70, NeuronType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 34, NeuronType.Output, nameof(output_layer));


        private double[] fact = new double[10];  // фактический вывод
        private double[] e_error_avr;            // среднее значение энергии ошибки

        // свойства
        public double[] Fact { get => fact; }   // геттер вывода
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        // конструктор
        public Network() { }

        // прямой проход НС
        public void ForwardPass(double[] netInput)
        {
            hidden_layer1.Data = netInput;
            hidden_layer1.Recognize(null, hidden_layer2);
            hidden_layer2.Recognize(null, output_layer);
            output_layer.Recognize(this, null);
        }

    }
}
