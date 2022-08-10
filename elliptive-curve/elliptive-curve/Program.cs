using static System.Console;

namespace elliptivecurve{
    class ElliptiveCurve
    {
        public static void Main(string[] args)
        {
            if(ReadUserValues(out double x,  out int d))
            {
                double x_r = x;
                GenerateY( x, out double y);
                double y_r = y;
                int prime = 1613;
                ScalarMultiplication(d, x, y, ref x_r, ref y_r,prime);
                WriteLine("Final value for public key: " + x_r + "," + y_r+","+x+","+y );
                GenerateSignature(d, x, y, out int hashedMessage, out double signature, out double x_random,prime);
                VerifySignature(signature, hashedMessage, x_random, x, y, x_r, y_r,prime);

            }
        }


        private static bool ReadUserValues( out double x,  out int d)
        {
            bool validInput = true;
            Write("Enter the x point for generatot point: ");
            if (!double.TryParse(ReadLine(), out x))
                validInput = false;
            
            Write("Enter an integer for private key: ");
            if (!int.TryParse(ReadLine(), out d))
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
           Write("Enter the y value for Generator point : ");
            double.TryParse(ReadLine(),out y);
            //y = 19;
            //y= Math.Sqrt( Math.Pow(x, 3) + (3 * x) + 5);
            //WriteLine("y" + y);
        }
        private static void GeneraateGPoint()
        {

        }

        private static void ScalarMultiplication(int d, double x, double y, ref double x_r, ref double y_r,int prime)
        {
            //WriteLine("print d " + d+"prime :" +prime);
            List<int> binaryList = ConvertToBinary(d);
            int zeroCount = 0;
            bool firstOne = true;
            for (int i = 0; i < binaryList.Count; i++)
            {

                if (binaryList[i] == 1)
                {
                    while (zeroCount > 0)
                    {
                        //DoubleThePoint(ref x, ref y);
                        DoubleThePointFiniteFiled(ref x, ref y,prime);
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
                        // DoubleThePoint(ref x, ref y);
                        DoubleThePointFiniteFiled(ref x, ref y,prime);
                        //AddDifferentPoints(ref x_r, ref y_r, x, y);
                        AddPointsFiniteField(ref x_r, ref y_r, x, y,prime);
                    }
                }
                else
                    zeroCount++;
            }
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

        private static void AddPointsFiniteField(ref double x_r, ref double y_r, double x, double y, int prime)
        {
            
            //WriteLine("Points to be added : " + x_r + "," + y_r + " + " + x + "," + y);
            double m = Mod((y_r - y) * ModInversCalculation((int)(x_r - x),prime) , prime);
            x_r = Mod((m * m) - (x_r + x), prime);
            y_r = Mod(m * (x - x_r) - y , prime);
            //WriteLine("Added point and generated : " + x_r + " ," + y_r);
        }

        private static void DoubleThePointFiniteFiled(ref double x, ref double y,int prime)
        {
            //WriteLine("Points to be doubled : " + x + "," + y);
            double oldX = x;
            double m =Mod((3 * x * x + -2) * ModInversCalculation((int)(2 * y), prime), prime) ;
            x = Mod(m * m - (2 * x),  prime);
            y = Mod(m * (oldX-x) - y, prime);
            //WriteLine("Doubled point : " + x + "," + y);
        }
        private static void GenerateSignature(int d,double x, double y, out int hashedMessage, out double signature, out double x_random, int prime)
        {
            
            Random rnd = new Random();
            int k = rnd.Next(prime);// shoule be below the the number of p[oints in the curve
            WriteLine("Random number: " + k);
            x_random = x;
            double y_random = y;
            //WriteLine("Random point before: "+k+"," + x_random + "," + y_random + "," + x + "," + y);
            ScalarMultiplication(k, x, y, ref x_random, ref y_random, prime);
            WriteLine("Random point: " + x_random + "," + y_random + "," + x + "," + y);
            x_random = x_random % prime;
            WriteLine("Random point x: " + x_random );
            hashedMessage = HashMessage(prime);
            WriteLine("Hashed Message: " + hashedMessage);
            signature = (ModInversCalculation(k, prime) * (hashedMessage + d * x_random)) % prime;
            WriteLine("Signature: " + signature );
            if(signature==0)
                GenerateSignature(d, x, y, out  hashedMessage, out  signature, out  x_random, prime);
          


        }
        private static int HashMessage(int prime)
        {
            int n = 1;
            do
            {
                n++;
            } while (Math.Pow(2, n) < prime);
            Random rnd = new Random();
            int random = rnd.Next(prime);
            return random * n<0? -random * n: random * n;
        } 

        private static void VerifySignature(double signature, int hashedMessage,double x_random, double x, double y,double x_r, double y_r, int prime)
        {
            WriteLine("Verify with : " + signature + "," + hashedMessage + "," + x_random + "," + x +","+y+", " + x_r + "," + y_r);
          
            double x_1 = x;
            double y_1 = y;
            ScalarMultiplication((ModInversCalculation((int)signature, prime) * hashedMessage) % prime, x, y, ref x_1, ref y_1, prime);
            WriteLine("Generated Point 1: " + x_1 + "," + y_1 + "," + x + "," + y);
            double x_2 = x_r;
            double y_2 = y_r;
            ScalarMultiplication((ModInversCalculation((int)signature, prime) * (int)x_random)%prime, x_r, y_r, ref x_2, ref y_2,prime);
            WriteLine("Generated Point 2: " + x_2 + "," + y_2 + "," + x_r + "," + y_r);
            AddPointsFiniteField(ref x_1, ref y_1, x_2, y_2,prime);
            WriteLine("Generated Point 3: " + x_1 + "," + y_1 );
        }
          /*  Method to calculate mod inverse. */
        private static int ModInversCalculation(int a, int m)
        {

            int g = Gcd(a, m);
            if (g != 1) { }
                //WriteLine("Inverse doesn't exist");
            else
            {
                // If a and m are relatively
                // prime, then modulo inverse
                // is a^(m-2) mode m
                //WriteLine("Modular multiplicative inverse is "+Power(a, m - 2, m));
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
        private static double Mod(double a, int p)
        {
            double m = a % p;
            return m < 0 ? m + p : m;
        }


    }


























}

