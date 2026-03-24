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
        //int pot = ParseInt(Player[9], 0);
        
        if (Potions <= 0)
        {
            Console.WriteLine("Du har inga drycker kvar.");
            return;
        }
        //int hp = ParseInt(Player[2], 0);
        //int maxhp = ParseInt(Player[3], 1);
        

        // Helning av spelaren
        int heal = 12;
        int hp = Hp;
        int newHp = Math.Min(MaxHp, Hp + heal);
        Hp =  newHp;
        Potions -= 1;
        //int newHp = Math.Min(maxhp, hp + heal);
        //Player[2] = newHp.ToString();
        //Player[9] = (pot - 1).ToString();

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
    
    public virtual int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        //string cls = player.ClassName ?? "Warrior";
        int specialDmg = 0;

        // Hantering av specialförmågor
        //if (cls == "Warrior")
        //{
            // Heavy Strike: hög skada men självskada
            //Console.WriteLine("Warrior använder Heavy Strike!");
            //int atk = ParseInt(Player[4], 5);
            
            //specialDmg = Math.Max(2, player.Attack + 3 - enemyDef);
            //ApplyDamageToPlayer(2); // självskada
        //}
        //else if (cls == "Mage")
        //{
            // Fireball: stor skada, kostar guld
            //int gold = ParseInt(Player[6], 0);
            
            //if (player.Gold >= 3)
            //{
                //Console.WriteLine("Mage kastar Fireball!");
                //Player[6] = (gold - 3).ToString();
                //player.Gold -= 3;
                //int atk = ParseInt(Player[4], 5);
                //specialDmg = Math.Max(3, player.Attack + 5 - (enemyDef / 2));
            //}
            //else
            //{
              //  Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
                //specialDmg = 0;
            //}
        //}
        //else if (cls == "Rogue")
        //{
            // Backstab: chans att ignorera försvar, hög risk/hög belöning
          //  if (Rng.NextDouble() < 0.5)
            //{
              //  Console.WriteLine("Rogue utför en lyckad Backstab!");
                //int atk = ParseInt(Player[4], 5);
                //specialDmg = Math.Max(4, player.Attack + 6);
            //}
            //else
            //{
              //  Console.WriteLine("Backstab misslyckades!");
                //specialDmg = 1;
            //}
        //}
        //else
        //{
          //  specialDmg = 0;
        //}

        // Dämpa skada mot bossen
        if (vsBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return Math.Max(0, specialDmg);
    }
    
    public void TakeDamage(int dmg)
    {
        //int hp = ParseInt(Player[2], 0);
        Hp -= Math.Max(0, dmg);
        //Player[2] = Math.Max(0, hp).ToString();
        Hp = Math.Max(0, Hp);
        
    }
    
    public void AddPlayerXp(int amount)
    {
        //int xp = ParseInt(Player[7], 0) + Math.Max(0, amount);
        //Player[7] = xp.ToString();
        int xp = Xp + Math.Max(0, amount);
        Xp = xp;
        MaybeLevelUp();
    }
    
    public void AddPlayerGold(int amount)
    {
        //int gold = ParseInt(Player[6], 0) + Math.Max(0, amount);
        //Player[6] = gold.ToString();
        int gold = Gold  + Math.Max(0, amount);
        Gold = gold;
    }
    
    public void MaybeLevelUp()
    {
        // Nivåtrösklar
        //int xp = ParseInt(Player[7], 0);
        //int lvl = ParseInt(Player[8], 1);
        
        int lvl = Level;
        int nextThreshold = lvl == 1 ? 10 : (lvl == 2 ? 25 : (lvl == 3 ? 45 : lvl * 20));

        if (Xp >= nextThreshold)
        {
            //Player[8] = (lvl + 1).ToString();
            Level = lvl + 1;

            // Uppgradering baserad på karaktärsklass
            //string cls = Player[1] ?? "Warrior";
            //int maxhp = ParseInt(Player[3], 1);
            //int atk = ParseInt(Player[4], 1);
            //int def = ParseInt(Player[5], 0);
            string cls = ClassName;

            switch (cls)
            {
                case "Warrior":
                    //maxhp += 6; atk += 2; def += 2;
                    MaxHp += 6;
                    Attack += 2;
                    Defence += 2;
                    break;
                case "Mage":
                    //maxhp += 4; atk += 4; def += 1;
                    MaxHp += 4;
                    Attack += 4; 
                    Defence += 1;
                    break;
                case "Rogue":
                    //maxhp += 5; atk += 3; def += 1;
                    MaxHp += 5;
                    Attack += 3;
                    Defence += 1;
                    break;
                default:
                    //maxhp += 4; atk += 3; def += 1;
                    MaxHp += 4;
                    Attack += 3;
                    Defence += 1;
                    break;
            }

            //Player[3] = maxhp.ToString();
            //Player[4] = atk.ToString();
            //Player[5] = def.ToString();
            //Player[2] = maxhp.ToString(); // full heal vid level up
            

            Console.WriteLine($"Du når nivå {lvl + 1}! Värden ökade och HP återställd.");
        }
    }
    
    public bool TrySpendGold(int cost)
    {
        //int gold = ParseInt(Player[6], 0);
        if (Gold >= cost)
        {
            //Player[6] = (gold - cost).ToString();
            Gold -= cost;
            return true;
        }

        return false;
    }
    
    public bool DoRest()
    {
        Console.WriteLine("Du slår läger och vilar.");
        //int maxhp = ParseInt(Player[3], 1);
        //Player[2] = maxhp.ToString();
        int maxhp = MaxHp;
        Hp = maxhp;
        Console.WriteLine("HP återställt till max.");
        return true;
    }
}