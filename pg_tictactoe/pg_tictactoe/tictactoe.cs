using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pg_tictactoe
{
    public partial class tictactoe : Form
    {
        int symbolCntIfWin = Constants.SYMBOL_CNT_IF_WIN;
        string xMark = Constants.XMARK, oMark = Constants.OMARK;
        string nextMarkValue; // maintain nxtMark to be done on board
        int xMarkCount, oMarkCount; // maintaining Number of X's or O's to save runtime.

        Configuration configuration = new Configuration(); // configuration object
        int[,] intGrid = new int[Configuration.GRID_SIZE, Configuration.GRID_SIZE];
        Button[,] btnGrid = new Button[Configuration.GRID_SIZE, Configuration.GRID_SIZE];
        int max_rows = Configuration.GRID_SIZE - 1, max_cols = Configuration.GRID_SIZE - 1;

        public tictactoe()
        {
            InitializeComponent();

            initialize(true);//call to initialize application, parameter should be always "true".
        }

        /**
         * Method to initialize application, assign values to various variables etc.
         * Accepts a freshGame, to avoid extra operations. First time call in Form1() 
         * should be with value true, and false otherwise always.
         * */
        private void initialize(bool freshGame)
        {
            for (int ivar = 1; ivar <= max_rows; ivar++)
            {
                for (int jvar = 1; jvar <= max_cols; jvar++)
                {
                    if (freshGame)
                    {
                        btnGrid[ivar, jvar] = new Button();
                        intGrid[ivar, jvar] = new int();
                        btnGrid[ivar, jvar] = (Button)Controls["button" + ivar + "" + jvar];
                        btnGrid[ivar, jvar].Click += new System.EventHandler(this.buttonAB_Click);
                        intGrid[ivar, jvar] = 0;
                        btnGrid[ivar, jvar].Text = "";
                    }
                    else
                    {
                        intGrid[ivar, jvar] = 0;
                        btnGrid[ivar, jvar].Text = "";
                        btnGrid[ivar, jvar].Enabled = true;
                    }
                }
            }

            xMarkCount = 0;
            oMarkCount = 0;
            nextMarkValue = xMark;
            btnNextPlay.Text = nextMarkValue;
            btnNextPlay.ForeColor = btnXCC.ForeColor;
            textBox1.Text = "";
        }

        /**
         * Method 1 - For common button Click Methods for the Grid of Buttons
         * This returns the Number value of button eg: 23 for button23
         **/
        public int getButtonNumber(object sender)
        {
            Button b = sender as Button;
            String s = b.Name;
            s = s.Replace("button", "");
            return Convert.ToInt32(s);
        }

        /**
         * Method 2 - For common button Click Methods for the Grid of Buttons
         * This is the click middleware- Click of button executes this,
         * which calls the generic Click method, providing Number value
         * of the button which was clicked by using Method 1.
         **/
        private void buttonAB_Click(object sender, EventArgs e)
        {
            button_click(getButtonNumber(sender));
        }

        /**
         * Method 3 - For common button Click Methods for the Grid of Buttons
         * This is the actual code that shuld be executed when any of the 
         * button from the Grid of Button is clicked.
         **/
        private void button_click(int p)  //generic code functonality for deck card clicks --matching happens here
        {
            performAction(p / 10, p % 10);
        }

        /**
         * Core method - used to mark X or O on board and check if X or O has won etc.
         * Accepts : position of bitton in grid.
         * */
        private void performAction(int row, int col)
        {
            if (nextMarkValue == xMark)
            {
                btnGrid[row, col].Text = nextMarkValue;
                btnGrid[row, col].ForeColor = btnXCC.ForeColor;
                nextMarkValue = oMark;
                btnNextPlay.Text = nextMarkValue;
                btnNextPlay.ForeColor = btnOCC.ForeColor;
                xMarkCount = xMarkCount + 1;
                checkIfWin(xMark, xMarkCount);

            }
            else if (nextMarkValue == oMark)
            {
                btnGrid[row, col].Text = nextMarkValue;
                btnGrid[row, col].ForeColor = btnOCC.ForeColor;
                nextMarkValue = xMark;
                btnNextPlay.Text = nextMarkValue;
                btnNextPlay.ForeColor = btnXCC.ForeColor;
                oMarkCount = oMarkCount + 1;
                checkIfWin(oMark, oMarkCount);
            }
        }

        /**
         * Method to check if the current mark which was just made caused 
         * a WIN  situation
         * */
        private void checkIfWin(string symbol, int symMarkCount)
        {
            bool win = false; //flag to indicate if WIN condition is satisfied.
            int rCount, cCount, fwdDiag, bwdDiag;

            if (symMarkCount < Constants.SYMBOL_CNT_IF_WIN)
            {
                win = false;
            }
            else
            {
                for (int ivar = 1; ivar <= max_rows; ivar++)
                {
                    //reinitialize before evaluating conditions
                    win = false;
                    rCount = 0; cCount = 0; fwdDiag = 0; bwdDiag = 0;

                    for (int jvar = 1; jvar <= max_rows; jvar++)
                    {
                        if (btnGrid[ivar, jvar].Text == symbol)
                        {
                            cCount++;
                        }

                        if (btnGrid[jvar, ivar].Text == symbol)
                        {
                            rCount++;
                        }

                        if (btnGrid[jvar, jvar].Text == symbol)
                        {
                            bwdDiag++;
                        }

                        if (btnGrid[jvar, (max_rows + 1) - jvar].Text == symbol)
                        {
                            fwdDiag++;
                        }
                    }

                    if (cCount == symbolCntIfWin || rCount == symbolCntIfWin || bwdDiag == symbolCntIfWin || fwdDiag == symbolCntIfWin) //WIN criteria
                    {
                        textBox1.Text = cCount + "," + rCount + "," + bwdDiag + "," + fwdDiag + "," + symbol;
                        win = true;
                        break;
                    }
                }
            }

            if (win)
            {
                winMethod(symbol);
            }
        }

        /**
         * Method executed, if one of player has WON.
         * */
        private void winMethod(string symbol)
        {
            pnlGameOver.Show();
            pnlGameOver.Top = Constants.PNL_GO_TOP_POS_VAL;
            pnlGameOver.Left = Constants.PNL_GO_LFT_POS_VAL;

            lbWinnerMsg.Text = lbWinnerMsg.Text.Replace("#", symbol);

            for (int ivar = 1; ivar <= max_rows; ivar++)
            {
                for (int jvar = 1; jvar <= max_cols; jvar++)
                {
                    btnGrid[ivar, jvar].Enabled = false;
                }
            }
        }

        /**
         * Method is invoked when Start New Game button is clicked.
         * */
        private void startNewGame()
        {
            initialize(false);
            pnlGameOver.Hide();
            lbWinnerMsg.Text = Constants.LBL_WINNER_MSG_STR;
        }

        /**
         * SHOW SETTINGS Action Button
         **/
        private void btnSettings_Click(object sender, EventArgs e)
        {
            pnlSettings.Show();
            pnlSettings.Top = Constants.PNL_SETTINGS_TOP_POS_VAL;
            pnlSettings.Left = Constants.PNL_SETTINGS_LFT_POS_VAL;
            pnlSettings.BringToFront();
        }

        /**
         * HIDE SETTINGS Action Button
         **/
        private void btnCloseSettings_Click(object sender, EventArgs e)
        {
            pnlSettings.Hide();

            if (btnNextPlay.Text == "X")
            {
                btnNextPlay.ForeColor = btnXCC.ForeColor;

            }
            else if (btnNextPlay.Text == "O")
            {

                btnNextPlay.ForeColor = btnOCC.ForeColor;
            }
        }

        /**
         * CONFIGURE_COLOR Action Button for X symbol
         **/
        private void btnXCC_Click(object sender, EventArgs e)
        {
            configuration.BTN_X_CLR_INDEX = (configuration.BTN_X_CLR_INDEX + 1) % (Constants.COLOR_CONFIG_COLOR_COUNT);
            btnXCC.ForeColor = Configuration.CLR_OPTNS[configuration.BTN_X_CLR_INDEX];
        }

        /**
         * CONFIGURE_COLOR Action Button for O symbol
         **/
        private void btnOCC_Click(object sender, EventArgs e)
        {
            configuration.BTN_O_CLR_INDEX = (configuration.BTN_O_CLR_INDEX + 1) % (Constants.COLOR_CONFIG_COLOR_COUNT);
            btnOCC.ForeColor = Configuration.CLR_OPTNS[configuration.BTN_O_CLR_INDEX];
        }

        /**
         * EXIT Action Button from GameOver pop-up
         **/
        private void btnGOExit_Click(object sender, EventArgs e)
        {
            //TODO - Exit Application
            pnlGameOver.Hide();
            lbWinnerMsg.Text = Constants.LBL_WINNER_MSG_STR;
        }

        /**
         * NEW_GAME Action Button
         **/
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            startNewGame();
        }

        /**
         * NEW_GAME Action Button from Game Over pop-up
         **/
        private void btnGONewGame_Click(object sender, EventArgs e)
        {
            startNewGame();
        }
    }

    /**
     * Support Class
     * Class for Constants used, accessed as static members of class
     **/
    class Constants
    {
        public static string XMARK = "X";
        public static string OMARK = "O";
        public static int SYMBOL_CNT_IF_WIN = 3;
        public static int PNL_GO_LFT_POS_VAL = 10;
        public static int PNL_GO_TOP_POS_VAL = 190;
        public static int PNL_SETTINGS_TOP_POS_VAL = 20;
        public static int PNL_SETTINGS_LFT_POS_VAL = 20;
        public static string LBL_WINNER_MSG_STR = "Game Over! # Wins";
        public static int COLOR_CONFIG_COLOR_COUNT = Configuration.CLR_OPTNS.Length;
    }

    /**
     * Support Class
     * Class for Configuration values, accessed as static members of class
     **/
    class Configuration
    {
        public int BTN_X_CLR_INDEX = 0;
        public int BTN_O_CLR_INDEX = 0;
        public static int GRID_SIZE = 4;
        public static Color[] CLR_OPTNS = new Color[] { Color.Black, Color.Red, Color.Blue, Color.Yellow, Color.Orange, Color.Pink };

    }
}
