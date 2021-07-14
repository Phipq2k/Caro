using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class KingofSkyGame : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;

        SocketManager socket;

        #endregion
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

            tmCoolDown.Interval = Constant.COOL_DOWN_INTERVAL;

            socket = new SocketManager();

            if(btnLAN.Enabled == true)
            {
                menuToolStripMenuItem.Enabled = false;
            }

        }
        #region Events Handler Methods
        void EndGame()
        {
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            claimADrawToolStripMenuItem.Enabled = false;
            txbLayerName.Text = txbLayerName.Text == "Player1" ? "Player2" : "Player1";
            MessageBox.Show(txbLayerName.Text + " thắng");
        }

        void NewGame()
        {
            menuToolStripMenuItem.Enabled = true;
            prbCoolDown.Value = 0;
            count = 0;
            tmCoolDown.Stop();
            //Gọi phương thức Vẽ bàn cờ trong đối tượng bàn cờ
            ChessBoard.DrawChessBoard();
            claimADrawToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;


        }

       
        void Quit()
        {
            Application.Exit();
        }

        void Undo()
        {
            ChessBoard.Undo();
            prbCoolDown.Value = 0;
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

        private DialogResult ErrorLAN()
        {
            return MessageBox.Show("Không có kết nối mạng LAN!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button3);
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
                Quit();
                
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
            return countUndoLimited == Constant.UNDOLIMITED;
        }
        private int count = 0;
        //Đi lại
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            count++;
            if (isUndoLimited(count))
            {
                undoToolStripMenuItem.Enabled = false;
            }
            else
            {
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
           if(btnLAN.Enabled == true)
            {
                ErrorLAN();
            }
           else
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
                        newGameToolStripMenuItem.Enabled = true;
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
                    newGameToolStripMenuItem.Enabled = true;
                    tmCoolDown.Stop();
                    pnlChessBoard.Enabled = false;
                    claimADrawToolStripMenuItem.Enabled = false;
                    MessageBox.Show("Đối phương đã đồng ý hòa");
                    break;
                case (int)SocketCommand.END_GAME:
                    EndGame();
                    break;
                case (int)SocketCommand.QUIT:
                    tmCoolDown.Stop();
                    MessageBox.Show("Người chơi đã thoát");
                    break;
                case (int)SocketCommand.CONNECTED:
                    MessageBox.Show("Đối phương đã kết nối");
                    break;
                default:
                    break;
            }

            Listen();
        }

        #endregion




    }
}
