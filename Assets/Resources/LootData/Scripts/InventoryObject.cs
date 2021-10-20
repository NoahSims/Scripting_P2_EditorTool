using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : ScriptableObject
{
    [Header("InventoryObject")]
    public string displayName;
    public int monetaryValue;
    public string description;
    public Texture2D thumbnail;

    public virtual string PrintStats()
    {
        return "";
    }
}
