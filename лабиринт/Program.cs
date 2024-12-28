using System;
using System.Collections.Generic;

class Program
{
    public struct Point
    {
        public int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    static Stack<Point> stack = new Stack<Point>();
    static List<Point> allways = new List<Point>();
    static Random random = new Random();
    static int consoleW = 51; // Ширина лабиринта (нечетное значение)
    static int consoleH = 25; // Высота лабиринта (нечетное значение)
    static char[,] maze;
    static Point playerpos;
    static Point targetpos;

    static void Main()
    {
        Console.CursorVisible = false;
        Thread thread = new Thread(PlayerActions);
        thread.Start();
        // Инициализация лабиринта
        maze = new char[consoleH, consoleW];
        for (int y = 0; y < consoleH; y++)
            for (int x = 0; x < consoleW; x++)
                maze[y, x] = '█'; // Заполняем всё стенами

        // Генерация лабиринта
        GenerateMaze(1, 1);

        // Отображение лабиринта
        Console.Clear();
        Console.SetWindowSize(consoleW + 2, consoleH + 2);
        Console.SetBufferSize(consoleW + 2, consoleH + 2);
        for (int y = 0; y < consoleH; y++)
        {
            for (int x = 0; x < consoleW; x++)
                Console.Write(maze[y, x]);
            Console.WriteLine();
        }
        int player = random.Next(0, allways.Count - 1);
        int point = random.Next(0, allways.Count - 1);
        if (player == point)
        {
            point = random.Next(0, allways.Count - 1);
        }
        Console.SetCursorPosition(allways[player].x, allways[player].y);
        Console.Write("P");
        playerpos = new Point(Console.CursorLeft, Console.CursorTop);
        Console.SetCursorPosition(allways[point].x, allways[point].y);
        Console.Write("@");
        targetpos = new Point(Console.CursorLeft, Console.CursorTop);
    }
    static void GenerateMaze(int startX, int startY)
    {
        stack.Push(new Point(startX, startY));
        allways.Add(new Point(startX, startY));
        maze[startY, startX] = ' '; // Начальная точка

        // Направления: вправо, влево, вверх, вниз
        Point[] directions = new Point[]
        {
            new Point(2, 0),
            new Point(-2, 0),
            new Point(0, -2),
            new Point(0, 2)
        };

        while (stack.Count > 0)
        {
            Point current = stack.Pop();

            // Перемешиваем направления
            for (int i = 0; i < directions.Length; i++)
            {
                int swapIndex = random.Next(directions.Length);
                var temp = directions[i];
                directions[i] = directions[swapIndex];
                directions[swapIndex] = temp;
            }

            // Ищем подходящее направление
            foreach (var dir in directions)
            {
                int nx = current.x + dir.x;
                int ny = current.y + dir.y;

                // Проверяем, можно ли вырыть проход
                if (nx > 0 && nx < consoleW - 1 && ny > 0 && ny < consoleH - 1 && maze[ny, nx] == '█')
                {
                    // Убираем стену между текущей и новой точкой
                    maze[ny - dir.y / 2, nx - dir.x / 2] = ' ';
                    maze[ny, nx] = ' ';

                    stack.Push(new Point(nx, ny)); // Добавляем новую точку в стек
                    allways.Add(new Point(nx, ny));
                }
            }
        }

    }
    static void PlayerActions()
    {
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.W:
                    if (maze[playerpos.y - 1, playerpos.x] != '█')
                    {
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('.');
                        playerpos.y--;
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('P');
                    }
                    break;
                case ConsoleKey.S:
                    if (maze[playerpos.y + 1, playerpos.x] != '█')
                    {
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('.');
                        playerpos.y++;
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('P');
                    }
                    break;
                case ConsoleKey.A:
                    if (maze[playerpos.y, playerpos.x - 1] != '█')
                    {
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('.');
                        playerpos.x--;
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('P');
                    }
                    break;
                case ConsoleKey.D:
                    if (maze[playerpos.y, playerpos.x + 1] != '█')
                    {
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('.');
                        playerpos.x++;
                        Console.SetCursorPosition(playerpos.x, playerpos.y);
                        Console.Write('P');
                    }
                    break;
            }
            if (playerpos.x == targetpos.x && playerpos.y == targetpos.y)
            {
                Console.Clear();
                Console.WriteLine("You win!");
                break;
            }
        }
    }
}
