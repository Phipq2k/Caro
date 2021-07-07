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

        public Point2D() {
        
        }

        public Point2D(double x = 0, double y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        #region Methods


        //Khoảng cách giữa 2 điểm
        public double Space2Point(Point2D b)
        {
            double space2Point = Math.Sqrt(Math.Pow(b.X - x, 2) + Math.Pow(b.Y - y, 2));
            return space2Point;
        }

        //Điểm đối xứng qua OX
        public Point2D OxSymmetry()
        {
            double xSymetryOX, ySymetryOX;
            xSymetryOX = x;
            ySymetryOX = -y;
            return new Point2D(xSymetryOX, ySymetryOX);
        }

        //Điểm đối xứng qua OY
        public Point2D OySymmetry()
        {
            double xSymetryOY, ySymetryOY;
            xSymetryOY = -x;
            ySymetryOY = y;
            return new Point2D(xSymetryOY, ySymetryOY);
        }

        //Điểm đối xứng qua O
        public Point2D OSymmetry()
        {
            double xSymetryO, ySymetryO;
            xSymetryO = -x;
            ySymetryO = -y;
            return new Point2D(xSymetryO, ySymetryO);
        }
        #endregion
    }
}
