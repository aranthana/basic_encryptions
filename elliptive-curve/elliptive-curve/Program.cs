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
            y = - m * (x - oldX) - y;
            WriteLine("Doubled point : " + x + "," + y);
        }

        private static void AddPointsFiniteField(ref double x_r, ref double y_r, double x, double y)
        {
            int prime = 1613;
            WriteLine("Points to be added : " + x_r + "," + y_r + " + " + x + "," + y);
            double m = ((y_r - y) * ModInversCalculation((int)(x_r - x),prime)) % prime;
            x_r = ((m * m) - (x_r + x)) % prime;
            y_r = (m * (x - x_r) - y) % prime;
            WriteLine("Added point and generated : " + x_r + " ," + y_r);
        }

        private static void DoubleThePointFiniteFiled(ref double x, ref double y)
        {
            int prime = 1613;
            WriteLine("Points to be doubled : " + x + "," + y);
            double oldX = x;
            double m =(((3 * x * x) + 3) * ModInversCalculation((int)(2 * y), prime)) % prime ;
            x = (m * m - 2 * x) % prime;
            y = (m * (oldX-x) - y) % prime;
            WriteLine("Doubled point : " + x + "," + y);
        }

          /*  Method to calculate mod inverse. */
        private static int ModInversCalculation(int a, int m)
        {

            int g = Gcd(a, m);
            if (g != 1)
                WriteLine("Inverse doesn't exist");
            else
            {
                // If a and m are relatively
                // prime, then modulo inverse
                // is a^(m-2) mode m
                WriteLine(
                    "Modular multiplicative inverse is "+Power(a, m - 2, m)
                    );
            }
            return Power(a, m - 2, m);
        }

        // To compute x^m-2 under
        // modulo m
        static int Power(int x, int y, int m)
        {
            if (y == 0)
                return 1;
    
            int p = Power(x, y / 2, m) % m;
            p = (p * p) % m;
            if (y % 2 == 0)
                return p;
            else
                return (x * p) % m;
        }

        // Function to return
        // gcd of a and b
        static int Gcd(int a, int b)
        {
            if (a == 0)
                return b;
            return Gcd(b % a, a);
        }


    }


























}

