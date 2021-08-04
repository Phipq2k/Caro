using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class KingofSkyGame : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;

        SocketManager socket;

        private int countUndo = 0;

        #endregion

        #region Events Handler Methods

        public KingofSkyGame()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;


            //Gọi đối tượng bàn cờ
            ChessBoard = new ChessBoardManager(pnlChessBoard, txbLayerName, pcbMark);

            //Gọi sự kiện xử lý thanh progressbar
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            //Khai báo các thuộc tính trong progressbar
            prbCoolDown.Step = Constant.COOL_DOWN_STEP;
            prbCoolDown.Maximum = Constant.COOL_DOWN_TIME;
            prbCoolDown.Value = 0;


            //Điểm số
            txtScore1.Text = Constant.player1Score.ToString();
            txtScore2.Text = Constant.player2Score.ToString();


            tmCoolDown.Interval = Constant.COOL_DOWN_INTERVAL;

            socket = new SocketManager();

            if (btnLAN.Enabled == true)
            {
                menuToolStripMenuItem.Enabled = false;
            }

        }


        void EndGame()
        {
            int score1 = Constant.player1Score;
            int score2 = Constant.player2Score;
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            claimADrawToolStripMenuItem.Enabled = false;
            //txbLayerName.Text = txbLayerName.Text == "Player1" ? "Player2" : "Player1";
            if (txbLayerName.Text == "Player1" && score2 <= Constant.BATTLE_LIMITED + 1)
            {
                newGameToolStripMenuItem.Enabled = true;
                txbLayerName.Text = "Player2";
                score2 = score2 += 1;
                txtScore2.Text = score2.ToString();
                socket.Send(new SocketData((int)SocketCommand.PLAYER_2_WIN, "", new Point()));

            }
            else if (txbLayerName.Text == "Player1" && score1 <= Constant.BATTLE_LIMITED + 1)
            {
                newGameToolStripMenuItem.Enabled = false;
                txbLayerName.Text = "Player1";
                score1 = score1 += 1;
                txtScore1.Text = score1.ToString();
                socket.Send(new SocketData((int)SocketCommand.PLAYER_1_WIN, "", new Point()));

            }
            MessageBox.Show(txbLayerName.Text + " thắng");


        }



        void NewGame()
        {
            pnlChessBoard.BackColor = Color.White;
            menuToolStripMenuItem.Enabled = true;
            prbCoolDown.Value = 0;
            countUndo = 0;
            tmCoolDown.Stop();

            //Gọi phương thức Vẽ bàn cờ trong đối tượng bàn cờ
            ChessBoard.DrawChessBoard();
            claimADrawToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;

        }

        private DialogResult ErrorConnectLAN()
        {
            return MessageBox.Show("Lỗi kết nối mạng LAN", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
        }


        void Quit()
        {
            Application.Exit();
        }

        void Undo()
        {
            try
            {
                ChessBoard.Undo();
                prbCoolDown.Value = 0;
            }
            catch
            {
                undoToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Cầu hòa
        /// </summary>
        void ClaimADraw()
        {
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            claimADrawToolStripMenuItem.Enabled = false;
            menuToolStripMenuItem.Enabled = true;
            socket.Send(new SocketData((int)SocketCommand.CLAIMADRAWED, "", new Point()));


        }

        private void DrawGame()
        {
            newGameToolStripMenuItem.Enabled = true;
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            claimADrawToolStripMenuItem.Enabled = false;
            MessageBox.Show("Hòa cờ");
        }

        private bool isCheckClaimADraw()
        {
            return MessageBox.Show(txbLayerName.Text + " muốn cầu hòa, bạn đồng ý không?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes;
        }


        //Hàm xử lý khi kết thúc trò chơi
        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();

        }

        //Hàm xử lý sự kiện thanh progressbar khi người dùng click cell 
        private void ChessBoard_PlayerMarked(object sender, BtnClickEvent e)
        {
            try
            {
                tmCoolDown.Start();
                pnlChessBoard.Enabled = false;
                newGameToolStripMenuItem.Enabled = false;
                claimADrawToolStripMenuItem.Enabled = false;
                prbCoolDown.Value = 0;

                undoToolStripMenuItem.Enabled = false;
                socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));

                Listen();

            }
            catch
            {
                pnlChessBoard.Enabled = false;
                tmCoolDown.Stop();
                ErrorConnectLAN();

            }
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prbCoolDown.PerformStep();

            if (prbCoolDown.Value >= prbCoolDown.Maximum)
            {
                EndGame();



            }

        }

        //Game mới
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            socket.Send(new SocketData((int)SocketCommand.NEW_GAME, "", new Point()));
            pnlChessBoard.Enabled = true;
        }


        private bool isUndoLimited(int countUndoLimited)
        {
            return countUndoLimited >= Constant.UNDOLIMITED;
        }


        //Đi lại
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isUndoLimited(countUndo))
            {
                undoToolStripMenuItem.Enabled = false;
            }
            else
            {

                countUndo++;
                Undo();
                socket.Send(new SocketData((int)SocketCommand.UNDO, "", new Point()));
            }


        }

        private void claimADrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            socket.Send(new SocketData((int)SocketCommand.CLAIMADRAW, "", new Point()));
        }

        //Thoát
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void KingofSkyGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnLAN.Enabled == false)
            {
                if (MessageBox.Show("Bạn có muốn thoát Kingofsky Game không?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                {
                    e.Cancel = true;
                }
                else
                {
                    try
                    {
                        socket.Send(new SocketData((int)SocketCommand.QUIT, "", new Point()));
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        ///Sự kiện kết nối và truyền gói tin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLAN_Click(object sender, EventArgs e)
        {
            socket.IP = txbIP.Text;
            btnLAN.Enabled = false;
            NewGame();

            if (!socket.ConnectServer())
            {
                socket.isServer = true;
                newGameToolStripMenuItem.Enabled = false;
                pnlChessBoard.Enabled = true;
                undoToolStripMenuItem.Enabled = false;
                claimADrawToolStripMenuItem.Enabled = false;
                socket.CreateServer();
            }
            else
            {
                socket.isServer = false;
                pnlChessBoard.Enabled = false;
                newGameToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Enabled = false;
                claimADrawToolStripMenuItem.Enabled = false;
                Listen();
            }


        }

        /// <summary>
        /// Hiển thị thông số và dữ liệu khi hiển thị form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KingofSkyGame_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }

        void Listen()
        {
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();

                    ProgessData(data);
                }
                catch (Exception e)
                {

                }
            });

            listenThread.IsBackground = true;
            listenThread.Start();



        }

        private void ProgessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NEW_GAME:
                    this.Invoke((MethodInvoker)(() =>
                     {
                         NewGame();
                         pnlChessBoard.Enabled = false;
                     }));

                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prbCoolDown.Value = 0;
                        pnlChessBoard.Enabled = true;
                        newGameToolStripMenuItem.Enabled = false;
                        claimADrawToolStripMenuItem.Enabled = true;
                        tmCoolDown.Start();
                        ChessBoard.OtherPlayerMark(data.Point);
                        undoToolStripMenuItem.Enabled = true;
                    }));


                    break;
                case (int)SocketCommand.UNDO:
                    pnlChessBoard.Enabled = true;
                    Undo();
                    pnlChessBoard.Enabled = false;
                    prbCoolDown.Value = 0;
                    break;
                case (int)SocketCommand.CLAIMADRAW:
                    if (isCheckClaimADraw())
                    {
                        ClaimADraw();
                    }
                    break;
                case (int)SocketCommand.CLAIMADRAWED:
                    DrawGame();
                    break;
                case (int)SocketCommand.PLAYER_1_WIN:
                    newGameToolStripMenuItem.Enabled = true;

                    break;
                case (int)SocketCommand.PLAYER_2_WIN:
                    newGameToolStripMenuItem.Enabled = false;
                    break;
                case (int)SocketCommand.QUIT:
                    tmCoolDown.Stop();
                    MessageBox.Show("Đối phương đã thoát");
                    break;
                default:
                    break;
            }

            Listen();
        }




        #endregion

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ván cờ được chơi trên bàn cờ 25x25 ô vuông. Hai bên sẽ thay phiên nhau tích vào những ô vuông trên bàn cờ. Ký hiệu mỗi nước đi của từng người là X hoặc O. Người chơi sẽ phải dùng chiến thuật và kinh nghiệm để tạo thành 1 hàng ngang, dọc, chéo có 5 quân cờ của mình.Lưu ý là 5 quân cờ này không được phép chặn 2 đầu bởi các quân cờ của đối phương. Người chiến thắng là người tạo được hàng 5 đầu tiên. ", "Hướng dẫn chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
