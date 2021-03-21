using System.Windows.Forms;

namespace SnakeGamePlatform
{
    public partial class GameBoard : Form
    {
        private Board board;
        public GameBoard(Board board)
        {
            InitializeComponent();
            this.board = board;
        }

        public Timer GetTimer()
        {
            return tmrClock;
        }

        private void GameBoard_KeyDown(object sender, KeyEventArgs e)
        {
            char c = (char)e.KeyValue;
            board.Events.KeyDown(board, c);
        }
    }
}
