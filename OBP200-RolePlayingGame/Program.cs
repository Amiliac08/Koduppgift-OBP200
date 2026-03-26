using System.Text;

namespace OBP200_RolePlayingGame;


class Program
{
    // ======= Globalt tillstånd  =======

    
    // Rum: [type, label]
    // types: battle, treasure, shop, rest, boss
    static List<string[]> Rooms = new List<string[]>();

    // Fiendemallar: [type, name, HP, ATK, DEF, XPReward, GoldReward]
    static List<string[]> EnemyTemplates = new List<string[]>();

    // Status för kartan
    static int CurrentRoomIndex = 0;

    // Random
    static Random Rng = new Random();

    // ======= Main =======

    static void Main(string[] args)
    {
        
        Console.OutputEncoding = Encoding.UTF8;
        InitEnemyTemplates();

        

        while (true)
        {
            ShowMainMenu();
            Console.Write("Välj: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            if (choice == "1")
            {
                Player player = StartNewGame();
                RunGameLoop(player);
            }
            else if (choice == "2")
            {
                Console.WriteLine("Avslutar...");
                return;
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }

            Console.WriteLine();
        }
    }

    // ======= Meny & Init =======

    static void ShowMainMenu()
    {
        Console.WriteLine("=== Text-RPG ===");
        Console.WriteLine("1. Nytt spel");
        Console.WriteLine("2. Avsluta");
    }

    static Player StartNewGame()
    {
        Console.Write("Ange namn: ");
        var name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name)) name = "Namnlös";

        Console.WriteLine("Välj klass: 1) Warrior  2) Mage  3) Rogue");
        Console.Write("Val: ");
        var k = (Console.ReadLine() ?? "").Trim();

        

        Player player;
        
        // Val av spelare
        switch (k)
        {
            case "1": 
                player = new Warrior(name);
                break;
            
            case "2": 
                player = new Mage(name);
                break;
                
            case "3": 
                player = new Rogue(name);
                break;
                
            default:
                player = new Warrior(name);
                break;
                
        }
        

        // Initiera karta (linjärt äventyr)
        Rooms.Clear();
        Rooms.Add(new[] { "battle", "Skogsstig" });
        Rooms.Add(new[] { "treasure", "Gammal kista" });
        Rooms.Add(new[] { "shop", "Vandrande köpman" });
        Rooms.Add(new[] { "battle", "Grottans mynning" });
        Rooms.Add(new[] { "rest", "Lägereld" });
        Rooms.Add(new[] { "battle", "Grottans djup" });
        Rooms.Add(new[] { "boss", "Urdraken" });

        CurrentRoomIndex = 0;

        Console.WriteLine($"Välkommen, {name} the {player.ClassName}!");
        ShowStatus(player);
        
