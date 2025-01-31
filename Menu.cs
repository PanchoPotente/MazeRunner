using Godot;
using System;

public partial class Menu : Control
{
	public void _on_play_pressed()
    {
        GetTree().ChangeSceneToFile("res://seleccion.tscn");
    }

    public void _on_options_pressed()
    {
        GetTree().ChangeSceneToFile("res://instrucciones.tscn");
    }

    public void _on_exit_pressed()
    {
        GetTree().Quit();
    }
}
