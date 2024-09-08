using System;
using System.Collections.Generic;
using System.Linq;
using Wizards.People;

namespace AbilityExample;

public class Helpers
{
    private static void FindCostumes(CharacterManager me, List<CharacterNames> resultList)
    {
        foreach (var chara in resultList)
        {
            foreach (var Cos in me.GetCharacterPrefab(chara).myArtPrefab.costumeList)
            {
                Console.WriteLine(chara);
                Console.WriteLine(Cos.displayName);
                Console.WriteLine(Cos.saveName);
                Console.WriteLine(Cos.unlockCost);
            }
        }
    }

    public static void FindAllAbilities()
    {
        var type = typeof(AbilitySO);
        var abilities = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(type))
            .Select(t => Activator.CreateInstance(t))
            .OfType<AbilitySO>();

        foreach (var ability in abilities)
        {
            Console.WriteLine($"Found ability instance of type: {ability.GetType().Name}");
        }
    }
    
    public static bool IsAdjacent(GridPosition pos1, GridPosition pos2)
    {
        int dx = Math.Abs(pos1.x - pos2.x);
        int dz = Math.Abs(pos1.z - pos2.z);

        return (dx <= 1 && dz <= 1) && !(dx == 0 && dz == 0);
    }
    
    public static List<GridPosition> GetAdjacentPositions(GridPosition fromPosition)
    {
        List<GridPosition> adjacentPositions = new List<GridPosition>();

        int x = fromPosition.x;
        int z = fromPosition.z;

        adjacentPositions.Add(new GridPosition(x + 1, z));   // Right
        adjacentPositions.Add(new GridPosition(x - 1, z));   // Left
        adjacentPositions.Add(new GridPosition(x, z + 1));   // Up
        adjacentPositions.Add(new GridPosition(x, z - 1));   // Down
        adjacentPositions.Add(new GridPosition(x + 1, z + 1)); // Top-right diagonal
        adjacentPositions.Add(new GridPosition(x - 1, z - 1)); // Bottom-left diagonal
        adjacentPositions.Add(new GridPosition(x + 1, z - 1)); // Bottom-right diagonal
        adjacentPositions.Add(new GridPosition(x - 1, z + 1)); // Top-left diagonal

        return adjacentPositions;
    }




}