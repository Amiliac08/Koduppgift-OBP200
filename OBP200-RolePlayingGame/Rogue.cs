namespace OBP200_RolePlayingGame;

public class Rogue : Player
{
    // Random
    static Random Rng = new Random();
    
    public Rogue(string name) :
        base(name, 32, 8, 3, "Rogue", 32, 20, 0, 1, 3)
    {
        
    }

    public override int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        int specialDmg = 0;
        // Backstab: chans att ignorera försvar, hög risk/hög belöning
        if (Rng.NextDouble() < 0.5)
        {
            Console.WriteLine("Rogue utför en lyckad Backstab!");
            
            specialDmg = Math.Max(4, Attack + 6);
        }
        else
        {
            Console.WriteLine("Backstab misslyckades!");
            specialDmg = 1;
        }
        
        
        return Math.Max(0, specialDmg);
    }
}