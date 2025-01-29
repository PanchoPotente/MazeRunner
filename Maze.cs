using System;
using System.Collections.Generic;
using Godot;

class Maze
{
    public bool[,] WallMap {get;private set;} 

    public int[,] TrapMap {get;private set;}

    public Player[] PlayerList {get;private set;}

    public int Turn {get; private set;}

    public Player CurrentPlayer 
    {
        get
        {
            return PlayerList[Turn % PlayerList.Length];
        }
        private set{}
    }

    public const int MapLength = 34;

    public const int MapWidth = 20;

    const int TrapOcurrence = 30;


    public Maze(Player[] players)
    {
        PlayerList = players;
        BuildWalls();
        BuildTraps();
    }

    private void BuildWalls()
    {
        WallMap = new bool[ MapLength, MapWidth ];
        bool[,] mask = new bool[ MapLength, MapWidth ];
        BuildWalls(0,0,mask);
    }

    private void BuildWalls(int x, int y, bool[,] mask)
    {
        mask[x,y] = true;
        WallMap[x,y] = true;
        int[][] directions = new int[][] { new int[] { 0, 2 }, new int[] { 0, -2 }, new int[] { 2, 0 }, new int[] { -2, 0 } };
        Shuffle(directions);
        for (int i = 0; i < directions.Length; i++)
        {
            if(IsInRange(x + directions[i][0], y + directions[i][1]) 
                && !(mask[x + directions[i][0], y + directions[i][1]]))
            {
                WallMap[x + directions[i][0] / 2, y + directions[i][1] / 2 ] = true;
                BuildWalls(x + directions[i][0], y + directions[i][1], mask);
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

    private bool IsInRange(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < MapLength && y < MapWidth);
    }

    private void BuildTraps()
    {
        TrapMap = new int[MapLength,MapWidth];
        Random random = new Random();
        for (int i = 0; i < TrapMap.GetLength(0); i++)
        {
            for (int j = 0; j < TrapMap.GetLength(1); j++)
            {
                if(WallMap[i,j])
                {
                    int x = random.Next( 0, TrapOcurrence);
                    if(x > TrapOcurrence - 4)
                    {
                        TrapMap[i,j] = x - (TrapOcurrence - 4);
                    }
                }
            }
        }
        TrapMap[0,0] = 0;
    }

    public void NextTurn() 
    {
        Turn++;
        CurrentPlayer.RefreshMovements();
    }

    public void Move(Vector2I vector)
    {
        vector += CurrentPlayer.Position;
        if(IsInRange(vector.X,vector.Y) && WallMap[vector.X, vector.Y] && CurrentPlayer.Movements > 0)
        {
            CurrentPlayer.Position = vector;
            CurrentPlayer.Movements --;
            if(TrapMap[CurrentPlayer.Position.X, CurrentPlayer.Position.Y] != 0) 
            ActivateTrap(vector.X,vector.Y);
        }
    }

    private void ActivateTrap(int x, int y)
    {
        int type = TrapMap[x,y];
        switch(type)
        {
            case 1:
            ActivateFirstTrap();
            break;

            case 2:
            ActivateSecondTrap();
            break;

            case 3: 
            ActivateThirdTrap();
            break;

            default:
            break;
        }
        TrapMap[x,y] = 0;
    }

    private void ActivateFirstTrap()
    {
        CurrentPlayer.Damage(2);
    }
    private void ActivateSecondTrap()
    {
        CurrentPlayer.Movements = 0;
    }
    private void ActivateThirdTrap()
    {
        CurrentPlayer.Damage(-1);
    }

    public void ActivateSkill()
    {
        if(CurrentPlayer.IsSkillActive)
        {
            switch(CurrentPlayer.Type)
            {
                case UnitType.AxeMan:
                break;
                case UnitType.Priest:
                break;
                case UnitType.Adventurer:
                break;
                case UnitType.Golem:
                break;
                case UnitType.Mummy:
                break;
            }
        }
        CurrentPlayer.TurnsPased = 0;
        
    }

    private void GolemSkill()
    {
        int[][] directions = new int[][] { new int[] { 0, 2 }, new int[] { 0, -2 }, new int[] { 2, 0 }, new int[] { -2, 0 } };
        Shuffle(directions);
        for (int i = 0; i < directions.Length; i++)
        {
            if(IsInRange(CurrentPlayer.Position.X + directions[i][0], CurrentPlayer.Position.Y + directions[i][1])
            && !(WallMap[CurrentPlayer.Position.X + directions[i][0], CurrentPlayer.Position.Y + directions[i][1]]))
            {
                WallMap[CurrentPlayer.Position.X + directions[i][0], CurrentPlayer.Position.Y + directions[i][1]] = true;
                break;
            }   
        }
    }

    private void PriestSkill()
    {
        CurrentPlayer.Damage(-3);
    }

    private void MummySkill()
    {
        CurrentPlayer.Damage(1);
        CurrentPlayer.Movements += CurrentPlayer.Speed;
    }

}

