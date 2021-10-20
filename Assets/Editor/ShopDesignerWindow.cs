using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopDesignerWindow : EditorWindow
{
    // the shop being created
    private static ShopData shopData;
    public static ShopData ShopInfo { get { return shopData; } }

    private static ShopData editingShop;
    private bool isEditingShop = false;

    private bool showInventory = true; // for the foldout menu on the inventory list
    private static int shopInventorySize = 0; // used to set the size of the inventory list
    Vector2 scrollPos = new Vector2(0, 0); // used for scrolling through the inventory list

    string assetDataPath;

    //Texture2D shopInventorySectionTexture;
    //Color shopInventorySectionColor = new Color(55f/255f, 32f/255f, 75f/255f, 0.8f);

    [MenuItem("Window/Shop Designer")]
    static void OpenWindow()
    {
        ShopDesignerWindow window = (ShopDesignerWindow)GetWindow(typeof(ShopDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        //InitTextures();
        InitData();
    }

    /*
    private void InitTextures()
    {
        shopInventorySectionTexture = new Texture2D(1, 1);
        shopInventorySectionTexture.SetPixel(0, 0, shopInventorySectionColor);
        shopInventorySectionTexture.Apply();
    }
    */

    public static void InitData()
    {
        shopData = (ShopData)ScriptableObject.CreateInstance(typeof(ShopData));
        editingShop = null;
        shopInventorySize = 0;
    }

    private void OnGUI()
    {
        // Title
        GUILayout.Label("Shop");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Edit Existing Shop");
        GUILayout.FlexibleSpace();
        editingShop = (ShopData)EditorGUILayout.ObjectField(editingShop, typeof(ShopData), false);
        if(editingShop != null)
        {
            if (!isEditingShop)
            {
                isEditingShop = true;
                ScriptableObject.DestroyImmediate(shopData);
            }
            shopData = editingShop;
            shopInventorySize = shopData.ShopInventoryList.Count;
        }
        else if(editingShop == null && isEditingShop)
        {
            isEditingShop = false;
            shopData = (ShopData)ScriptableObject.CreateInstance(typeof(ShopData));
        }
        EditorGUILayout.EndHorizontal();

        // Get Shop Name
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Shop Name");
        shopData.shopName = EditorGUILayout.TextField(shopData.shopName);
        EditorGUILayout.EndHorizontal();

        // Get canBuyFromPlayer
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Can buy items from player");
        GUILayout.FlexibleSpace();
        shopData.canBuyFromPlayer = EditorGUILayout.Toggle(shopData.canBuyFromPlayer);
        EditorGUILayout.EndHorizontal();

        // Get HasLimitedMoney
        if (shopData.canBuyFromPlayer)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Has limited Money");
            GUILayout.FlexibleSpace();
            shopData.hasLimitedMoney = EditorGUILayout.Toggle(shopData.hasLimitedMoney);
            EditorGUILayout.EndHorizontal();

            // Get Shop's Available Money
            if (shopData.hasLimitedMoney)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Money Available");
                GUILayout.FlexibleSpace();
                shopData.money = EditorGUILayout.IntField(shopData.money);
                EditorGUILayout.EndHorizontal();
            }
        }

        // Shop Inventory List
        EditorGUILayout.BeginHorizontal();
        {
            // Foldout
            showInventory = EditorGUILayout.Foldout(showInventory, "Shop Inventory");
            GUILayout.FlexibleSpace();
            // Get Inventory Size
            shopInventorySize = EditorGUILayout.DelayedIntField(shopInventorySize);
            shopInventorySize = (int)Mathf.Clamp(shopInventorySize, 0, int.MaxValue);
        } EditorGUILayout.EndHorizontal();
        if (showInventory)
        {
            // if shopInventorySize is > shopInventoryList.count; add new elements to the Inventory List
            int numToAdd = shopInventorySize - shopData.ShopInventoryList.Count;
            if (numToAdd > 0)
            {
                for (int i = 0; i < numToAdd; i++)
                {
                    shopData.ShopInventoryList.Add(new ShopItemData());
                }
            }

            // if shopInventorySize is < shopInventoryList.count; remove elements from end of Inventory List
            if (numToAdd < 0)
            {
                for (int i = numToAdd; i < 0; i++)
                {
                    shopData.ShopInventoryList.RemoveAt(shopData.ShopInventoryList.Count - 1);
                }
            }
            
            // Start Scroll Area
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < shopData.ShopInventoryList.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                
                // if InventoryList slot is missing an item, request an item
                if (shopData.ShopInventoryList[i].shopItem == null)
                {
                    GUILayout.Label("Enter an item");
                    shopData.ShopInventoryList[i].shopItem = (InventoryObject)EditorGUILayout.ObjectField(shopData.ShopInventoryList[i].shopItem, typeof(InventoryObject), false);
                }
                // else, if not missing object, display shopItemData object
                else
                {
                    shopData.ShopInventoryList[i].shopItem = (InventoryObject)EditorGUILayout.ObjectField(shopData.ShopInventoryList[i].shopItem, typeof(InventoryObject), false);

                    EditorGUILayout.BeginHorizontal();
                    {

                        // show item thumbnail
                        Rect itemData = EditorGUILayout.BeginVertical("box", GUILayout.Height(60), GUILayout.Width(60));
                        GUILayout.Label(" ");
                        Texture2D itemThumbnail = shopData.ShopInventoryList[i].shopItem.thumbnail;
                        if (itemThumbnail != null)
                        {
                            GUI.DrawTexture(itemData, itemThumbnail, ScaleMode.ScaleToFit);
                        } EditorGUILayout.EndVertical();

                        // shopItemData values
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                // Get HasLimitedQuantity
                                GUILayout.Label("Has Limited Quantity");
                                shopData.ShopInventoryList[i].HasLimitedQuantity = EditorGUILayout.Toggle(shopData.ShopInventoryList[i].HasLimitedQuantity, GUILayout.Width(50));
                                // Get Quantity
                                if (shopData.ShopInventoryList[i].HasLimitedQuantity)
                                {
                                    GUILayout.Label("Quantity");
                                    shopData.ShopInventoryList[i].Quantity = EditorGUILayout.IntField(shopData.ShopInventoryList[i].Quantity, GUILayout.Width(130));

                                }
                                GUILayout.FlexibleSpace();
                            } EditorGUILayout.EndHorizontal();

                            // Get and display Sell Multiplier
                            EditorGUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("Base value: {" + shopData.ShopInventoryList[i].shopItem.monetaryValue + "} * ");
                                GUILayout.Label("Sell Multiplier      ");
                                shopData.ShopInventoryList[i].SellMultiplier = EditorGUILayout.FloatField(shopData.ShopInventoryList[i].SellMultiplier, GUILayout.Width(100));
                                GUILayout.Label(" = " + Mathf.Round(shopData.ShopInventoryList[i].shopItem.monetaryValue * shopData.ShopInventoryList[i].SellMultiplier));
                                GUILayout.FlexibleSpace();
                            } EditorGUILayout.EndHorizontal();

                            // Get and display Refund Multiplier
                            if (shopData.canBuyFromPlayer)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label("Base value: {" + shopData.ShopInventoryList[i].shopItem.monetaryValue + "} * ");
                                GUILayout.Label("Refund Multiplier");
                                shopData.ShopInventoryList[i].RefundMultiplier = EditorGUILayout.FloatField(shopData.ShopInventoryList[i].RefundMultiplier, GUILayout.Width(100));
                                GUILayout.Label(" = -" + Mathf.Round(shopData.ShopInventoryList[i].shopItem.monetaryValue * shopData.ShopInventoryList[i].RefundMultiplier));
                                GUILayout.FlexibleSpace();
                                EditorGUILayout.EndHorizontal();
                            }

                        } EditorGUILayout.EndVertical();

                        // list options
                        EditorGUILayout.BeginVertical("box");
                        {
                            if (GUILayout.Button("Move Up") && i != 0)
                            {
                                ShopItemData temp = shopData.ShopInventoryList[i - 1];
                                shopData.ShopInventoryList[i - 1] = shopData.ShopInventoryList[i];
                                shopData.ShopInventoryList[i] = temp;
                            }
                            if (GUILayout.Button("Move Down") && i != shopData.ShopInventoryList.Count - 1)
                            {
                                ShopItemData temp = shopData.ShopInventoryList[i + 1];
                                shopData.ShopInventoryList[i + 1] = shopData.ShopInventoryList[i];
                                shopData.ShopInventoryList[i] = temp;
                            }
                            if (GUILayout.Button("Clear"))
                            {
                                shopData.ShopInventoryList.RemoveAt(i);
                                shopInventorySize--;
                            }
                        } EditorGUILayout.EndVertical();

                    } EditorGUILayout.EndHorizontal();
                } 
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

        }

        if (!isEditingShop)
        {
            if (shopData.shopName == null || shopData.shopName.Length < 1)
            {
                EditorGUILayout.HelpBox("This shop needs a [Shop Name] before it can be saved", MessageType.Warning);
            }
            else if (GUILayout.Button("Save", GUILayout.Height(40)))
            {
                SaveShopData();
            }
        }
        else
        {
            if(GUILayout.Button("Begin New Shop", GUILayout.Height(40)))
            {
                isEditingShop = false;
                InitData();
            }
        }
    }

    /*
    void DrawInventoryLayout(int i)
    {
        //Rect lastRect = GUILayoutUtility.GetLastRect();
        shopInventorySection.x = 20;
        shopInventorySection.y = 40 * i;
        shopInventorySection.width = Screen.width - 40f;
        shopInventorySection.height = 40;

        GUI.DrawTexture(shopInventorySection, shopInventorySectionTexture);
    }
    */

    void SaveShopData()
    {
        for (int i = 0; i < shopData.ShopInventoryList.Count; i++)
        {
            if(shopData.ShopInventoryList[i].shopItem == null)
            {
                Debug.Log("remove " + i);
                shopData.ShopInventoryList.RemoveAt(i);
                shopInventorySize--;
                i--;
            }
        }

        assetDataPath = "Assets/Resources/ShopData/Data/";
        assetDataPath += ShopInfo.shopName + ".asset";
        AssetDatabase.CreateAsset(ShopInfo, assetDataPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Asset saved to: " + assetDataPath);
        editingShop = (ShopData)AssetDatabase.LoadAssetAtPath(assetDataPath, typeof(ShopData));
        isEditingShop = true;
    }
}
