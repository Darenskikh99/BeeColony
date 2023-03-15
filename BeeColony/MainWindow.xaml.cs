using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeeColony
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            BeeColonyAI myBeeColony = new BeeColonyAI(int.Parse(X1Left.Text), int.Parse(X1Right.Text), int.Parse(X2Left.Text), int.Parse(X2Right.Text), Func.Text);
            myBeeColony.GetAnswer(int.Parse(SizePopulation.Text), int.Parse(SizeBestAgents.Text), int.Parse(SizeOrdinaryAgents.Text), int.Parse(NumberIteration.Text));
            MinFunc.Text = myBeeColony.FunctionMin.ToString();
            X1Min.Text = myBeeColony.X1Min.ToString();
            X2Min.Text = myBeeColony.X2Min.ToString();
        }
    }
}
