namespace OBP200_RolePlayingGame;

public abstract class Player : Character
{
    // Random
    static Random Rng = new Random();
    
    public string ClassName { get; }
    public int MaxHp { get; private set; }
    public int Gold { get; protected set; }
    public int Xp { get; private set; }
    public int Level { get; private set; }
    public int Potions { get; private set; }

    public List<string> Inventory { get; } 

    public Player(string name, int hp, int attack, int defence, string classname, int maxHp, int gold, int xp, int level, int potions) : base(name, hp, attack, defence)
    {
        ClassName = classname;
        MaxHp = maxHp;
        Gold = gold;
        Xp = xp;
        Level = level;
        Potions = potions;

        Inventory = new List<string>() { "Wooden sword", "Cloth Armor" };
    }
    
    public void UsePotion()
    {
        if (Potions <= 0)
        {
            Console.WriteLine("Du har inga drycker kvar.");
            return;
        }
        

        // Helning av spelaren
        int heal = 12;
        int hp = Hp;
        int newHp = Math.Min(MaxHp, Hp + heal);
        Hp =  newHp;
        Potions -= 1;
        

        Console.WriteLine($"Du dricker en dryck och återfår {newHp - hp} HP.");
    }
    
    
    public void AddPotion()
    {
        Potions++;
    }

    public void AddWeapon()
    {
        Attack++;
    }

    public void AddArmor()
    {
        Defence++;
    }
    
    // Hantering av specialförmågor
    public virtual int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        int specialDmg = 0;
        

        // Dämpa skada mot bossen
        if (vsBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return Math.Max(0, specialDmg);
    }
    
    public void TakeDamage(int dmg)
    {
        Hp -= Math.Max(0, dmg);
        
        Hp = Math.Max(0, Hp);
        
    }
    
    public void AddPlayerXp(int amount)
    {
        int xp = Xp + Math.Max(0, amount);
        Xp = xp;
        MaybeLevelUp();
    }
    
    public void AddPlayerGold(int amount)
    {
        int gold = Gold  + Math.Max(0, amount);
        Gold = gold;
    }
    
    public void MaybeLevelUp()
    {
        // Nivåtrösklar
        
        int lvl = Level;
        int nextThreshold = lvl == 1 ? 10 : (lvl == 2 ? 25 : (lvl == 3 ? 45 : lvl * 20));

        if (Xp >= nextThreshold)
        {
            Level = lvl + 1;

            // Uppgradering baserad på karaktärsklass
            string cls = ClassName;

            switch (cls)
            {
                case "Warrior":
                    MaxHp += 6;
                    Attack += 2;
                    Defence += 2;
                    break;
                
                case "Mage":
                    MaxHp += 4;
                    Attack += 4; 
                    Defence += 1;
                    break;
                
                case "Rogue":
                    MaxHp += 5;
                    Attack += 3;
                    Defence += 1;
                    break;
                
                default:
                    MaxHp += 4;
                    Attack += 3;
                    Defence += 1;
                    break;
            }
            

            Console.WriteLine($"Du når nivå {lvl + 1}! Värden ökade och HP återställd.");
        }
    }
    
    // Används i TryBuy() för att kunna handla i DoShop()
    public bool TrySpendGold(int cost)
    {
        if (Gold >= cost)
        {
            Gold -= cost;
            return true;
        }

        return false;
    }
    
    public bool DoRest()
    {
        Console.WriteLine("Du slår läger och vilar.");
        
        int maxhp = MaxHp;
        Hp = maxhp;
        
        Console.WriteLine("HP återställt till max.");
        return true;
    }
}