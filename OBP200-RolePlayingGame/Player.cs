namespace OBP200_RolePlayingGame;

public abstract class Player : Character
{
    // Random
    static Random Rng = new Random();
    
    public string ClassName { get; private set; }
    public int MaxHp { get; private set; }
    public int Gold { get; protected set; }
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
    
    public int ApplyDamageToPlayer(int dmg)
    {
        //int hp = ParseInt(Player[2], 0);
        Hp -= Math.Max(0, dmg);
        //Player[2] = Math.Max(0, hp).ToString();
        Hp = Math.Max(0, Hp);

        return dmg;
    }
}