using System;
using Godot;

public enum UnitType
{
    AxeMan,
    Priest,
    Golem,
    Adventurer,
    Mummy
}

class Player
{
    public int Speed;
    public UnitType Type {get; private set;}
    public Vector2I Position;
    public int LifePoints;
    public int Movements;
    private int coolDown;
    public int TurnsPased;
    public bool IsSkillActive 
    {
        get 
        {
            return (TurnsPased >= coolDown);
        }
    }

    public Player(UnitType type)
    {
        Type = type;
        Position = Vector2I.Zero;

        switch(type)
        {
            case UnitType.AxeMan:
            MakeAxeMan();
            break;
            case UnitType.Adventurer:
            MakeAdventurer();
            break;
            case UnitType.Priest:
            MakePriest();
            break;
            case UnitType.Golem:
            MakeGolem();
            break;
            case UnitType.Mummy:
            MakeMummy();
            break;
        }
        RefreshMovements();
        TurnsPased = coolDown ;
    }


    private void MakeAxeMan()
    {
        Speed = 5;
        LifePoints = 6;
        coolDown = 2;
    }
    private void MakePriest()
    {
        Speed = 4;
        LifePoints = 5;
        coolDown = 2;
    }
    private void MakeAdventurer()
    {
        Speed = 5;
        LifePoints = 5;
        coolDown = 2;
    }
    private void MakeGolem()
    {
        Speed = 3;
        LifePoints = 8;
        coolDown = 3;
    }
    private void MakeMummy()
    {
        Speed = 10;
        LifePoints = 2;
        coolDown = 0;

    }

    public void Death()
    {
        Position = Vector2I.Zero;
        Movements = 0;  
        Player player = new Player(Type);
        this.LifePoints = player.LifePoints;
    }

    public void RefreshMovements()
    {
        Movements = Speed;
        TurnsPased++;
    }

    public void Damage(int value)
    {
        if(LifePoints - value > 0) LifePoints -= value;
        else Death();
    }
    

}
