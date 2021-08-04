using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    //Class chứa các thuộc tính hằng số hỗ trợ cho việc thiết lập số liệu trong game
    public class Constant
    {
        //Kích thước của mỗi ô trong bàn cờ
        public static int CHESS_WIDTH = 30;
        public static int CHESS_HEIGHT = 30;

        //Kích thước của bàn cờ
        public static int CHESS_BOARD_WIDTH = 26;
        public static int CHESS_BOARD_HEIGHT = 25;

        //Mức step trong đơn vị thời gian của progressbar
        public static int COOL_DOWN_STEP = 100;
        //Thời gian đếm ngược trong game
        public static int COOL_DOWN_TIME = 30000;
        //Thời gian step trong progessbar
        public static int COOL_DOWN_INTERVAL = 100;
        //Giới hạn số lần đi lại
        public static int UNDOLIMITED = 3;
        //Điểm player
        public static int player1Score = 0;
        public static int player2Score = 0;

        //Giới hạn trận game
        public static int BATTLE_LIMITED = 2;



    }
}
