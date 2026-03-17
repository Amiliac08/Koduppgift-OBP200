namespace OBP200_RolePlayingGame;

public class Character
{
    public string Name { get; protected set; }
    public int Hp { get; protected set; }
    public int Attack { get; protected set; }
    public int Defence { get; protected set; }

    public Character(string name, int hp, int attack, int defence)
    {
        Name = name;
        Hp = hp;
        Attack = attack;
        Defence = defence;
    }

    protected virtual void CalculateDamage()
    {
        
    }

}