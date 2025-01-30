using Godot;
using System;


public partial class GameGenerator : Node2D
{
	[Export] TileMapLayer Laberinto;
	[Export] TileMapLayer Trampas;
	[Export] Camera2D Camera;
	[Export] Node2D[] Players;
	private Node2D CurrentNode
	{
		get
		{
			return Players[maze.Turn % Players.Length];
		}
	}
	Maze maze;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		maze = new Maze(new Player[] {new Player(UnitType.Golem), new Player(UnitType.Priest)});
		UpdateWalls();
		UpdateTraps();
		AdjustCamera();
		CreatePlayers();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("move_rigth"))
		{
			maze.Move(Vector2I.Right);
			UpdateCurrentNode();
		}
		if(Input.IsActionJustPressed("move_left"))
		{
			maze.Move(Vector2I.Left);
			UpdateCurrentNode();
		}
		if(Input.IsActionJustPressed("move_up"))
		{
			maze.Move(Vector2I.Up);
			UpdateCurrentNode();
		}
		if(Input.IsActionJustPressed("move_down"))
		{
			maze.Move(Vector2I.Down);
			UpdateCurrentNode();
		}
		if(Input.IsActionJustPressed("turn_change"))
		{
			maze.NextTurn();
		}
		if(Input.IsActionJustPressed("skill_activate"))
		{
			GD.Print("Is activate");
			if(maze.CurrentPlayer.IsSkillActive)
			{
				maze.ActivateSkill();
				UpdateAll();
			}
		}
	}

	private void UpdateWalls()
	{
		for (int i = 0; i < maze.WallMap.GetLength(0); i++)
		{
			for (int j = 0; j < maze.WallMap.GetLength(1); j++)
			{
				if(!maze.WallMap[i,j])
				Laberinto.SetCell(new Vector2I(i,j), 0, new Vector2I(8,1));
				else
				{
					Laberinto.SetCell(new Vector2I(i,j), -1);
				}
			}
		}
	} 
	private void UpdateTraps()
	{
		for (int i = 0; i < maze.TrapMap.GetLength(0); i++)
		{
			for (int j = 0; j < maze.TrapMap.GetLength(1); j++)
			{
				switch(maze.TrapMap[i,j])
				{
					case 1:
					SetFirstTrap(i,j);
					break;
					case 2:
					SetSecondTrap(i,j);
					break;
					case 3:
					SetThirdTrap(i,j);
					break;
					default:
					Trampas.SetCell(new Vector2I(i,j),-1);
					break;
				}
			}
		}
	}
	private void AdjustCamera()
	{
		Vector2I cellSize = Laberinto.TileSet.TileSize;
		Vector2 mapSizeInPixels = new Vector2(Maze.MapLength * cellSize.X * 3, Maze.MapWidth * cellSize.Y * 3);
		Vector2 viewportSize = GetViewportRect().Size;
		float zoomX = viewportSize.X / mapSizeInPixels.X;
        float zoomY = viewportSize.Y / mapSizeInPixels.Y;
        float zoomFactor = Mathf.Min(zoomX, zoomY);
		Camera.Zoom = new Vector2(zoomFactor, zoomFactor);

	}

	private void SetFirstTrap(int x, int y)
	{
		Trampas.SetCell(new Vector2I(x,y), 0, new Vector2I(0,7));
	}
	private void SetSecondTrap(int x, int y)
	{
		Trampas.SetCell(new Vector2I(x,y), 0, new Vector2I(0,8));
	}
	private void SetThirdTrap(int x, int y)
	{
		Trampas.SetCell(new Vector2I(x,y), 0, new Vector2I(1,8));
	}

	private void CreatePlayers()
	{
		for (int i = 0; i < maze.PlayerList.Length; i++)
		{
			switch (maze.PlayerList[i].Type)
			{
				case UnitType.AxeMan:
				CreateAxeMan(i);
				break;

				case UnitType.Adventurer:
				CreateAdventurer(i);
				break;

				case UnitType.Priest:
				CreatePriest(i);
				break;

				case UnitType.Golem:
				CreateGolem(i);
				break;

				case UnitType.Mummy:
				CreateMummy(i);
				break;
				
				default:
				break;
			}
			
		}
	}
	private void CreateAxeMan(int i)
	{
		PackedScene escena = (PackedScene)GD.Load("res://axe_man.tscn");
		Node2D instancia = escena.Instantiate<Node2D>(); 
		Players[i].AddChild(instancia);
		Vector2 x = Laberinto.MapToLocal(maze.PlayerList[i].Position);
		Players[i].Position = Laberinto.ToGlobal(x);
	}
	private void CreateAdventurer(int i)
	{
		PackedScene escena = (PackedScene)GD.Load("res://adventurer.tscn");
		Node2D instancia = escena.Instantiate<Node2D>(); 
		Players[i].AddChild(instancia);
		Vector2 x = Laberinto.MapToLocal(maze.PlayerList[i].Position);
		Players[i].Position = Laberinto.ToGlobal(x);
	}
	private void CreatePriest(int i)
	{
		PackedScene escena = (PackedScene)GD.Load("res://priest.tscn");
		Node2D instancia = escena.Instantiate<Node2D>(); 
		Players[i].AddChild(instancia);
		Vector2 x = Laberinto.MapToLocal(maze.PlayerList[i].Position);
		Players[i].Position = Laberinto.ToGlobal(x);
	}
	private void CreateGolem(int i)
	{
		PackedScene escena = (PackedScene)GD.Load("res://golem.tscn");
		Node2D instancia = escena.Instantiate<Node2D>(); 
		Players[i].AddChild(instancia);
		Vector2 x = Laberinto.MapToLocal(maze.PlayerList[i].Position);
		Players[i].Position = Laberinto.ToGlobal(x);
	}
	private void CreateMummy(int i)
	{
		PackedScene escena = (PackedScene)GD.Load("res://mummy.tscn");
		Node2D instancia = escena.Instantiate<Node2D>(); 
		Players[i].AddChild(instancia);
		Vector2 x = Laberinto.MapToLocal(maze.PlayerList[i].Position);
		Players[i].Position = Laberinto.ToGlobal(x);
	}

	private void UpdateCurrentNode()
	{
		Vector2 localPos = Laberinto.MapToLocal(maze.CurrentPlayer.Position);
		CurrentNode.Position = Laberinto.ToGlobal(localPos);
		UpdateTraps();
	}

	private void UpdateAll()
	{
		UpdateWalls();
		UpdateTraps();
	}
}
