using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private int[,] grid;
        private int rows = 20;
        private int columns = 10;
        private int cellSize = 30;
        private Shape currentShape;
        private System.Windows.Forms.Timer timer;
        private int score = 0;
        private int linesCleared = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            grid = new int[rows, columns];
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // скорость игры
            timer.Tick += Timer_Tick;

            StartGame();
              this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveShapeDown();
            Invalidate(); // перерисовываем форму
        }

        private void StartGame()
        {
            currentShape = GetRandomShape();
            currentShape.Y = 0; // Начинаем с верхней части поля
            currentShape.X = columns / 2 - currentShape.Matrix.GetLength(1) / 2; // Центрируем фигуру по горизонтали

            //Проверка, можно ли разместить новую фигуру
            if (!CanPlaceShape(currentShape))
            {
                timer.Stop();
                MessageBox.Show("Игра окончена!");
            }
            else
            {
                timer.Start();
            }
        }

        private void MoveShapeDown()
        {
            if (CanMove(currentShape, 1, 0))
            {
                currentShape.Move(1, 0);
            }
            else
            {
                MergeShapeToGrid(); // Фиксируем фигуру на поле
                CheckForFullLines();
                StartGame(); // Создаем новую фигуру

            }
            Invalidate(); // перерисовываем форму
        }

        private void MergeShapeToGrid()
        {
            for (int y = 0; y < currentShape.Matrix.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.Matrix.GetLength(1); x++)
                {
                    if (currentShape.Matrix[y, x] == 1)
                    {
                        grid[currentShape.Y + y, currentShape.X + x] = 1;
                    }
                }
            }
        }

        private void CheckForFullLines()
        {
            for (int y = 0; y < rows; y++)
            {
                bool fullLine = true;
                for (int x = 0; x < columns; x++)
                {
                    if (grid[y, x] == 0)
                    {
                        fullLine = false;
                        break;
                    }
                }

                if (fullLine)
                {
                    //Увеличиваем счёт и количество очищенных линий
                    linesCleared++;
                    score += 100; // Вы можете изменить количество очков за линию

                    //Удаляем полную линию
                    for (int j = y; j > 0; j--)
                    {
                        for (int x = 0; x < columns; x++)
                        {
                            grid[j, x] = grid[j - 1, x];
                        }
                    }
                    for (int x = 0; x < columns; x++)
                    {
                        grid[0, x] = 0;
                    }
                }
            }
            UpdateScore();
            Invalidate(); // Перерисовываем форму
        }

        private void UpdateScore()
        {
            scoreLabel.Text = $"Счет: {score}";
            linesClearedLabel.Text = $"Очистка линий: {linesCleared}";
        }

        private bool CanPlaceShape(Shape shape)
        {
            for (int y = 0; y < shape.Matrix.GetLength(0); y++)
            {
                for (int x = 0; x < shape.Matrix.GetLength(1); x++)
                {
                    if (shape.Matrix[y, x] == 1 && grid[shape.Y + y, shape.X + x] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CanMove(Shape shape, int dy, int dx)
        {
            for (int y = 0; y < shape.Matrix.GetLength(0); y++)
            {
                for (int x = 0; x < shape.Matrix.GetLength(1); x++)
                {
                    if (shape.Matrix[y, x] == 1)
                    {
                        int newX = shape.X + x + dx;
                        int newY = shape.Y + y + dy;
                        if (newX < 0 || newX >= columns || newY < 0 || newY >= rows || grid[newY, newX] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (CanMove(currentShape, 0, -1))
                    {
                        currentShape.Move(0, -1);
                    }
                    break;
                case Keys.Right:
                    if (CanMove(currentShape, 0, 1))
                    {
                        currentShape.Move(0, 1);
                    }
                    break;
                case Keys.Down:
                    MoveShapeDown();
                    break;
                case Keys.Up:
                    RotateShape();
                    break;
            }
            Invalidate(); // Перерисовываем форму
        }

        private void RotateShape()
        {
            currentShape.Rotate();
            if (!CanMove(currentShape, 0, 0)) // Проверяем, можно ли разместить фигуру после вращения
            {
                currentShape.Rotate(); // Если нельзя, поворачиваем обратно
            }
        }

        private Shape GetRandomShape()
        {
            Random rand = new Random();
            int shapeType = rand.Next(0, 7); // 0-6 для 7 разных фигур
            switch (shapeType)
            {
                case 0: return new IShape();
                case 1: return new JShape();
                case 2: return new LShape();
                case 3: return new OShape();
                case 4: return new SShape();
                case 5: return new TShape();
                case 6: return new ZShape();
                default: return new IShape();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            //Рисуем игровое поле
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    //Подсветка занятых полей
                    if (grid[y, x] != 0)
                    {
                        g.FillRectangle(Brushes.Blue, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else
                    {
                        g.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }

            //Рисуем текущую фигуру
            for (int y = 0; y < currentShape.Matrix.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.Matrix.GetLength(1); x++)
                {
                    if (currentShape.Matrix[y, x] == 1)
                    {
                        g.FillRectangle(Brushes.Red, (currentShape.X + x) * cellSize, (currentShape.Y + y) * cellSize, cellSize, cellSize);
                    }
                }
            }
        }
    }

    public abstract class Shape
    {
        public int[,] Matrix { get; protected set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Move(int dy, int dx)
        {
            Y += dy;
            X += dx;
        }

        public void Rotate()
        {
            int width = Matrix.GetLength(1);
            int height = Matrix.GetLength(0);
            int[,] newMatrix = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newMatrix[x, height - 1 - y] = Matrix[y, x];
                }
            }

            Matrix = newMatrix;
        }
    }

    public class IShape : Shape
    {
        public IShape()
        {
            Matrix = new int[,]
            {
                { 1, 1, 1, 1 }
            };
        }
    }

    public class JShape : Shape
    {
        public JShape()
        {
            Matrix = new int[,]
            {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 1, 1, 0 }
            };
        }
    }

    public class LShape : Shape
    {
        public LShape()
        {
            Matrix = new int[,]
            {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 0, 1, 1 }
            };
        }
    }

    public class OShape : Shape
    {
        public OShape()
        {
            Matrix = new int[,]
            {
                { 1, 1 },
                { 1, 1 }
            };
        }
    }

    public class SShape : Shape
    {
        public SShape()
        {
            Matrix = new int[,]
            {
                { 0, 1, 1 },
                { 1, 1, 0 },
                { 0, 0, 0 }
            };
        }
    }

    public class TShape : Shape
    {
        public TShape()
        {
            Matrix = new int[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 }
            };
        }
    }

    public class ZShape : Shape
    {
        public ZShape()
        {
            Matrix = new int[,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 },
                { 0, 0, 0 }
            };
        }
    }
}