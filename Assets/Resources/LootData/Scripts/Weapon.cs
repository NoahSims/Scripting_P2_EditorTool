using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Weapon", menuName = "Lootable Object/ Weapon")]
public class Weapon : LootableObject
{
    public WeaponTypes weaponType;
    public DamageTypes damageType;
    public ElementalTypes elementalType;
    public int damage;
    public int effectiveRange;

    public override string PrintStats()
    {
        string stats = "";

        stats += "Damage:   " + damage + "\n";
        stats += "Range:    " + effectiveRange + "\n";
        if (elementalType != ElementalTypes.None)
            stats += "Effect:   " + elementalType + "\n";
        stats += "Type:     " + weaponType + "\n";
        stats += "Dmg Type: " + damageType + "\n";
        
        return stats;
    }
}

public enum WeaponTypes { Melee, Ranged, Magic};
public enum DamageTypes { Force, Piercing, Slashing, Magic};
public enum ElementalTypes { None, Fire, Frost, Poison};
