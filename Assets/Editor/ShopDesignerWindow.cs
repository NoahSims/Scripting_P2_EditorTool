using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopDesignerWindow : EditorWindow
{
    private static ShopData shopData;
    public static ShopData ShopInfo { get { return shopData; } }

    private bool showInventory = true;
    int tempCapacity = 0;
    Vector2 scrollPos = new Vector2(0, 0);

    Texture2D shopInventorySectionTexture;
    Color shopInventorySectionColor = new Color(55f/255f, 32f/255f, 75f/255f, 0.8f);
    Rect shopInventorySection;

    //GUIStyle shopInventoryItemStyle = new GUIStyle(GUI.skin.label);
    //EditorStyles shopInventoryItemStyle = new EditorStyles(EditorStyles.)

    [MenuItem("Window/Shop Designer")]
    static void OpenWindow()
    {
        ShopDesignerWindow window = (ShopDesignerWindow)GetWindow(typeof(ShopDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitTextures();
        //InitStyles();
        InitData();
    }

    /*
    private void InitStyles()
    {
        shopInventoryItemStyle.margin = new RectOffset(20, 20, 0, 0);
    }
    */

    private void InitTextures()
    {
        shopInventorySectionTexture = new Texture2D(1, 1);
        shopInventorySectionTexture.SetPixel(0, 0, shopInventorySectionColor);
        shopInventorySectionTexture.Apply();
    }

    public static void InitData()
    {
        shopData = (ShopData)ScriptableObject.CreateInstance(typeof(ShopData));
        
    }

    private void OnGUI()
    {
        //GUILayout.BeginArea(shopSection);
        GUILayout.Label("Shop");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Shop Name");
        shopData.shopName = EditorGUILayout.TextField(shopData.shopName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Can buy items from player");
        GUILayout.FlexibleSpace();
        shopData.canBuyFromPlayer = EditorGUILayout.Toggle(shopData.canBuyFromPlayer);
        EditorGUILayout.EndHorizontal();

        if(shopData.canBuyFromPlayer)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Has limited Money");
            GUILayout.FlexibleSpace();
            shopData.hasLimitedMoney = EditorGUILayout.Toggle(shopData.hasLimitedMoney);
            EditorGUILayout.EndHorizontal();

            if (shopData.hasLimitedMoney)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Money Available");
                GUILayout.FlexibleSpace();
                shopData.money = EditorGUILayout.IntField(shopData.money);
                EditorGUILayout.EndHorizontal();
            }
        }


        /*
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("List");
        shopData.ShopList = EditorGUILayout.ObjectField(shopData.ShopList, typeof(List<LootableObject>));
        EditorGUILayout.EndHorizontal();
        
        
        SerializedObject sd = new SerializedObject(shopData);
        SerializedProperty shopInventoryList = sd.FindProperty("ShopList");
        EditorGUILayout.PropertyField(shopInventoryList, true);
        sd.ApplyModifiedProperties();
        */





        EditorGUILayout.BeginHorizontal();
        showInventory = EditorGUILayout.Foldout(showInventory, "Shop Inventory");
        GUILayout.FlexibleSpace();
        tempCapacity = EditorGUILayout.DelayedIntField(tempCapacity);
        tempCapacity = (int)Mathf.Clamp(tempCapacity, 0, int.MaxValue);
        EditorGUILayout.EndHorizontal();
        if (showInventory)
        {
            /*
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Size");
            
            tempCapacity = EditorGUILayout.DelayedIntField(tempCapacity);
            tempCapacity = (int)Mathf.Clamp(tempCapacity, 0, int.MaxValue);
            //shopData.ShopList.Capacity = tempCapacity;
            //Debug.Log("Quatntity = " + tempCapacity);
            EditorGUILayout.EndHorizontal();
            */

            int numToAdd = tempCapacity - shopData.ShopList.Count;
            if (numToAdd > 0)
            {
                for (int i = 0; i < numToAdd; i++)
                {
                    //shopData.ShopList.Add((ShopItemData)ScriptableObject.CreateInstance(typeof(ShopItemData)));
                    shopData.ShopList.Add(new ShopItemData());
                }
            }

            if(numToAdd < 0)
            {
                for (int i = numToAdd; i < 0; i++)
                {
                    shopData.ShopList.RemoveAt(shopData.ShopList.Count - 1);
                }
            }
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            
            for (int i = 0; i < shopData.ShopList.Count; i++)
            {
                //DrawInventoryLayout(i);
                //GUILayout.BeginArea(shopInventorySection);
                EditorGUILayout.BeginVertical("box");
                //EditorGUILayout.BeginHorizontal("box", GUILayout.Height(40f));
                //GUILayout.FlexibleSpace();
                //SerializedObject sid = new SerializedObject(shopData.ShopList[i]);
                //SerializedProperty shopItem = sid.FindProperty("shopItem");
                //LootableObject shopObject = (LootableObject)shopItem.objectReferenceValue;
                LootableObject shopObject = shopData.ShopList[i].shopItem;
                
                if (shopObject == null)
                {
                    GUILayout.Label("Enter an item");
                    //EditorGUILayout.PropertyField(shopItem, new GUIContent("Enter an item"), true);
                    shopData.ShopList[i].shopItem = (LootableObject)EditorGUILayout.ObjectField(shopData.ShopList[i].shopItem, typeof(LootableObject), false);
                }
                else
                {

                    //EditorGUILayout.PropertyField(shopItem, true);
                    shopData.ShopList[i].shopItem = (LootableObject)EditorGUILayout.ObjectField(shopData.ShopList[i].shopItem, typeof(LootableObject), false);

                    EditorGUILayout.BeginHorizontal();

                    // show item thumbnail
                    Rect itemData = EditorGUILayout.BeginVertical("box", GUILayout.Height(60), GUILayout.Width(60));
                    GUILayout.Label(" ");
                    Texture2D itemThumbnail = shopObject.thumbnail;
                    if (itemThumbnail != null)
                    {
                        GUI.DrawTexture(itemData, itemThumbnail, ScaleMode.ScaleToFit);
                    }
                    EditorGUILayout.EndVertical();

                    // item stats
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    //SerializedProperty shopItemHasLimitedQuantity = sid.FindProperty("HasLimitedQuantity");
                    //EditorGUILayout.PropertyField(shopItemHasLimitedQuantity, true);
                    //if (shopItemHasLimitedQuantity.boolValue)
                    GUILayout.Label("Has Limited Quantity");
                    shopData.ShopList[i].HasLimitedQuantity = EditorGUILayout.Toggle(shopData.ShopList[i].HasLimitedQuantity, GUILayout.Width(50));
                    if (shopData.ShopList[i].HasLimitedQuantity)
                    {
                        //SerializedProperty shopItemQuant = sid.FindProperty("Quantity");
                        //EditorGUILayout.PropertyField(shopItemQuant, true);
                        GUILayout.Label("Quantity");
                        shopData.ShopList[i].Quantity = EditorGUILayout.IntField(shopData.ShopList[i].Quantity, GUILayout.Width(130));

                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Base value: {" + shopObject.monetaryValue + "} * ");
                    //SerializedProperty shopItemSellMultiplier = sid.FindProperty("SellMultiplier");
                    //EditorGUILayout.PropertyField(shopItemSellMultiplier, true);
                    GUILayout.Label("Sell Multiplier      ");
                    shopData.ShopList[i].SellMultiplier = EditorGUILayout.FloatField(shopData.ShopList[i].SellMultiplier, GUILayout.Width(100));
                    //GUILayout.Label(" = " + Mathf.Round(shopObject.monetaryValue * shopItemSellMultiplier.floatValue));
                    GUILayout.Label(" = " + Mathf.Round(shopObject.monetaryValue * shopData.ShopList[i].SellMultiplier));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    if (shopData.canBuyFromPlayer)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Base value: {" + shopObject.monetaryValue + "} * ");
                        //SerializedProperty shopItemRefundMultiplier = sid.FindProperty("RefundMultiplier");
                        //EditorGUILayout.PropertyField(shopItemRefundMultiplier, true);
                        GUILayout.Label("Refund Multiplier");
                        shopData.ShopList[i].RefundMultiplier = EditorGUILayout.FloatField(shopData.ShopList[i].RefundMultiplier, GUILayout.Width(100));
                        //GUILayout.Label(" = -" + Mathf.Round(shopObject.monetaryValue * shopItemRefundMultiplier.floatValue));
                        GUILayout.Label(" = -" + Mathf.Round(shopObject.monetaryValue * shopData.ShopList[i].RefundMultiplier));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();

                    // list options
                    EditorGUILayout.BeginVertical("box");
                    if (GUILayout.Button("Move Up") && i != 0)
                    {
                        ShopItemData temp = shopData.ShopList[i - 1];
                        shopData.ShopList[i - 1] = shopData.ShopList[i];
                        shopData.ShopList[i] = temp;
                    }
                    if (GUILayout.Button("Move Down") && i != shopData.ShopList.Count - 1)
                    {
                        ShopItemData temp = shopData.ShopList[i + 1];
                        shopData.ShopList[i + 1] = shopData.ShopList[i];
                        shopData.ShopList[i] = temp;
                    }
                    if (GUILayout.Button("Clear"))
                    {
                        shopData.ShopList.RemoveAt(i);
                        tempCapacity--;
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();
                }
                //EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                //sid.ApplyModifiedProperties();
                //GUILayout.EndArea();
            }

            EditorGUILayout.EndScrollView();

        }
        

        if (GUILayout.Button("Save", GUILayout.Height(40)))
        {
            SaveShopData();
        }
    }

    void DrawInventoryLayout(int i)
    {
        //Rect lastRect = GUILayoutUtility.GetLastRect();
        shopInventorySection.x = 20;
        shopInventorySection.y = 40 * i;
        shopInventorySection.width = Screen.width - 40f;
        shopInventorySection.height = 40;

        GUI.DrawTexture(shopInventorySection, shopInventorySectionTexture);
    }

    void SaveShopData()
    {
        for (int i = 0; i < shopData.ShopList.Count; i++)
        {
            if(shopData.ShopList[i].shopItem == null)
            {
                Debug.Log("remove " + i);
                shopData.ShopList.RemoveAt(i);
                tempCapacity--;
                i--;
            }
        }

        string dataPath = "Assets/Resources/ShopData/Data/";
        dataPath += ShopInfo.shopName + ".asset";
        AssetDatabase.CreateAsset(ShopInfo, dataPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
