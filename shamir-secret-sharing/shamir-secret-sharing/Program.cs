using static System.Console;


namespace shamirsecretsharing
{
    class ShamirSecretSharing
    {
        public static void Main(string[] args)
        {

            if (ReadUserValues(out int modificationMethod, out double secretNumber, out int numberOfCoefficients, out int numberOfShares))
            {

                double[] coefficientsArray = new double[numberOfCoefficients];

                GenerateCoefficients(coefficientsArray, numberOfCoefficients);

                Point[] pointsArray = new Point[numberOfShares];

                if (modificationMethod == 5)
                    GenerateSharesFiniteFields(coefficientsArray, secretNumber, numberOfShares, pointsArray);
                else
                {
                    GenerateShares(secretNumber, coefficientsArray, numberOfShares, pointsArray);

                    if (modificationMethod == 1 || modificationMethod == 2)
                        ModifyShares(pointsArray, modificationMethod, numberOfCoefficients);
                   if (modificationMethod == 3)
                    {
                        AddRandomORK(numberOfShares, numberOfCoefficients, pointsArray);
                        Point[] pickedShares = new Point[numberOfCoefficients + 1];
                        PickRamdonShares(pickedShares, pointsArray, numberOfCoefficients);
                        Decrypt(pickedShares, numberOfCoefficients);
                    }
                    if (modificationMethod == 4)
                        GenerateLagrangeORKs(pointsArray, numberOfCoefficients);
                }
            }else
                Write("Invalid entry - try again >>  ");

        }

        private static bool ReadUserValues(out int modificationMethod, out double secretNumber, out int numberOfCoefficients, out int numberOfShares)
        {
            WriteLine(" 1: Adding a factor to shares.\n 2: Multiplying the shares by a factor.\n 3: Modify shares by random numbers.\n 4: Modify shares with lagrange. \n 5. Finite fileds");
            Write("please enter the number to select share modification method as above :");
            bool validInput=true;

            if (!int.TryParse(ReadLine(), out modificationMethod))
                validInput = false;

            Write("Enter secret number :");
            if (!double.TryParse(ReadLine(), out secretNumber))
                validInput = false;

            Write("Enter the number of coefficients:");
            if (!int.TryParse(ReadLine(), out numberOfCoefficients))
                validInput = false;

            if (modificationMethod == 3)
            {
                Write("Enter the number of shares:");
                if (!int.TryParse(ReadLine(), out numberOfShares))
                    validInput = false;
            }
            else
                numberOfShares = numberOfCoefficients + 1;

            return validInput;
        }
        private static void GenerateCoefficients(double[] coefficientsArray, int numberOfCoefficients)
        {
            int i = 0;
            while (i < numberOfCoefficients)
            {
                // Generate random numbers to assign coefficients within 10.
                Random rnd = new Random();
                coefficientsArray[i] = rnd.Next(10);
                i++;
            };
        }

        private static void GenerateShares(double secretNumber, double[] coefficientsArray, int numberOfShares, Point[] pointsArray)
        {
            int j = 0;
            while (j < numberOfShares)
            {
                // Generate random numbers to assign to x within 10.
                Random rnd = new Random();
                double x = rnd.Next(15);
                // Calculate y
                int t = 0;
                double y = 0;
                if (x != 0 && !Isduplicate(pointsArray, x))
                {
                    while (t < coefficientsArray.Length)
                    {
                        y = y + coefficientsArray[t] * Math.Pow(x, coefficientsArray.Length - t);
                        t++;
                    }
                    y = y + secretNumber;
                    pointsArray[j] = new Point(x, y);
                    WriteLine(pointsArray[j].X + " ," + pointsArray[j].Y);

                    j++;
                }
            }
        }

        /* Method to check for existance of X to avoid duplicates.*/
        private static bool Isduplicate(Point[] pointsArray, double x)
        {
            foreach (Point p in pointsArray)
            {
                if (p != null && x == p.X)
                    return true;
            }
            return false;
        }

        /* Method to modify shares with addition and multipication. */
        private static void ModifyShares(Point[] pointsArray, int modificationMethod,int numberOfCoefficients)
        {
            
            Write("Enter modification factor:");
            if (int.TryParse(ReadLine(), out int modificationFact))
            {
                foreach (Point p in pointsArray)
                {
                    if (modificationMethod == 1)
                        p.Y = p.Y + modificationFact;
                    else if (modificationMethod == 2)
                        p.Y = p.Y * modificationFact;
                }
                WriteLine(pointsArray[0].X + " ," + pointsArray[0].Y);
                Decrypt(pointsArray, numberOfCoefficients);
            }
            else
                Write("Invalid modification factor entered !");
          
            
        }

        /* Method to genarete random ORKs based on the same number of coefficients*/
        private static void AddRandomORK(int numberOfShares, int numberOfCoefficients, Point[] pointsArray)
        {

            double[] randomCoefficientsArray = new double[numberOfCoefficients];
            // Generate coefficients for random graph.
            GenerateCoefficients(randomCoefficientsArray, numberOfCoefficients);

            Write("Enter modification factor:");
            if (int.TryParse(ReadLine(), out int modificationFact))
                GenerateRs(randomCoefficientsArray, numberOfShares, pointsArray, modificationFact);
            else
                Write("Invalid modification factor entered !");
        }

