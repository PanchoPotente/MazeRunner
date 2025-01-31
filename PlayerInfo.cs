using Godot;
using System;

public partial class PlayerInfo : Control
{
	[Export] Label Life;
	[Export] Label Moves;

	[Export] Label Skill;

	public void UpdateStats(int lp, int mov, bool skill)
	{
		Life.Text = "Life :  " + lp;
		Moves.Text = "Moves :  " + mov;
		if(skill) Skill.Text = "Skill : SI";
		else Skill.Text = "Skill : NO";
	}
}
