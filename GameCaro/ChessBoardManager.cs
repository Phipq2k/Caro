using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    //Custom đối tượng bàn cờ
    public class ChessBoardManager
    {
        #region Properties
        private Panel ChessBoard;
        public Panel ChessBoard1 { get => ChessBoard; set => ChessBoard = value; }

        private List<PLayers> pLayer;
        public List<PLayers> PLayer { get => pLayer; set => pLayer = value; }

        private int currentPlayer;
        public int CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }

        private TextBox playerName;
        public TextBox PlayerName { get => playerName; set => playerName = value; }

        private PictureBox mark;
        public PictureBox Mark { get => mark; set => mark = value; }

        //Ma trận chứa button khi được đánh dấu
        private List<List<Button>> matrix;
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }

        //Thuộcsự kiện xử lý khi players đánh cờ tác động đến progressbar
        private event EventHandler<BtnClickEvent> playerMarked;
        public event EventHandler<BtnClickEvent> PlayerMarked
        {
            add
            {
                playerMarked += value;
            }
            remove
            {
                playerMarked -= value;
            }
        }
        //Thuộcsự kiện xử lý khi trò chơi kết thúc của hiệu ứng progressbar
        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }


        private Stack<PlayInfo> playTimeLine;

        internal Stack<PlayInfo> PlayTimeLine { get => playTimeLine; set => playTimeLine = value; }

        private SoundPlayer soundPlayer;

        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.Mark = mark;
            this.PLayer = new List<PLayers>()
            {
                new PLayers("Player1", Image.FromFile(Application.StartupPath + "\\Resources\\X1.png")),
                new PLayers("Player2", Image.FromFile(Application.StartupPath + "\\Resources\\O1.png"))
            };
            
           
        }
        #endregion

        #region Methods

        #region Tạo bàn cờ
        public void DrawChessBoard()
        {
            ChessBoard.Enabled = true;
            ChessBoard.Controls.Clear();

            PlayTimeLine = new Stack<PlayInfo>();

            CurrentPlayer = 0;
            ChangePlayer();

            matrix = new List<List<Button>>();

            //Tạo ô để set up vị trí cho các ô kế tiếp 
            Button oldcell = new Button() { Width = 0, Location = new Point(0, 0) };
            //Vòng lặp render ô trong bàn cờ
            for (int i = 0; i < Constant.CHESS_BOARD_HEIGHT; i++)
            {
                /*Tạo ma trận bàn cờ*/
                //Tạo một list sau mỗi vòng lặp render hàng
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Constant.CHESS_BOARD_WIDTH; j++)
                {
                    Button cell = new Button()
                    {
                        Width = Constant.CHESS_WIDTH,
                        Height = Constant.CHESS_HEIGHT,
                        BackColor = Color.White,
                        Location = new Point(oldcell.Location.X + oldcell.Width, oldcell.Location.Y), //Sử dụng thuộc tính Location để set up vị trí cho ô mới mỗi khi được render
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    //Sự kiến nhấn vào ô thay đổi thông tin người chơi
                    cell.Click += Cell_Click;

                    //Add button đã render vào trong bàn cờ
                    ChessBoard.Controls.Add(cell);
                    //Add phần tử cell vào list
                    Matrix[i].Add(cell);
                    oldcell = cell;
                }

                //Reset lại vị trí render ô trong bàn cờ
                oldcell.Location = new Point(0, oldcell.Location.Y + Constant.CHESS_HEIGHT);
                oldcell.Width = 0;
                oldcell.Height = 0;

            }
            #endregion

        }
        //Hàm xử lý sự kiện click cell
        private void Cell_Click(object sender, EventArgs e)
        {
            Button cell = sender as Button;
            //Xử lý bug có thể thay đổi ô đánh rồi
            if (cell.BackgroundImage != null)
                return;

            //Âm thanh khi click cell
            soundPlayer = new SoundPlayer(Application.StartupPath + "\\Resources\\effectgamecaro.wav");
            soundPlayer.Play();

            MarkPlayer(cell);

            //Lưu trữ tọa độ của ô vào stack
           PlayTimeLine.Push(new PlayInfo(getChessPoint(cell), CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();

            //Xử lý sự kiện người dùng click cell tác động đến progressbar
            if(playerMarked != null)
            {
                playerMarked(this, new BtnClickEvent(getChessPoint(cell)));
            }

            //Điều kiện thắng thua
            if (isEndgame(cell))
            {
                endGame();
            }

        }

        public void OtherPlayerMark(Point point)
        {

            Button cell = Matrix[point.Y][point.X];

            //Xử lý bug có thể thay đổi ô đánh rồi
            if (cell.BackgroundImage != null)
                return;

            //Âm thanh khi click cell
            soundPlayer = new SoundPlayer(Application.StartupPath + "\\Resources\\effectgamecaro.wav");
            soundPlayer.Play();

            MarkPlayer(cell);

            //Lưu trữ tọa độ của ô vào stack
            PlayTimeLine.Push(new PlayInfo(getChessPoint(cell), CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();

            //Điều kiện thắng thua
            if (isEndgame(cell))
            {
                endGame();
                PlayerName.Text = PlayerName.Text == "Player1" ? "Player1" : "Player2";
            }
        }

        //Xử lý sự kiện thắng thua
        public void endGame()
        {
            if(endedGame != null)
            {
                endedGame(this, new EventArgs());
            }
        }

        public bool Undo()
        {
            if (PlayTimeLine.Count <= 0)
            {
                return false;
            }
            bool isUndo1 = UndoAStep();
            bool isUndo2 = UndoAStep();
            PlayInfo oldpoint = PlayTimeLine.Peek();
            CurrentPlayer = oldpoint.CurrentPlayer == 1 ? 0 : 1;
            return isUndo1 && isUndo2;

        }

        private bool UndoAStep()
        {

            PlayInfo oldpoint = PlayTimeLine.Pop();
            Button cell = Matrix[oldpoint.Point.Y][oldpoint.Point.X];

            cell.BackgroundImage = null;



            if (PlayTimeLine.Count <= 0)
            {
                CurrentPlayer = 0;
            }
            else
            {
                oldpoint = PlayTimeLine.Peek();
                
            }

            ChangePlayer();
            return true;
        }

        //Xác định thắng thua
        private bool isEndgame(Button cell)
        {
            return isEndHorizontal(cell) || isEndVertical(cell) || isEndPrimaryDiagonal(cell) || isEndSubDiagonal(cell);
        }

        //Lấy tọa độ của cell
        private Point getChessPoint(Button cell)
        {
            int vertical = Convert.ToInt32(cell.Tag);
            int horizontal = Matrix[vertical].IndexOf(cell);

            Point point = new Point(horizontal, vertical);

            return point;
        }

        //Thắng theo chiều ngang
        private bool isEndHorizontal(Button cell)
        {
            Point point = getChessPoint(cell);
            int countLeft = 0;
            //Xét từ vị trí cell về bên trái trong 1 hàng
            for (int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == cell.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }
            int countRight = 0;
            //Xét từ vị trí cell về bên phải trong 1 hàng
            for (int i = point.X + 1; i < Constant.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == cell.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }


            return countLeft + countRight >= 5;
        }

        //Thắng theo chiều dọc
        private bool isEndVertical(Button cell)
        {
            Point point = getChessPoint(cell);
            int countTop = 0;
            //Xét từ vị trí cell về bên trên trong 1 cột
            for(int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == cell.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;
            //Xét từ vị trí cell về bên dưới trong 1 cột
            for (int i = point.Y+1; i < Constant.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == cell.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }


            return countTop + countBottom >= 5;
        }

        //Thắng theo đường chéo chính
        private bool isEndPrimaryDiagonal(Button cell)
        {
            Point point = getChessPoint(cell);
            int countLeftTop = 0;
            //Xét từ vị trí cell về bên trái theo đường chéo
            for (int i = 0; i <= point.X; i++)
            {
                //Xử lý bug khi đánh cờ bị ra khỏi bàn cờ theo chiều bên trái và bên trên
                if(point.X - i <0 || point.Y - i < 0)
                {
                    break;
                }
                if (Matrix[point.Y - i][point.X - i].BackgroundImage == cell.BackgroundImage)
                {
                    countLeftTop++;
                }
                else
                    break;
            }
            int countRightBottom = 0;
            //Xét từ vị trí cell về bên phải trong 1 hàng
            for (int i = 1; i <= Constant.CHESS_BOARD_WIDTH - point.X; i++)
            {
                //Xử lý bug khi đánh cờ bị ra khỏi bàn cờ theo chiều bên phải và bên dưới
                if (point.Y + i >= Constant.CHESS_BOARD_HEIGHT || point.X + i >= Constant.CHESS_BOARD_WIDTH)
                {
                    break;
                }
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == cell.BackgroundImage)
                {
                    countRightBottom++;
                }
                else
                    break;
            }
            return countLeftTop + countRightBottom >= 5;
        }

        //Thắng theo đường chéo phụ
        private bool isEndSubDiagonal(Button cell)
        {
            Point point = getChessPoint(cell);
            int countLeftBottom = 0;
           
            for (int i = 0; i <= point.X; i++)
            {
                //Xử lý bug khi đánh cờ bị ra khỏi bàn cờ theo chiều bên trái và bên duới
                if (point.X - i < 0 || point.Y + i >= Constant.CHESS_BOARD_HEIGHT)
                {
                    break;
                }
                //Xét từ vị trí cell theo chiều dưới trái
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == cell.BackgroundImage)
                {
                    countLeftBottom++;
                }
                else
                    break;
            }
            int countRightTop = 0;
           
            for (int i = 1; i <= Constant.CHESS_BOARD_WIDTH - point.X; i++)
            {
                //Xử lý bug khi đánh cờ bị ra khỏi bàn cờ theo chiều bên phải và bên trên
                if (point.X + i >= Constant.CHESS_BOARD_WIDTH || point.Y - i < 0)
                {
                    break;
                }
                //Xét từ vị trí cell theo chiều trên phải
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == cell.BackgroundImage)
                {
                    countRightTop++;
                }
                else
                    break;
            }
            return countLeftBottom + countRightTop >= 5;
        }
        //Thay đổi hiệu ứng trong bàn cờ
        private void MarkPlayer(Button cell)
        {
            cell.BackgroundImage = PLayer[currentPlayer].Mark;
        }

        //Thay đổi người chơi
        private void ChangePlayer()
        {
            PlayerName.Text = PLayer[currentPlayer].Name;
            Mark.Image = PLayer[currentPlayer].Mark;
        }
       
        #endregion
    }

    //Class sự kiện nhấn cell truyền tọa độ qua socket
    public class BtnClickEvent : EventArgs
    {
        private Point clickedPoint;

        public BtnClickEvent(Point clickedPoint)
        {
            this.clickedPoint = clickedPoint;
        }

        public Point ClickedPoint { get => clickedPoint; set => clickedPoint = value; }

    }
}
