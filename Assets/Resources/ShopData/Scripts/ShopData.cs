using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : ScriptableObject
{
    public string shopName;
    public bool canBuyFromPlayer;
    public bool hasLimitedMoney;
    public int money;

    public List<ShopItemData> ShopList = new List<ShopItemData>();

}
