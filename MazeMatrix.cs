using Godot;
using System;
using System.Collections.Generic;

class MazeMatrix
{
    public int[,] BoardMatrix {  get; private set; }

    public MazeMatrix(int size)
    {
        int[,] matrix = new int[size*2,size*2];
        CreateMaze(matrix);
        DisposeTraps(matrix);
        BoardMatrix = matrix;
        
    }

    private void CreateMaze(int[,] matrix)
    {
        bool[,] mask = new bool[matrix.GetLength(0),matrix.GetLength(1)];
        CreateMaze(0,0,matrix,mask);
    }
    private void CreateMaze(int x, int y, int[,] matrix, bool[,] mask)
    {
        mask[x,y] = true;
        matrix[x, y] = 1;
        int[][] directions = new int[][] { new int[] { 0, 2 }, new int[] { 0, -2 }, new int[] { 2, 0 }, new int[] { -2, 0 } };
        Shuffle(directions);
        for (int i = 0; i < directions.Length; i++)
        {
            if (IsBetwenTheBounds(x + directions[i][0], y + directions[i][1], matrix.GetLength(0)) &&
                !(mask[x + directions[i][0], y + directions[i][1]]))
            {
                matrix[x + (directions[i][0]/2),y + (directions[i][1]/2)] = 1;
                CreateMaze(x + directions[i][0],y + directions[i][1],matrix,mask);
            }
        }

    }

    private static void Shuffle(int[][] directions)
    {
        Random random = new Random();
        for (int i = 0; i < directions.Length; i++)
        {
            int n = random.Next(0, 4);
            int[] swap = directions[i];
            directions[i] = directions[n];
            directions[n] = swap;
        }
    }

    private static bool IsBetwenTheBounds(int x, int y, int length)
    {
        return (x >= 0 && x < length && y >= 0 && y < length);
    }

    private static void DisposeTraps(int[,] matrix)
    {
        Random random = new Random();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if(matrix[i,j] == 1)
                {
                    int x = random.Next(0,30);
                    if(x > 26)
                    {
                        matrix[i,j] = x - 25;
                    }
                }
            }
        }
    }

    public void PrintMatrix()
    {
        for (int i = 0; i < BoardMatrix.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < BoardMatrix.GetLength(1); j++)
            {
                row += BoardMatrix[i, j];
            }
            Console.WriteLine(row);
        }
    }

    public Vector2I[] ReachableCells(Vector2I position, int speed)
    {
        int[,] matrix = new int[BoardMatrix.GetLength(0), BoardMatrix.GetLength(1)];
        int[][] directions = new int[][] { new int[] { 0, 2 }, new int[] { 0, -2 }, new int[] { 2, 0 }, new int[] { -2, 0 } };
        List<Vector2I> cells = new List<Vector2I>(0);
        cells.Add(position);
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < directions.Length; j++)
            {
                if(matrix[cells[i].X,cells[i].Y] < speed
                    &&IsBetwenTheBounds(cells[i].X + directions[j][0], cells[i].Y + directions[j][1], matrix.Length)
                    && BoardMatrix[cells[i].X + directions[j][0],cells[i].Y + directions[j][1]] != 0 )
                    {
                        matrix[cells[i].X + directions[j][0],cells[i].Y + directions[j][1]] ++;
                        cells.Add(new Vector2I(cells[i].X + directions[j][0],cells[i].Y + directions[j][1]));
                    }
            }
        }

        return cells.ToArray();
    }

}
