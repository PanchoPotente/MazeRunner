using Godot;
using System;

public partial class Seleccion : Control
{
	[Export] ButtonGroup Player1;
	[Export] ButtonGroup Player2;

	public void _on_continuar_pressed()
	{
		var activeRadioButton1 = Player1.GetPressedButton() as CheckBox;
		var activeRadioButton2 = Player2.GetPressedButton() as CheckBox;
		if(activeRadioButton1 != null && activeRadioButton2 != null)
		{
			Global.FirstPlayer = CheckUnits(activeRadioButton1.Name);
			Global.SecondPlayer = CheckUnits(activeRadioButton2.Name);
			GetTree().ChangeSceneToFile("res://game_generator.tscn");
		}
	}

	private UnitType CheckUnits(string name)
	{
		switch(name)
		{
			case "AxeMan":
			return UnitType.AxeMan;

			case "Adventurer":
			return UnitType.Adventurer;
			 
			case "Golem":
			return UnitType.Golem;

			case "Mummy":
			return UnitType.Mummy;

			case "Priest" :
			return UnitType.Priest;

			default:
			return UnitType.Adventurer;
			
		}
	}
}
