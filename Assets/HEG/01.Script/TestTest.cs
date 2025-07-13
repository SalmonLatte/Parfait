using System;
using NUnit.Framework;

using UnityEngine;

public class TestTest : MonoBehaviour
{
    public ParfaitUI menuUnitPrefab;
    public Transform menuUnitParent;
    public ParfaitRecipeManager parfaitRecipeManager;
    
    public static TestTest Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    //[MenuItem("Menu/MenuTest")]
    public void MenuTest()
    {
        Debug.Log("erwerwerwrw");
        Debug.Log(parfaitRecipeManager.normalParfaitRecipeDic.Count);
        foreach (var recipeDic in parfaitRecipeManager.normalParfaitRecipeDic)
        {
            Debug.Log(recipeDic.Value.name);

            ParfaitUI menuUnit = Instantiate(menuUnitPrefab, menuUnitParent);
            menuUnit.ShowCustomerParfaitUI(recipeDic.Value);
        }
    }
    
    
}
