using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData : ScriptableObject
{
    public string shopName;
    public bool canBuyFromPlayer = false;
    public bool hasLimitedMoney = false;
    public int money;

    public List<ShopItemData> ShopList = new List<ShopItemData>();

}

[System.Serializable]
public class ShopItemData : ScriptableObject
{
    public LootableObject shopItem = null;
    public int Quantity = -1;
    public float SellMultiplier = 1f;
    public float RefundMultiplier = 1f;

}