using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] List<Loot> lootList = new List<Loot>();


    public Loot GetLoot()
    {
        List<Loot> possibleLoot = new List<Loot>();
        foreach (Loot item in lootList)
        {
            int randomNumber = Random.Range(1, 101);
            if (item.chance >= randomNumber)
            {
                possibleLoot.Add(item);
            }
        }
        if (possibleLoot.Count > 0)
        {
            Loot lowestChanceItem = possibleLoot[0];
            for (int i = 0; i < possibleLoot.Count; i++)
            {
                if (possibleLoot[i].chance < lowestChanceItem.chance)
                {
                    lowestChanceItem = possibleLoot[i];
                }
            }
            return lowestChanceItem;
        }
        else return null;
    }
}
