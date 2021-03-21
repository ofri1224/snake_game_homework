using System;
using System.Drawing;

namespace SnakeGamePlatform
{

    public class GameEvents : IGameEvents
    {
        bool alive = true;
        bool timestopped = false;
        static GameObject snake_head;
        int score = 0;
        GameObject[] snake = new GameObject[1] { snake_head };
        TextLabel lblScore;
        TextLabel game_over;
        GameObject food;
        readonly Image snake_head_L = Properties.Resources.snake_head;
        readonly Image snake_head_U = Properties.Resources.snake_head__2_;
        readonly Image snake_head_R = Properties.Resources.snake_head__3_;
        readonly Image snake_head_D = Properties.Resources.snake_head__4_;
        //This function is called by the game one time on initialization!
        //Here you should define game board resolution and size (x,y).
        //Here you should initialize all variables defined above and create all visual objects on screen.
        //You could also start game background music here.
        //use board Object to add game objects to the game board, play background music, set interval, etc...
        public void GameInit(Board board)
        {
            //Setup board size and resolution!
            Board.resolutionFactor = 1;
            board.XSize = 600;
            board.YSize = 800;

            //Adding a text label to the game board.
            Position labelPosition = new Position(100, 100);
            lblScore = new TextLabel("Snake game by ofri gal", labelPosition);
            lblScore.SetFont("Ariel", 24);
            //board.AddLabel(lblScore);

            //Adding Game Object
            Newfood(board);
            Position snake_head_Position = new Position(200, 100);
            snake_head = new GameObject(snake_head_Position, 20, 20);
            snake_head.direction = GameObject.Direction.RIGHT;
            snake_head.SetImage(Properties.Resources.snake_head__3_);
            snake[0] = snake_head;
            for (int j = 0; j < snake.Length; j++)
            {
                board.AddGameObject(snake[j]);
            }



            //Start game timer!
            board.StartTimer(100);
        }
        public void Newfood(Board board)
        {
            Random rnd = new Random();
            Position foodPosition = new Position(20 * (rnd.Next(0, board.XSize / 20)), 20 * (rnd.Next(0, board.YSize / 20)));
            food = new GameObject(foodPosition, 20, 20);
            food.SetImage(Properties.Resources.food);
            if (food.OnScreen(board))
            {
                for (int i = 1; i < snake.Length; i++)
                {
                    if (snake[i].IntersectWith(food))
                    {
                        Newfood(board);
                        break;
                    }
                }
                board.AddGameObject(food);
            }
            else
            {
                Newfood(board);
            }
        }

        //This function is called frequently based on the game board interval that was set when starting the timer!
        //Use this function to move game objects and check collisions
        public void GameClock(Board board)
        {
            for (int i = 1; i < snake.Length; i++)
            {
                if (snake[0].IntersectWith(snake[i]) || !(snake_head.OnScreen(board)))
                {
                    board.PlayShortMusic(@"\Images\game_over.wav");
                    alive = false;
                    board.StopTimer();
                    Position labelPosition = new Position(board.XSize / 2, board.YSize / 2);
                    game_over = new TextLabel($"game over \n final score: \n {score} \n press x to quit", labelPosition);
                    game_over.SetFont("Ariel", 24);
                    game_over.LabelControl.BackColor = Color.Red;
                    board.AddLabel(game_over);
                }
            }
            if (snake[0].IntersectWith(food))
            {
                board.PlayShortMusic(@"\Images\eat.wav");
                score += 1;
                board.RemoveGameObject(food);
                GameObject[] new_snake = new GameObject[snake.Length + 1];
                Array.Copy(snake, 0, new_snake, 0, snake.Length);
                snake = new_snake;
                Newfood(board);
            }
            for (int i = snake.Length - 1; i > 0; i--)
            {
                if (snake[i] != null)
                {
                    board.RemoveGameObject(snake[i]);
                }
                snake.SetValue(new GameObject(snake[i - 1].GetPosition(), 20, 20), i);
                snake[i].PicControl.Image = Properties.Resources.snake_body;
                board.AddGameObject(snake[i]);
            }
            Position snake_head_Position = snake_head.GetPosition();
            switch (snake_head.direction)
            {
                case GameObject.Direction.RIGHT:
                    snake_head_Position.Y += 20;
                    break;
                case GameObject.Direction.LEFT:
                    snake_head_Position.Y -= 20;
                    break;
                case GameObject.Direction.UP:
                    snake_head_Position.X -= 20;
                    break;
                case GameObject.Direction.DOWN:
                    snake_head_Position.X += 20;
                    break;
            }
            snake_head.SetPosition(snake_head_Position);
        }

        //This function is called by the game when the user press a key down on the keyboard.
        //Use this function to check the key that was pressed and change the direction of game objects acordingly.
        //Arrows ascii codes are given by ConsoleKey.LeftArrow and alike
        //Also use this function to handle game pause, showing user messages (like victory) and so on...
        public void KeyDown(Board board, char key)
        {
            if (alive)
            {
                switch (key)
                {
                    case (char)ConsoleKey.LeftArrow:
                        if (snake_head.direction != GameObject.Direction.RIGHT)
                        {
                            snake_head.direction = GameObject.Direction.LEFT;
                            snake_head.PicControl.Image = snake_head_L;
                        }
                        break;
                    case (char)ConsoleKey.RightArrow:
                        if (snake_head.direction != GameObject.Direction.LEFT)
                        {
                            snake_head.direction = GameObject.Direction.RIGHT;
                            snake_head.PicControl.Image = snake_head_R;
                        }
                        break;
                    case (char)ConsoleKey.UpArrow:
                        if (snake_head.direction != GameObject.Direction.DOWN)
                        {
                            snake_head.direction = GameObject.Direction.UP;
                            snake_head.PicControl.Image = snake_head_U;
                        }
                        break;
                    case (char)ConsoleKey.DownArrow:
                        if (snake_head.direction != GameObject.Direction.UP)
                        {
                            snake_head.direction = GameObject.Direction.DOWN;
                            snake_head.PicControl.Image = snake_head_D;
                        }
                        break;
                    case (char)ConsoleKey.X:
                        System.Windows.Forms.Application.Exit();
                        break;
                    case (char)ConsoleKey.Escape:
                        if (timestopped)
                        {
                            board.StartTimer(100);
                            timestopped = false;
                        }
                        else
                        {
                            board.StopTimer();
                            timestopped = true;
                        }
                        break;
                }
            }
            if (key == (char)ConsoleKey.X)
            {
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}
