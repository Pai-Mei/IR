using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRsoft
{
    public class MultiValuePoint
    {
        public Int32 Dimenision { get; private set; }
        public double X { get; set; }
        public double[] Values { get; private set; }

        public MultiValuePoint()
        {

        }

        public MultiValuePoint(double X, params double[] Values)
        {
            this.X = X;
            this.Values = Values;
            Dimenision = Values.Length;
        }
    }

    public class Profile : List<MultiValuePoint>
    {
        public Int32 Dimenision;
        public String[] Labels;

        public Profile()
        {
            
        }

        public Profile(params String[] Labels)
        {
            this.Labels = Labels;
            this.Dimenision = Labels.Length;
        }

        public void Add(double x, params double[] Values)
        {
            if(Dimenision == 0)
            {
                Add(new MultiValuePoint(x, Values));
                this.Dimenision = Values.Length;
            }
            if (this.Dimenision != Values.Length)
                throw new WrongDimension();
            Add(new MultiValuePoint(x, Values));
        }

        public class WrongDimension : ArgumentOutOfRangeException
        {
            public WrongDimension() : base("Wrong count of argument")
            {
            }
        }
    }
}
