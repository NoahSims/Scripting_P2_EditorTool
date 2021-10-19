using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopDesignerWindow : EditorWindow
{
    private static ShopData shopData;
    public static ShopData ShopInfo { get { return shopData; } }

    private bool showInventory = true;

    [MenuItem("Window/Shop Designer")]
    static void OpenWindow()
    {
        ShopDesignerWindow window = (ShopDesignerWindow)GetWindow(typeof(ShopDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitData();
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
        GUILayout.Label("ShopName");
        shopData.shopName = EditorGUILayout.TextField(shopData.name);
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
        }

        if(shopData.hasLimitedMoney)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Money Available");
            shopData.money = EditorGUILayout.IntField(shopData.money);
            EditorGUILayout.EndHorizontal();
        }
        /*
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("List");
        shopData.ShopList = EditorGUILayout.ObjectField(shopData.ShopList, typeof(List<LootableObject>));
        EditorGUILayout.EndHorizontal();
        */

        SerializedObject sd = new SerializedObject(shopData);
        SerializedProperty shopInventoryList = sd.FindProperty("ShopList");
        EditorGUILayout.PropertyField(shopInventoryList, true);
        sd.ApplyModifiedProperties();

        SerializedProperty test = sd.FindProperty("ShopInventory");
        EditorGUILayout.PropertyField(test, true);

        /*
        showInventory = EditorGUILayout.Foldout(showInventory, "Shop Inventory");
        if (showInventory)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Size");
            int tempCapacity = EditorGUILayout.DelayedIntField(shopData.ShopList.Capacity);
            shopData.ShopList.Capacity = (int)Mathf.Clamp(tempCapacity, 0, int.MaxValue);
            EditorGUILayout.EndHorizontal();
            
        }
        */

        if (GUILayout.Button("Test", GUILayout.Height(40)))
        {
            Debug.Log("Test");
        }
    }
}
