namespace OBP200_RolePlayingGame;

public abstract class Player : Character
{
    public string ClassName { get; private set; }
    public int MaxHp { get; private set; }
    public int Gold { get; private set; }
    public int Xp { get; private set; }
    public int Level { get; private set; }
    public int Potions { get; private set; }
    
    public List<string> Inventory { get; private set; }

    public Player(string name, int hp, int attack, int defence, string classname, int maxHp, int gold, int xp, int level, int potions) : base(name, hp, attack, defence)
    {
        ClassName = classname;
        MaxHp = maxHp;
        Gold = gold;
        Xp = xp;
        Level = level;
        Potions = potions;

        Inventory = new List<string>() { "Wooden sword;Cloth Armor" };
    }
    
    public virtual void UsePotion()
    {
        Potions++;
    }

    public virtual void TryRunAway()
    {
        
    }
    
    public virtual bool IsPlayerDead()
    {
        return true;
    }
}