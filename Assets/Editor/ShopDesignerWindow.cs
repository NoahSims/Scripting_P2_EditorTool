using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopDesignerWindow : EditorWindow
{
    private static ShopData shopData;
    public static ShopData ShopInfo { get { return shopData; } }

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

        if(GUILayout.Button("Test", GUILayout.Height(40)))
        {
            Debug.Log("Test");
        }
    }
}
