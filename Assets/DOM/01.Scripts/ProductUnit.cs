using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductUnit : MonoBehaviour
{
    [SerializeField] private Image productImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject soldOutImage;
    [SerializeField] private Button productButton;
    
    private int productID;
    private int price;
    private bool isLock;
    private bool isSoldOut;

    private Action allRefresh;

    private void Awake()
    {
        // productButton = transform.GetComponent<Button>();
        productButton.onClick.AddListener(Buy);
    }

    public void SetInfo(IngredientData data, bool isLock, bool isSoldOut, Action allRefresh)
    {
        productID = data.id;
        productImage.sprite = data.image;
        nameText.text = data.name;
        priceText.text = data.buyPrice.ToString();
        price = data.buyPrice;
        // this.isLock = isLock;
        this.isSoldOut = isSoldOut;
        this.allRefresh = allRefresh;
        
        Refresh();
    }

    public void Refresh()
    {
        if (productButton == null)
            Debug.Log("productButton is null");
        if (isSoldOut)
        {
            lockImage.SetActive(false);
            soldOutImage.SetActive(true);
            productButton.interactable = false;
            priceText.text = "SOLD OUT";
        }
        else if (isLock || SaveLoadManager.Instance.Money < price)
        {
            lockImage.SetActive(true);
            soldOutImage.SetActive(false);
            productButton.interactable = false;
        }
        else
        {
            lockImage.SetActive(false);
            soldOutImage.SetActive(false);
            productButton.interactable = true;
        }
    }

    public void Buy()
    {
        Debug.Log("$$$$$$$$$$$" + isLock + ", " + isSoldOut);
        if (isLock || isSoldOut)
            return;
        AudioManager.Instance.PlaySFX("Coin");
        SaveLoadManager.Instance.Money -= price;
        SaveLoadManager.Instance.OpenIngredient.Add(productID);
        SaveLoadManager.Instance.Save();
        Debug.Log(SaveLoadManager.Instance.Money);
        isSoldOut = true;
        allRefresh?.Invoke();
    }
    
    public void SetLock(bool b) { isLock = b; }
    public int GetID() { return productID; }
    public bool IsSoldOut() { return isSoldOut; }
}
