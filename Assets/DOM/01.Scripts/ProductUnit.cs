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

    private Button productButton;
    private int productID;
    private int price;
    private bool isLock;
    private bool isSoldOut;

    private Action allRefresh;

    private void Awake()
    {
        productButton = transform.GetComponent<Button>();
        productButton.onClick.AddListener(Buy);
    }

    public void SetInfo(IngredientData data, bool isLock, bool isSoldOut, Action allRefresh)
    {
        productID = data.id;
        productImage.sprite = data.image;
        nameText.text = data.name;
        priceText.text = data.buyPrice.ToString();
        price = data.buyPrice;
        this.isLock = isLock;
        this.isSoldOut = isSoldOut;
        this.allRefresh = allRefresh;
        
        Refresh();
    }
    
    public void SetInfo(string name, int price, bool isLock, bool isSoldOut, Action allRefresh)
    {
        // productSprite = sprite;
        nameText.text = name;
        priceText.text = price.ToString();
        this.price = price;
        this.isLock = isLock;
        this.isSoldOut = isSoldOut;
        this.allRefresh = allRefresh;

        Refresh();
    }

    public void Refresh()
    {
        if (isSoldOut)
        {
            lockImage.SetActive(false);
            soldOutImage.SetActive(true);
            productButton.enabled = false;
            priceText.text = "SOLD OUT";
        }
        else if (isLock || SaveLoadManager.Instance.Money < price)
        {
            lockImage.SetActive(true);
            soldOutImage.SetActive(false);
            productButton.enabled = false;
        }
        else
        {
            lockImage.SetActive(false);
            soldOutImage.SetActive(false);
            productButton.enabled = true;
        }
    }

    public void Buy()
    {
        Debug.Log("$$$$$$$$$$$" + isLock + ", " + isSoldOut);
        if (isLock || isSoldOut)
            return;
        SaveLoadManager.Instance.Money -= price;
        Debug.Log(SaveLoadManager.Instance.Money);
        isSoldOut = true;
        allRefresh?.Invoke();
    }
    
    public void SetLock(bool b) { isLock = b; }
    public int GetID() { return productID; }
    public bool IsSoldOut() { return isSoldOut; }
}