        return player;
    }

    static void RunGameLoop(Player player)
    {
        while (true)
        {
            var room = Rooms[CurrentRoomIndex];
            Console.WriteLine($"--- Rum {CurrentRoomIndex + 1}/{Rooms.Count}: {room[1]} ({room[0]}) ---");

            bool continueAdventure = EnterRoom(room[0], player);
            
            if (IsPlayerDead(player))
            {
                Console.WriteLine("Du har stupat... Spelet över.");
                break;
            }
            
            if (!continueAdventure)
            {
                Console.WriteLine("Du lämnar äventyret för nu.");
                break;
            }

            CurrentRoomIndex++;
            
            if (CurrentRoomIndex >= Rooms.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Du har klarat äventyret!");
                break;
            }
            
            Console.WriteLine();
            Console.WriteLine("[C] Fortsätt     [Q] Avsluta till huvudmeny");
            Console.Write("Val: ");
            var post = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (post == "Q")
            {
                Console.WriteLine("Tillbaka till huvudmenyn.");
                break;
            }

            Console.WriteLine();
        }
    }

    // ======= Rumshantering =======

    static bool EnterRoom(string type, Player player)
    {
        switch ((type ?? "battle").Trim())
        {
            case "battle":
                return DoBattle(isBoss: false, player);
            case "boss":
                return DoBattle(isBoss: true, player);
            case "treasure":
                return DoTreasure(player);
            case "shop":
                return DoShop(player);
            case "rest":
                return player.DoRest();
            default:
                Console.WriteLine("Du vandrar vidare...");
                return true;
        }
    }

    // ======= Strid =======

    static bool DoBattle(bool isBoss, Player player)
    {
        
        var enemy = GenerateEnemy(isBoss);
        Console.WriteLine($"En {enemy[1]} dyker upp! (HP {enemy[2]}, ATK {enemy[3]}, DEF {enemy[4]})");

        int enemyHp = ParseInt(enemy[2], 10);
        int enemyAtk = ParseInt(enemy[3], 3);
        int enemyDef = ParseInt(enemy[4], 0);

        while (enemyHp > 0 && !IsPlayerDead(player))
        {
            Console.WriteLine();
            ShowStatus(player);
            Console.WriteLine($"Fiende: {enemy[1]} HP={enemyHp}");
            Console.WriteLine("[A] Attack   [X] Special   [P] Dryck   [R] Fly");
            if (isBoss) Console.WriteLine("(Du kan inte fly från en boss!)");
            Console.Write("Val: ");

            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (cmd == "A")
            {
                int damage = CalculatePlayerDamage(enemyDef, player);
                enemyHp -= damage;
                Console.WriteLine($"Du slog {enemy[1]} för {damage} skada.");
            }
            else if (cmd == "X")
            {
                int special = player.UseClassSpecial(enemyDef, isBoss);
                enemyHp -= special;
                Console.WriteLine($"Special! {enemy[1]} tar {special} skada.");
            }
            else if (cmd == "P")
            {
                player.UsePotion();
            }
            else if (cmd == "R" && !isBoss)
            {
                if (TryRunAway(player))
                {
                    Console.WriteLine("Du flydde!");
                    return true; // fortsätt äventyr
                }
                else
                {
                    Console.WriteLine("Misslyckad flykt!");
                }
            }
            else
            {
                Console.WriteLine("Du tvekar...");
            }

            if (enemyHp <= 0) break;

            // Fiendens tur
            int enemyDamage = CalculateEnemyDamage(enemyAtk, player);
            player.TakeDamage(enemyDamage);
            Console.WriteLine($"{enemy[1]} anfaller och gör {enemyDamage} skada!");
        }

        if (IsPlayerDead(player))
        {
            return false; // avsluta äventyr
        }

        // Vinstrapporter, XP, guld, loot
        int xpReward = ParseInt(enemy[5], 5);
        int goldReward = ParseInt(enemy[6], 3);

        player.AddPlayerXp(xpReward);
        player.AddPlayerGold(goldReward);

        Console.WriteLine($"Seger! +{xpReward} XP, +{goldReward} guld.");
        MaybeDropLoot(enemy[1], player);

        return true;
    }

    static string[] GenerateEnemy(bool isBoss)
    {
        if (isBoss)
        {
            // Boss-mall
            return new[] { "boss", "Urdraken", "55", "9", "4", "30", "50" };
        }
        else
        {
            // Slumpa bland templates
            var template = EnemyTemplates[Rng.Next(EnemyTemplates.Count)];
            
            // Slmumpmässig justering av stats
            int hp = ParseInt(template[2], 10) + Rng.Next(-1, 3);
            int atk = ParseInt(template[3], 3) + Rng.Next(0, 2);
            int def = ParseInt(template[4], 0) + Rng.Next(0, 2);
            int xp = ParseInt(template[5], 4) + Rng.Next(0, 3);
            int gold = ParseInt(template[6], 2) + Rng.Next(0, 3);
            return new[] { template[0], template[1], hp.ToString(), atk.ToString(), def.ToString(), xp.ToString(), gold.ToString() };
        }
    }

    static void InitEnemyTemplates()
    {
        EnemyTemplates.Clear();
        EnemyTemplates.Add(new[] { "beast", "Vildsvin", "18", "4", "1", "6", "4" });
        EnemyTemplates.Add(new[] { "undead", "Skelett", "20", "5", "2", "7", "5" });
        EnemyTemplates.Add(new[] { "bandit", "Bandit", "16", "6", "1", "8", "6" });
        EnemyTemplates.Add(new[] { "slime", "Geléslem", "14", "3", "0", "5", "3" });
    }

    static int CalculatePlayerDamage(int enemyDef, Player player)
    {
        
        int atk = player.Attack;
        string cls = player.ClassName;

        // Beräkna grundskada
        int baseDmg = Math.Max(1, atk - (enemyDef / 2));
        int roll = Rng.Next(0, 3); // liten variation

        switch (cls.Trim())
        {
            case "Warrior":
                baseDmg += 1; // warrior buff
                break;
            case "Mage":
                baseDmg += 2; // mage buff
                break;
            case "Rogue":
                baseDmg += (Rng.NextDouble() < 0.2) ? 4 : 0; // rogue crit-chans
                break;
            default:
                baseDmg += 0;
                break;
        }

        return Math.Max(1, baseDmg + roll);
    }
    

    static int CalculateEnemyDamage(int enemyAtk, Player player)
    {
        
        int roll = Rng.Next(0, 3);

        int dmg = Math.Max(1, enemyAtk - (player.Defence / 2)) + roll;

        // Liten chans till "glancing blow" (minskad skada)
        if (Rng.NextDouble() < 0.1) dmg = Math.Max(1, dmg - 2);

        return dmg;
    }
    

    static bool TryRunAway(Player player)
    {
        // Flyktschans baserad på karaktärsklass
        
        double chance = 0.25;
        if (player.ClassName == "Rogue") chance = 0.5;
        if (player.ClassName == "Mage") chance = 0.35;
        return Rng.NextDouble() < chance;
    }
    
    static bool IsPlayerDead(Player player)
    {
        if (player.Hp <= 0)
        {
            return true;
        }
        return false;
    }
    

    static void MaybeDropLoot(string enemyName, Player player)
    {
        // Enkel loot-regel
        if (Rng.NextDouble() < 0.35)
        {
            string item = "Minor Gem";
            if (enemyName.Contains("Urdraken")) item = "Dragon Scale";

            
            player.Inventory.Add(item);
            

            Console.WriteLine($"Föremål hittat: {item} (lagt i din väska)");
        }
    }

    // ======= Rumshändelser =======

    static bool DoTreasure(Player player)
    {
        Console.WriteLine("Du hittar en gammal kista...");
        if (Rng.NextDouble() < 0.5)
        {
            int gold = Rng.Next(8, 15);
            player.AddPlayerGold(gold);
            Console.WriteLine($"Kistan innehåller {gold} guld!");
        }
        else
        {
            var items = new[] { "Iron Dagger", "Oak Staff", "Leather Vest", "Healing Herb" };
            string found = items[Rng.Next(items.Length)];
            
            player.Inventory.Add(found);
            Console.WriteLine($"Du plockar upp: {found}");
        }
        return true;
    }

    static bool DoShop(Player player)
    {
        Console.WriteLine("En vandrande köpman erbjuder sina varor:");
        while (true)
        {
            Console.WriteLine($"Guld: {player.Gold} | Drycker: {player.Potions}");
            Console.WriteLine("1) Köp dryck (10 guld)");
            Console.WriteLine("2) Köp vapen (+2 ATK) (25 guld)");
            Console.WriteLine("3) Köp rustning (+2 DEF) (25 guld)");
            Console.WriteLine("4) Sälj alla 'Minor Gem' (+5 guld/st)");
            Console.WriteLine("5) Lämna butiken");
            Console.Write("Val: ");
            var val = (Console.ReadLine() ?? "").Trim();

            if (val == "1")
            {
                TryBuy(10, () => player.AddPotion(), "Du köper en dryck.", player);
            }
            else if (val == "2")
            {
                TryBuy(25, () => player.AddWeapon(), "Du köper ett bättre vapen.", player);
            }
            else if (val == "3")
            {
                TryBuy(25, () => player.AddArmor(), "Du köper bättre rustning.", player);
            }
            else if (val == "4")
            {
                SellMinorGems(player, "Minor Gem");
            }
            else if (val == "5")
            {
                Console.WriteLine("Du säger adjö till köpmannen.");
                break;
            }
            else
            {
                Console.WriteLine("Köpmannen förstår inte ditt val.");
            }
        }
        return true;
    }
    
    static void TryBuy(int cost, Action apply, string successMsg, Player player)
    {
        
        if (player.TrySpendGold(cost))
        {
            apply();
            Console.WriteLine(successMsg);
        }
        else
        {
            Console.WriteLine("Du har inte råd.");
        }
    }

    static void SellMinorGems(Player player, string item)
    {
        
        int count = player.Inventory.Count(x => x == "Minor Gem");
       
        if(!player.Inventory.Contains("Minor Gem"))
        {
            Console.WriteLine("Inga 'Minor Gem' i väskan.");
            return;
        }
        
        player.Inventory.RemoveAll(x => x == "Minor Gem");

        player.AddPlayerGold(count * 5);
        Console.WriteLine($"Du säljer {count} st Minor Gem för {count * 5} guld.");
    }
    

    // ======= Status =======

    static void ShowStatus(Player player)
    {
        Console.WriteLine($"[{player.Name} | {player.ClassName}] HP {player.Hp}/{player.MaxHp} ATK {player.Attack} DEF {player.Defence} LVL {player.Level} XP {player.Xp} Guld {player.Gold} Drycker {player.Potions}");
        
        if (player.Inventory != null)
        {
            foreach (var item in player.Inventory)
            {
                Console.WriteLine(item);
            }
        }
    }
    
    // ======= Hjälpmetoder =======

    static int ParseInt(string s, int fallback)
    {
        try
        {
            int value = Convert.ToInt32(s);
            return value;
        }
        catch (Exception e)
        {
            return fallback;
        }
    }
}
