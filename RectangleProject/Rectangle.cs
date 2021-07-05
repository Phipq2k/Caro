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

        public Rectangle(Point2D point1, Point2D point2, Point2D point3)
        {
            this.Point1 = point1;
            this.Point2 = point2;
            this.Point3 = point3;
        }

        #region Methods
        public bool isRectangle()
        {
            double space1 = point1.Space2Point(point2.X, point2.Y);
            double space2 = point2.Space2Point(point3.X, point3.Y);
            double space3 = point1.Space2Point(point3.X, point3.Y);
            return space1 + space2 > space3 && space2 + space3 > space1 && space1 + space3 > space2;
        }

        public double Perimeter()
        {
            double space1 = point1.Space2Point(point2.X, point2.Y);
            double space2 = point2.Space2Point(point3.X, point3.Y);
            double space3 = point1.Space2Point(point3.X, point3.Y);
            return space1 + space2 + space3;
        }

        public double Acreage()
        {
            return Math.Abs((point2.X - point1.X) * (point3.Y - point1.Y) - (point3.X - point1.X) * (point2.Y - point1.Y)) / 2;
        }

        public String checkRectangleType()
        {
            String result;
            double space1 = point1.Space2Point(point2.X, point2.Y);
            double space2 = point2.Space2Point(point3.X, point3.Y);
            double space3 = point1.Space2Point(point3.X, point3.Y);
            if (isRectangle())
            {
                if (space1 == space2 || space2 == space3 || space3 == space1)
                {
                    result = "Tam giac can";
                }
                else
                {
                    if (Math.Sqrt(Math.Pow(space1, 2) + Math.Pow(space2, 2)) == space3 || Math.Sqrt(Math.Pow(space2, 2) + Math.Pow(space3, 2)) == space1 || Math.Sqrt(Math.Pow(space1, 2) + Math.Pow(space3, 2)) == space2)
                    {
                        result = "Tam giac vuong";
                    }
                    else
                    {
                        if (Math.Sqrt(Math.Pow(space1, 2) + Math.Pow(space2, 2)) < space3 || Math.Sqrt(Math.Pow(space2, 2) + Math.Pow(space3, 2)) < space1 || Math.Sqrt(Math.Pow(space1, 2) + Math.Pow(space3, 2)) < space2)
                        {
                            result = "Tam giac tu";
                        }
                        else
                        {
                            result = "Tam giac nhon";
                        }
                    }                
                } 
            }
            else
            {
                result = "Khong phai tam giac";
            }
                         
            return result;
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
