namespace OBP200_RolePlayingGame;

public class Mage : Player
{
    public Mage(string name) :
        base(name, 28, 10, 2, "Mage", 28, 15, 0, 1, 2)
    {
        
    }

    public override int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        int specialDmg = 0;
        
        if (Gold >= 3)
        {
            Console.WriteLine("Mage kastar Fireball!");
            //Player[6] = (gold - 3).ToString();
            Gold -= 3;
            //int atk = ParseInt(Player[4], 5);
            specialDmg = Math.Max(3, Attack + 5 - (enemyDef / 2));
        }
        else
        {
            Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
            specialDmg = 0;
        }
        
        
        return Math.Max(0, specialDmg);
    }
}