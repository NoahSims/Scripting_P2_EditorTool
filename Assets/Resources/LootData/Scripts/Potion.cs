using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Potion", menuName = "Lootable Object/ Potion")]
public class Potion : InventoryObject
{
    [Header("Potion")]
    public PotionEffects potionEffect;
    public int potionStrength;

    public override string PrintStats()
    {
        string stats = "";

        stats += "Effect: " + potionEffect + "\n";
        stats += "Strength: " + potionStrength + "\n";

        return stats;
    }
}

public enum PotionEffects { Healing, Mana};
