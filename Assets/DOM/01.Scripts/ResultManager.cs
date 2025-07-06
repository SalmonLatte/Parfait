using System;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject productUnitPrefab;
    [SerializeField] Transform productUnitParent;
    List<IngredientData> ingredientDatas = new List<IngredientData>();
    List<ProductUnit> productUnits = new List<ProductUnit>();
    private void Start()
    {
        ingredientDatas = CSVManager.instance.GetIngredientDataList();
        SetInfo();
    }

    public void SetInfo()
    {
        for (int i = 0; i < ingredientDatas.Count; i++)
        {
            Debug.Log(ingredientDatas[i].name);
            bool isSoldOut = false;
            GameObject productUnitObject = Instantiate(productUnitPrefab, productUnitParent);
            //유저가 가진 재료면 unlock
            if (SaveLoadManager.Instance.OpenIngredient.Contains(ingredientDatas[i].id))
            {
                isSoldOut = true;
            }
            //세팅
            ProductUnit productUnit = productUnitObject.GetComponent<ProductUnit>();
            productUnits.Add(productUnit);
            productUnit.SetInfo(ingredientDatas[i].name, ingredientDatas[i].buyPrice, true, isSoldOut, AllRefresh);
        }    
    }

    public void AllRefresh()
    {
        for (int i = 0; i < productUnits.Count; i++)
        {
            productUnits[i].Refresh();
        }
    }
}
