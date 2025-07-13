using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject productUnitPrefab;
    [SerializeField] Transform productUnitParent;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI initialMoneyText;
    [SerializeField] private TextMeshProUGUI earnedMoneyText;
    // [SerializeField] private TextMeshProUGUI spentMoneyText;
    [SerializeField] private TextMeshProUGUI totalMoneyText;

    // List<GameObject> productUnitObjects = new List<GameObject>();
    
    List<IngredientData> ingredientDatas = new List<IngredientData>();
    List<ProductUnit> productUnits = new List<ProductUnit>();

    private int[] ingredientID = { 100, 106, 107, 101, 102, 108, 109, 103, 104, 110, 111, 105 };
    private int curUnlockIndex = 3;
    
    private void Start()
    {
        ingredientDatas = CSVManager.instance.GetIngredientDataList();
        Init();
    }

    public void Init()
    {
        print("상품 INIT");
        for (int i = 0; i < ingredientDatas.Count; i++)
        {
            GameObject productUnitObject = Instantiate(productUnitPrefab, productUnitParent);
            ProductUnit productUnit = productUnitObject.GetComponent<ProductUnit>();
            productUnits.Add(productUnit);
        }
        
        // SetInfo(1, 0, 0);
    }
    
    public void SetInfo(int day, int initialMoney, int earnedMoney)
    {
        print("!!!!!!!!!" + initialMoney +", " + earnedMoney);
        dayText.text = "Day " + (SaveLoadManager.Instance.Day - 1);
        initialMoneyText.text = (initialMoney-earnedMoney).ToString();
        earnedMoneyText.text = "+" + earnedMoney.ToString();
        totalMoneyText.text = initialMoney.ToString();
        
        for (int i = 0; i < ingredientDatas.Count; i++)
        {
            bool isSoldOut = false;
            // bool isLock = true;
            bool isLock = false;

            
            //유저가 가진 재료면 soldout, 이제 막 열린거면 unlock
            if (SaveLoadManager.Instance.OpenIngredient.Contains(ingredientDatas[i].id))
                isSoldOut = true;
            if (ingredientID[curUnlockIndex] == ingredientDatas[i].id)
                isLock = false;
            
            //세팅
            productUnits[i].SetInfo(ingredientDatas[i], isLock, isSoldOut, AllRefresh);
        }
        AllRefresh();
    }

    public void AllRefresh()
    {
        totalMoneyText.text = SaveLoadManager.Instance.Money.ToString();
        for (int i = 0; i < productUnits.Count; i++)
        {

            //열어야하는 재료가 없으면 넘어감
            if (curUnlockIndex == 11)
                break;
            if (ingredientID[curUnlockIndex] == productUnits[i].GetID())
            {
                Debug.Log(productUnits[i].GetID() + ", " + ingredientID[curUnlockIndex] +", " + productUnits[i].IsSoldOut());

                if (productUnits[i].IsSoldOut())
                {
                    curUnlockIndex++;
                    ProductUnit target = productUnits.FirstOrDefault(i => i.GetID() == ingredientID[curUnlockIndex]);
                    if (target)
                    {
                        target.SetLock(false);
                        target.transform.SetAsFirstSibling();
                        break;
                    }
                    
                }
            }
        }

        for (int i = 0; i < productUnits.Count; i++)
        {
            productUnits[i].Refresh();
            if (productUnits[i].IsSoldOut())
            {
                productUnits[i].transform.SetAsLastSibling();
            }
        }
        
    }
}
