using static System.Console;
using System;

namespace shamirsecretsharing
{
    class ShamirSecretSharing
    {
        public static void Main(string[] args)
        {


            ReadUservalues(out double secretNumber, out int numberOfCoefficients, out int numberOfShares);

            double[] coefficientsArray = new double[numberOfCoefficients];
            
            GenerateCoefficients(coefficientsArray, numberOfCoefficients);

            Point[] pointsArray = new Point[numberOfShares];

            GenerateShares(secretNumber, coefficientsArray, numberOfShares, pointsArray);

            // ModifyShares(pointsArray);

            AddRandomORK(numberOfShares, numberOfCoefficients, pointsArray);
            Point[] pickedShares = new Point[numberOfCoefficients + 1];
            PickRamdonShares(pickedShares,pointsArray, numberOfCoefficients);
            Decrypt(pickedShares, numberOfCoefficients);

        }

        private static void ReadUservalues(out double secretNumber, out int numberOfCoefficients, out int numberOfShares)
        {
            Write("Enter secret number :");
            secretNumber = double.Parse(ReadLine());

            Write("Enter the number of coefficients:");
            numberOfCoefficients = int.Parse(ReadLine());

            Write("Enter the number of shares:");
            numberOfShares = int.Parse(ReadLine());
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

        private static void ModifyShares(Point[] pointsArray)
        {
            Write("Enter modification factor:");
            int modification_fact = int.Parse(ReadLine());
            foreach (Point p in pointsArray)
                p.Y = p.Y + modification_fact;
            WriteLine(pointsArray[0].X + " ," + pointsArray[0].Y);
        }


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

                pointsArray[j].R = r;
                pointsArray[j].Y = pointsArray[j].Y + r;
                WriteLine(pointsArray[j].X + " ," + pointsArray[j].Y + " ," + pointsArray[j].R);

                j++;

            }
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
                    WriteLine("Picked shares: " + pickedShares[i].X + " ," + pickedShares[i].Y + " ," + pickedShares[i].R);
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
            WriteLine("print secret : " + y);

        }
    }

    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double R { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }


}
