using System;

namespace RectangleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Input
            Console.WriteLine("Nhap xA: ");
            double xA = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap yA: ");
            double yA = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap xB: ");
            double xB = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap yB: ");
            double yB = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap xC: ");
            double xC = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap yC: ");
            double yC = Convert.ToDouble(Console.ReadLine());
            #endregion

            #region Calculator
            Point2D A = new Point2D(xA, yA);
            Point2D B = new Point2D(xB, yB);
            Point2D C = new Point2D(xC, yC);


            double AB = A.Space2Point(B.X, B.Y);
            double BC = B.Space2Point(C.X, C.Y);
            double AC = A.Space2Point(C.X, C.Y);

            Rectangle rectangle = new Rectangle(A,B,C);


            #endregion

            #region Output

            if (rectangle.isRectangle())
                Console.WriteLine("True");
            else
                Console.WriteLine("False");
            /*Console.WriteLine(String.Format("Diem doi xung qua Ox cua A: {0}", A.OxSymmetry()));
            Console.WriteLine(String.Format("Diem doi xung qua Ox cua A: {0}", A.OySymmetry()));
            Console.WriteLine(String.Format("Diem doi xung qua Ox cua A: {0}", A.OSymmetry()));*/
            Console.WriteLine(Math.Round(AB,1) + "," + Math.Round(BC,1) + "," + Math.Round(AC,1));

            Console.WriteLine(rectangle.checkRectangleType());

            #endregion
        }
    }
}
