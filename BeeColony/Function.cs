using org.mariuszgromada.math.mxparser;

namespace BeeColony
{
    class Function
    {
        readonly Argument X1 = new Argument("x1");
        readonly Argument X2 = new Argument("x2");
        public Expression UserFunction = new Expression();

        public Function(string userFunction)
        {
            UserFunction = new Expression(userFunction, X1, X2);
        }

        public double Calculate(double x1,  double x2)
        {
            X1.setArgumentValue(x1);
            X2.setArgumentValue(x2);
            return UserFunction.calculate();
        }
    }
}
