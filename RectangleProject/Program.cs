using System;

namespace RectangleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Input
            Console.WriteLine("Nhap xA(cm): ");
            double xA = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap yA(cm): ");
            double yA = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap xB(cm): ");
            double xB = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap yB(cm): ");
            double yB = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap xC(cm): ");
            double xC = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Nhap yC(cm): ");
            double yC = Convert.ToDouble(Console.ReadLine());
            #endregion

            #region Calculator
            Point2D A = new Point2D(xA, yA);
            Point2D B = new Point2D(xB, yB);
            Point2D C = new Point2D(xC, yC);


            double AB = A.Space2Point(B);
            double BC = B.Space2Point(C);
            double AC = A.Space2Point(C);

            Rectangle rectangle = new Rectangle(A,B,C);

            Constant constant = new Constant(rectangle.checkRectangleType());


            #endregion

            #region Output

            if (rectangle.isRectangle())
                Console.WriteLine("True");
            else
                Console.WriteLine("False");
            /*Console.WriteLine(String.Format("Diem doi xung qua Ox cua A: {0}", A.OxSymmetry()));
            Console.WriteLine(String.Format("Diem doi xung qua Ox cua A: {0}", A.OySymmetry()));
            Console.WriteLine(String.Format("Diem doi xung qua Ox cua A: {0}", A.OSymmetry()));*/


            Console.WriteLine(String.Format("AB: {0}cm \nBC: {1}cm \nAC: {2}cm", Math.Round(AB,2), Math.Round(BC,2), Math.Round(AC,2)));

           /* Console.WriteLine(rectangle.checkRectangleType());*/
            Console.WriteLine(constant.Type());

            Console.WriteLine(String.Format("Chu vi: {0}cm, Dien tich: {1}cm2", Math.Round(rectangle.Perimeter(),2), Math.Round(rectangle.Acreage(),2)));

            #endregion
        }
    }
}
