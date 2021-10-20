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
        shopData.canBuyFromPlayer = EditorGUILayout.Toggle(shopData.canBuyFromPlayer);
        EditorGUILayout.EndHorizontal();

        if(shopData.canBuyFromPlayer)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Has limited Money");
            shopData.hasLimitedMoney = EditorGUILayout.Toggle(shopData.hasLimitedMoney);
            EditorGUILayout.EndHorizontal();

            if (shopData.hasLimitedMoney)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Money Available");
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
        
        

        
        showInventory = EditorGUILayout.Foldout(showInventory, "Shop Inventory");
        if (showInventory)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Size");
            
            tempCapacity = EditorGUILayout.DelayedIntField(tempCapacity);
            tempCapacity = (int)Mathf.Clamp(tempCapacity, 0, int.MaxValue);
            //shopData.ShopList.Capacity = tempCapacity;
            //Debug.Log("Quatntity = " + tempCapacity);
            EditorGUILayout.EndHorizontal();

            int numToAdd = tempCapacity - shopData.ShopList.Count;
            if (numToAdd > 0)
            {
                for (int i = 0; i < numToAdd; i++)
                {
                    shopData.ShopList.Add((ShopItemData)ScriptableObject.CreateInstance(typeof(ShopItemData)));
                    //shopData.ShopList.Add(new ShopItemData());
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
                SerializedObject sid = new SerializedObject(shopData.ShopList[i]);
                SerializedProperty shopItem = sid.FindProperty("shopItem");
                LootableObject shopObject = (LootableObject)shopItem.objectReferenceValue;
                
                if (shopObject == null)
                {
                    //GUILayout.Label("Enter an item");
                    EditorGUILayout.PropertyField(shopItem, new GUIContent("Enter an item"), true);
                }
                else
                {
                    
                    EditorGUILayout.PropertyField(shopItem, true);

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
                    SerializedProperty shopItemHasLimitedQuantity = sid.FindProperty("HasLimitedQuantity");
                    EditorGUILayout.PropertyField(shopItemHasLimitedQuantity, true);
                    if (shopItemHasLimitedQuantity.boolValue)
                    {
                        SerializedProperty shopItemQuant = sid.FindProperty("Quantity");
                        EditorGUILayout.PropertyField(shopItemQuant, true);
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Base value: {" + shopObject.monetaryValue + "} * ");
                    SerializedProperty shopItemSellMultiplier = sid.FindProperty("SellMultiplier");
                    EditorGUILayout.PropertyField(shopItemSellMultiplier, true);
                    GUILayout.Label(" = " + Mathf.Round(shopObject.monetaryValue * shopItemSellMultiplier.floatValue));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    if (shopData.canBuyFromPlayer)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Base value: {" + shopObject.monetaryValue + "} * ");
                        SerializedProperty shopItemRefundMultiplier = sid.FindProperty("RefundMultiplier");
                        EditorGUILayout.PropertyField(shopItemRefundMultiplier, true);
                        GUILayout.Label(" = -" + Mathf.Round(shopObject.monetaryValue * shopItemRefundMultiplier.floatValue));
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
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();
                }
                //EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                sid.ApplyModifiedProperties();
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
        string dataPath = "Assets/Resources/ShopData/Data/";
        dataPath += ShopInfo.shopName + ".asset";
        AssetDatabase.CreateAsset(ShopInfo, dataPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
