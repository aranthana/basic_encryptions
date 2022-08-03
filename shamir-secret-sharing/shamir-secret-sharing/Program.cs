using static System.Console;
using System;

namespace shamirsecretsharing
{
    class ShamirSecretSharing
    {
        public static void Main(string[] args)
        {


            ReadUservalues(out int modificationMethod,out double secretNumber, out int numberOfCoefficients, out int numberOfShares);

            double[] coefficientsArray = new double[numberOfCoefficients];
            
            GenerateCoefficients(coefficientsArray, numberOfCoefficients);

            Point[] pointsArray = new Point[numberOfShares];

            GenerateShares(secretNumber, coefficientsArray, numberOfShares, pointsArray);
            if (modificationMethod == 1 || modificationMethod ==2)
            {
                ModifyShares(pointsArray, modificationMethod);
                Decrypt(pointsArray, numberOfCoefficients);
            }
                if (modificationMethod == 3)
            {
                AddRandomORK(numberOfShares, numberOfCoefficients, pointsArray);
                Point[] pickedShares = new Point[numberOfCoefficients + 1];
                PickRamdonShares(pickedShares,pointsArray, numberOfCoefficients);
                Decrypt(pickedShares, numberOfCoefficients);
            }
            if (modificationMethod==4)
                GenerateLagrangeORKs(pointsArray, numberOfCoefficients);

           

        }

        private static void ReadUservalues(out int modificationMethod,out double secretNumber, out int numberOfCoefficients, out int numberOfShares)
        {
           WriteLine(" 1: Adding a factor to shares.\n 2: Multiplying the shares by a factor.\n 3: Modify shares by random numbers.\n 4: Modify shares with lagrange.");
            Write("please enter the number to select share modification method as above :");
            modificationMethod = int.Parse(ReadLine());
            
            Write("Enter secret number :");
            secretNumber = double.Parse(ReadLine());

            Write("Enter the number of coefficients:");
            numberOfCoefficients = int.Parse(ReadLine());

            if (modificationMethod == 3)
            {
                Write("Enter the number of shares:");
                numberOfShares = int.Parse(ReadLine());
            }
            else
                numberOfShares = numberOfCoefficients + 1;
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

        private static void GenerateShares(double secretNumber, double [] coefficientsArray, int numberOfShares, Point[] pointsArray)
        {
            int j = 0;
            while (j < numberOfShares)
            {
                // Generate random numbers to assign to x within 10.
                Random rnd = new Random();
                double x = rnd.Next(10);
                // Calculate y
                int t = 0;
                double y = 0;
                if (x != 0 && !Isduplicate(pointsArray,x))
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

        // Check for existance of X to avoid duplicates.
        private static bool Isduplicate(Point[] pointsArray, double x)
        {
            foreach (Point p in pointsArray)
            {
                if (p != null && x == p.X)
                    return true;
            }
            return false;
        }
        // Modify shares with addition and multipication
        private static void ModifyShares(Point[] pointsArray, int modificationMethod)
        {
            Write("Enter modification factor:");
            int modification_fact = int.Parse(ReadLine());
            foreach (Point p in pointsArray)
            {
                if(modificationMethod ==1)
                    p.Y = p.Y + modification_fact;
                else if (modificationMethod ==2)
                    p.Y = p.Y * modification_fact;
            }
            WriteLine(pointsArray[0].X + " ," + pointsArray[0].Y);
        }

        // Genarete random ORKs based on the same number of coefficients
        private static void AddRandomORK(int numberOfShares, int numberOfCoefficients, Point[] pointsArray)
        {
            
            double[] randomCoefficientsArray = new double[numberOfCoefficients];
            // Generate coefficients for random graph.
            GenerateCoefficients(randomCoefficientsArray, numberOfCoefficients);

            Write("Enter modification factor:");
            int modificationFact = int.Parse(ReadLine());

            GenerateRs(randomCoefficientsArray, numberOfShares, pointsArray, modificationFact);
            
        }
        // Generete R values for the already generated Xs.
        private static void GenerateRs(double[] randomCoefficientsArray, double numberOfShares, Point[] pointsArray,int modificationFact)
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

               // pointsArray[j].R = r;
                pointsArray[j].Y = pointsArray[j].Y + r;
                WriteLine(pointsArray[j].X + " ," + pointsArray[j].Y );

                j++;

            }
        }

        private static void GenerateLagrangeORKs(Point[] pointsArray, int numberOfCoefficients)
        {
            Write("Enter modification factor:");
            double modificationFact = double.Parse(ReadLine());
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
                pointsArray[i].Y = Math.Pow(modificationFact, pointsArray[i].Y * xm);
                WriteLine("The modified share : " + pointsArray[i].Y);
            }
            DecryptLagrangeORKs(pointsArray, numberOfCoefficients, modificationFact);
        }

        private static void DecryptLagrangeORKs(Point[] points, int numberOfCoefficients,double modificationFact)
        {
            double multipliedShares = 1;
            for (int i = 0; i <= numberOfCoefficients; i++)
            {
                multipliedShares = multipliedShares * points[i].Y;
            }
              
            WriteLine("The modified share : " + multipliedShares);

            //double secret = Math.Pow(multipliedShares, (1.0 / modificationFact));
            double secret = Math.Log(multipliedShares) / Math.Log(modificationFact);
            WriteLine("print secret : " + secret);

        }

        // Pick the minimum required number of shares randomlly
        private static void PickRamdonShares(Point[] pickedShares,Point[] points,int numberOfCoefficients)
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

                    if (i != j) {
                        xm = xm * (points[j].X / (points[j].X - points[i].X));
                    }

                }
                y = y + points[i].Y * xm;
            }
            WriteLine("The modified share : " + y);

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
