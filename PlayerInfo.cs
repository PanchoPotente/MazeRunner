using Godot;
using System;

public partial class PlayerInfo : Control
{
	[Export] Label Life;
	[Export] Label Moves;

	public void UpdateStats(int lp, int mov)
	{
		Life.Text = "Life :  " + lp;
		Moves.Text = "Moves :  " + mov;
	}
}
