using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeColony 
{
    class Bee : INotifyPropertyChanged
    {
        private double _x1Value;
        private double _x2Value;
        private double _rezult;

        public double X1Value
        {
            get { return _x1Value; }
            set { _x1Value = value; }
        }

        public double X2Value
        {
            get { return _x2Value; }
            set { _x2Value = value; }
        }

        public double Rezult
        {
            get { return _rezult; }
            set { _rezult = value; }
        }

        public Bee(double x1, double x2, double rezult)
        {
            X1Value = x1;
            X2Value = x2;
            Rezult = rezult;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
