using static System.Console;

namespace elliptivecurve{
    class ElliptiveCurve
    {
        public static void Main(string[] args)
        {
            if(ReadUserValues(out double x,  out int n))
            {
                double x_r = x;
                GenerateY( x, out double y);
                double y_r = y;
                List<int> binaryList = ConvertToBinary(n);
                int zeroCount = 0;
                bool firstOne = true;
                for (int i = 0; i < binaryList.Count; i++)
                {
                    
                    if (binaryList[i] == 1)
                    {
                        while (zeroCount > 0)
                        {
                            DoubleThePoint(ref x, ref y);
                            zeroCount--;
                        }
                        if (firstOne)
                        {
                            firstOne = false;
                            x_r = x;
                            y_r = y;
                        }
                        else
                        {
                            DoubleThePoint(ref x, ref y);
                            AddDifferentPoints(ref x_r, ref y_r, x, y);
                        }
                    }
                    else
                        zeroCount++;
                }
                WriteLine("Final value : " + x_r + "," + y_r );

            }
        }


        private static bool ReadUserValues( out double x,  out int n)
        {
            bool validInput = true;
            Write("Enter the x point: ");
            if (!double.TryParse(ReadLine(), out x))
                validInput = false;
            
            Write("Enter the n point: ");
            if (!int.TryParse(ReadLine(), out n))
                validInput = false;
            return validInput;
        }
         private static List<int> ConvertToBinary (int n)
        {
            List<int> binaryList = new List<int>();
            do
            {
                binaryList.Add(n % 2);
                n = n / 2;
            } while (n!=0);
            return binaryList;
        }
        private static void GenerateY(double x,out double y)
        {
            y = Math.Sqrt( Math.Pow(x, 3) + 3 * x + 5);
        }

        private static void AddDifferentPoints(ref double x_r,ref double y_r, double x, double y)
        {
            WriteLine("Points to be added : "+x_r +"," +y_r + " + " + x + "," + y);
            double m= (y_r - y) / (x_r - x);
            x_r =  (m * m) - (x_r + x);
            y_r = -(m * (x_r - x)) - y;
            WriteLine("Added point and generated : " + x_r + " ," + y_r);
        }

        private static void DoubleThePoint(ref double x, ref double y)
        {
            WriteLine("Points to be doubled : " + x + "," + y);
            double oldX = x;
            double m = ((3 * x * x) + 3) / (2 * y);
            x = m * m - 2 * x;
            y = - Math.Round(m, 5) * (Math.Round(x,5) - oldX) - y;
            WriteLine("Doubled point : " + x + "," + y);
        }
    }


























}

