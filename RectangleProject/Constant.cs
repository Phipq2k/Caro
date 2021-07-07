using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleProject
{
    class Constant
    {
        private int number;

        public Constant(int number)
        {
            this.Number = number;
        }

        public int Number { get => number; set => number = value; }

        public String Type() 
        {
            String type = "";
            switch (Number)
            {
                case 0:
                    type ="Tam giac tu";
                    break;
                case 1:
                    type = "Tam giac nhon";
                    break;
                case 2:
                    type = "Tam giac can";
                    break;
                case 3:
                    type = "Tam giac vuong";
                    break;
                case 4:
                    type = "Tam giac deu";
                    break;
                case 5:
                    type = "Tam giac vuong can";
                    break;
                default:
                    type = "Khong phai tam giac";
                    break;

            }
            return type;
        }
    }
}
