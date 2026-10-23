namespace _31_2_pavlov_arsenchik_Levchenko_1.NeuroNet
{
    class Network
    {
        //все слои сети
        private InputLayer input_layer = null;
        private HiddenLayer hidden_Layer1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_Layer1));
        private HiddenLayer hidden_Layer2 = new HiddenLayer(30, 70, NeuronType.Hidden, nameof(hidden_Layer2));
        private OutputLayer output_layer = new OutputLayer(10, 30, NeuronType.Hidden, nameof(output_layer));


        private double[] fact = new double[10];
        private double[] e_error_avr;


        public double [] Fact { get => fact; }


        public double[] E_error_avg { get => e_error_avr; set => e_error_avr = value; }


        public Network() { }


        public void ForwardPass(double[] netInput)
        {
            hidden_Layer1.Data = netInput;
            hidden_Layer1.Recognize(null, hidden_Layer2);
            hidden_Layer2.Recognize(null, output_layer);
            output_layer.Recognize(this, null);


        }
    }
}
