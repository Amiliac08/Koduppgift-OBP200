namespace OBP200_RolePlayingGame;

public class Warrior : Player
{
    public Warrior(string name) :
        base(name, 40, 7, 5, "Warrior", 40, 15, 0, 1, 2)
    {
        
    }

    public override int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        int specialDmg = 0;
        // Heavy Strike: hög skada men självskada
        Console.WriteLine("Warrior använder Heavy Strike!");
        //int atk = ParseInt(Player[4], 5);
            
        specialDmg = Math.Max(2, Attack + 3 - enemyDef);
        ApplyDamageToPlayer(2); // självskada
        
        
        return Math.Max(0, specialDmg);
    }
}