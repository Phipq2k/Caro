using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    //Đối tượng người chơi
    public class PLayers
    {
        private String name;
        private Image mark;

        public string Name { get => name; set => name = value; }
        public Image Mark { get => mark; set => mark = value; }

        public PLayers(string name, Image mark)
        {
            this.name = name;
            this.mark = mark;
        }

    }
}
