using Godot;
using System;


public partial class GameGenerator : Node2D
{
	[Export] TileMapLayer Laberinto;
	[Export] TileMapLayer Trampas;
	[Export] Camera2D Camera;
	[Export] Node2D[] Players;

	[Export] Control PlayerInfo1; 
	[Export] Control PlayerInfo2; 
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
		maze = new Maze(new Player[] {new Player(Global.FirstPlayer), new Player(Global.SecondPlayer)});
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
			UpdatePlayerInfo();
		}
		if(Input.IsActionJustPressed("skill_activate"))
		{
			if(maze.CurrentPlayer.IsSkillActive)
			{
				maze.ActivateSkill();
				UpdateAll();
			}
		}
		if(maze.CurrentPlayer.Position.X == 32 && maze.CurrentPlayer.Position.Y == 18)
		{
			GetTree().ChangeSceneToFile("res://seleccion.tscn");
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
		Laberinto.SetCell(new Vector2I(32,18),0, new Vector2I(0,2));
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
		UpdatePlayerInfo();
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
		Vector2 localPos = Laberinto.MapToLocal(maze.PlayerList[0].Position);
		Players[0].Position = Laberinto.ToGlobal(localPos);
		Vector2 localPos2 = Laberinto.MapToLocal(maze.PlayerList[1].Position);
		Players[1].Position = Laberinto.ToGlobal(localPos2);
		UpdateTraps();
		UpdatePlayerInfo();
	}

	private void UpdateAll()
	{
		UpdateCurrentNode();
		UpdateWalls();
		UpdateTraps();
	}

	private void UpdatePlayerInfo()
	{
		PlayerInfo FirstPlayer = GetNode<PlayerInfo>("/root/GameGenerator/CanvasLayer/Player1Info");
		PlayerInfo SecondPlayer = GetNode<PlayerInfo>("/root/GameGenerator/CanvasLayer/Player2Info");
		SecondPlayer.UpdateStats(maze.PlayerList[1].LifePoints, maze.PlayerList[1].Movements);
		FirstPlayer.UpdateStats(maze.PlayerList[0].LifePoints, maze.PlayerList[0].Movements);
		
	}
}