        /* Method to generete R values for the already generated Xs.*/
        private static void GenerateRs(double[] randomCoefficientsArray, double numberOfShares, Point[] pointsArray, int modificationFact)
        {
            int j = 0;
            while (j < numberOfShares)
            {

                double x = pointsArray[j].X;

                int t = 0;
                double r = 0;
                while (t < randomCoefficientsArray.Length)
                {
                    r = r + randomCoefficientsArray[t] * Math.Pow(x, randomCoefficientsArray.Length - t);
                    t++;
                }
                r = r + modificationFact;

                pointsArray[j].Y = pointsArray[j].Y + r;
                WriteLine(pointsArray[j].X + " ," + pointsArray[j].Y);

                j++;

            }
        }

        private static void GenerateLagrangeORKs(Point[] pointsArray, int numberOfCoefficients)
        {
            int prime = 1613;
            Write("Enter modification factor:");
            if (int.TryParse(ReadLine(), out int modificationFact))
            {
                for (int i = 0; i <= numberOfCoefficients; i++)
                {
                    double xm = 1;
                    for (int j = 0; j <= numberOfCoefficients; j++)
                    {

                        if (i != j)
                        {
                            xm = xm * (pointsArray[j].X / (pointsArray[j].X - pointsArray[i].X));
                        }

                    }
                    pointsArray[i].Y = Math.Pow(modificationFact, (pointsArray[i].Y * xm)%prime );
                    WriteLine("The modified share : " + pointsArray[i].Y);
                }
                DecryptLagrangeORKs(pointsArray, numberOfCoefficients, modificationFact);
            }
            else
                Write("Invalid modification factor entered !");
        }

        private static void DecryptLagrangeORKs(Point[] points, int numberOfCoefficients, double modificationFact)
        {
            double multipliedShares = 1;
            for (int i = 0; i <= numberOfCoefficients; i++)
            {
                multipliedShares = multipliedShares * points[i].Y;
            }

            WriteLine("The modified share : " + multipliedShares);

            double secret = Math.Log(multipliedShares) / Math.Log(modificationFact);
            WriteLine("print secret : " + secret);

        }

        /* Method to pick the minimum required number of shares randomlly.*/
        private static void PickRamdonShares(Point[] pickedShares, Point[] points, int numberOfCoefficients)
        {
            int i = 0;
            while (i <= numberOfCoefficients)
            {
                Random rnd = new Random();
                int index = rnd.Next(points.Length);
                if (!Isduplicate(pickedShares, points[index].X))
                {
                    pickedShares[i] = points[index];
                    WriteLine("Picked shares: " + pickedShares[i].X + " ," + pickedShares[i].Y);
                    i++;
                }
            }

        }

        private static void Decrypt(Point[] points, int numberOfCoefficients)
        {
            double y = 0;
            for (int i = 0; i <= numberOfCoefficients; i++)
            {
                double xm = 1;
                for (int j = 0; j <= numberOfCoefficients; j++)
                {

                    if (i != j)
                    {
                        xm = xm * (points[j].X / (points[j].X - points[i].X));
                    }

                }
                y = y + points[i].Y * xm;
            }
            WriteLine("The modified share : " + y);

        }


        /* Method to generate shares with finite fileds*/
        private static void GenerateSharesFiniteFields(double[] coefficientsArray, double secretNumber, int numberOfShares, Point[] pointsArray)
        {
            Write("Enter the value of prime:");
            if (int.TryParse(ReadLine(), out int prime))
            {
                for (int x = 1; x <= numberOfShares; x++)
                {
                    double temp = secretNumber;
                    for (int j = 1; j < coefficientsArray.Length + 1; j++)
                    {
                        temp = (temp + (coefficientsArray[j - 1] * (Math.Pow(x, j) % prime)) % prime) % prime;

                    }

                    pointsArray[x - 1] = new Point(x, temp);
                    WriteLine("The modified share : " + pointsArray[x - 1].X + "," + pointsArray[x - 1].Y);

                }
                SecretReconstruct(prime, coefficientsArray.Length, pointsArray);
            }
            else
                Write("Invalid prime number entered !");
        }

        public static void SecretReconstruct(int prime, int numberOfCoefficients, Point[] points)
        {
            double y = 0;
            double s = 0;
            for (int i = 0; i <= numberOfCoefficients; i++)
            {
                double xn = 1;
                double xd = 1;
                for (int j = 0; j <= numberOfCoefficients; j++)
                {

                    if (i != j)
                    {
                        xn = (xn * -points[j].X) % prime;
                        xd = (xd * (-points[j].X + points[i].X)) % prime;
                    }

                }
                // WriteLine("print secret temp x: " + xn + "," + xd);
                y = points[i].Y * xn * ModInversCalculation((int)xd, prime);
                // y = points[i].Y * xn / xd;
                //WriteLine("print secret temp: " + (prime + s + y) + "," + y);
                s = (s + y) % prime;
            }
            WriteLine("print secret : " + s);
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
                Console.WriteLine(
                    "Modular multiplicative inverse is "+return Power(a, m - 2, m)
                    );
            }
            return Power(a, m - 2, m);
        }

        // To compute x^y under
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

    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }


}
