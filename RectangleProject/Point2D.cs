using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleProject
{
    class Point2D
    {
        private double x;
        private double y;

        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        #region Methods


        //Khoảng cách giữa 2 điểm
        public double Space2Point(double x1, double y1)
        {
            double space2Point = Math.Sqrt(Math.Pow(x1 - x, 2) + Math.Pow(y1 - y, 2));
            return space2Point;
        }

        //Điểm đối xứng qua OX
        public String OxSymmetry()
        {
            double xSymetryOX, ySymetryOX;
            xSymetryOX = 2 * x - x;
            ySymetryOX = -y;
            return String.Format("Point2D'({0},{1})", xSymetryOX, ySymetryOX);
        }

        //Điểm đối xứng qua OY
        public String OySymmetry()
        {
            double xSymetryOY, ySymetryOY;
            xSymetryOY = -x;
            ySymetryOY = 2 * y - y;
            return String.Format("Point2D'({0},{1})", xSymetryOY, ySymetryOY);
        }

        //Điểm đối xứng qua O
        public String OSymmetry()
        {
            double xSymetryO, ySymetryO;
            xSymetryO = -x;
            ySymetryO = -y;
            return String.Format("Point2D'({0},{1})", xSymetryO, ySymetryO);
        }
        #endregion
    }
}
