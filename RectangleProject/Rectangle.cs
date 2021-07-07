using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleProject
{
    class Rectangle
    {
        private Point2D point1;
        private Point2D point2;
        private Point2D point3;

        internal Point2D Point1 { get => point1; set => point1 = value; }
        internal Point2D Point2 { get => point2; set => point2 = value; }
        internal Point2D Point3 { get => point3; set => point3 = value; }

        public Rectangle()
        {

        }

        public Rectangle(Point2D point1, Point2D point2, Point2D point3)
        {
            this.Point1 = point1;
            this.Point2 = point2;
            this.Point3 = point3;
        }

        public double Space1{ get => point1.Space2Point(point2); }
        public double Space2 { get => point2.Space2Point(point3); }
        public double Space3 { get => point1.Space2Point(point3); }
        

        #region Methods
        public bool isRectangle()
        {
            return Space1 + Space2 > Space3 && Space2 + Space3 > Space1 && Space1 + Space3 > Space2;
        }

        public double Perimeter()
        {
            return Space1 + Space2 + Space3;
        }

        public double Acreage()
        {
            double p = Perimeter() / 2;
            return Math.Sqrt(p * (p - Space1) * (p - Space2) * (p - Space3));
        }

        /// <summary>
        /// Có 6 loại tam giác:
        /// 0 = Tam giác tu
        /// 1 = Tam giác nhon
        /// 2 = Tam giác can
        /// 3 = Tam giác vuong
        /// 4 = Tam giác deu
        /// 5 = Tam giác vuong can
        /// </summary>
        /// <returns></returns>
        public int checkRectangleType()
        {
            int type = 0;
            if (isRectangle())
            { 
                if (Math.Sqrt(Math.Pow(Space1, 2) + Math.Pow(Space2, 2)) < Space3 || Math.Sqrt(Math.Pow(Space2, 2) + Math.Pow(Space3, 2)) < Space1 || Math.Sqrt(Math.Pow(Space1, 2) + Math.Pow(Space3, 2)) < Space2)
                {
                    type = 0;
                }
                else if (Math.Sqrt(Math.Pow(Space1, 2) + Math.Pow(Space2, 2)) > Space3 && Math.Sqrt(Math.Pow(Space2, 2) + Math.Pow(Space3, 2)) > Space1 && Math.Sqrt(Math.Pow(Space1, 2) + Math.Pow(Space3, 2)) > Space2)
                {
                    type = 1;
                }

                else if (Space1 == Space2 || Space2 == Space3 || Space1 == Space3)
                {
                    type = 2;
                }
                else if (Math.Sqrt(Math.Pow(Space1, 2) + Math.Pow(Space2, 2)) == Space3 || Math.Sqrt(Math.Pow(Space2, 2) + Math.Pow(Space3, 2)) == Space1 || Math.Sqrt(Math.Pow(Space1, 2) + Math.Pow(Space3, 2)) == Space2)
                {
                    type = 3;
                }

                else if (Space1 == Space2 && Space1 == Space3)
                {
                    type = 4;
                }
                else
                {
                    type = 5;
                }
            }
            else
            {
                type = -1;
            }
            return type;
        }


        public void OxSRectangle()
        {
           //Gọi 3 đối tượng tính OxSymetry
        }

        public void OySRectangle()
        {
            //Gọi 3 đối tượng tính OySymetry
        }

        public void OSRectangle()
        {
            //Gọi 3 đối tượng tính OSymetry
        }

        #endregion
    }
}
