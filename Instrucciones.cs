using Godot;
using System;

public partial class Instrucciones : Control
{
	public void on_salir_pressed()
	{
		GetTree().ChangeSceneToFile("res://menu.tscn");
	}
}
