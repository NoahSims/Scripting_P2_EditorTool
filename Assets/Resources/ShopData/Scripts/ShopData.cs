using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : ScriptableObject
{
    public string shopName;
    public bool canBuyFromPlayer = false;
    public bool hasLimitedMoney = false;
    public int money;

    public List<LootableObject> ShopList = new List<LootableObject>();
    public Dictionary<LootableObject, int> ShopInventory = new Dictionary<LootableObject, int>();

}
