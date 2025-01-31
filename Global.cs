// Global.cs (Autoload)
using Godot;

public partial class Global : Node
{
  public static UnitType FirstPlayer { get; set; } = UnitType.AxeMan;
  public static UnitType SecondPlayer { get; set; } = UnitType.Golem;
}
