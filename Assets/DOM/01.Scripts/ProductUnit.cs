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
    private int price;
    private bool isLock;
    private bool isSoldOut;

    private Action allRefresh;

    private void Awake()
    {
        productButton = transform.GetComponent<Button>();
        productButton.onClick.AddListener(Buy);
    }

    public void SetInfo(Sprite productSprite, string name, int price, bool isLock, bool isSoldOut, Action allRefresh)
    {
        productImage.sprite = productSprite;
        nameText.text = name;
        priceText.text = price.ToString();
        this.price = price;
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
        SaveLoadManager.Instance.Money -= price;
        allRefresh?.Invoke();
    }
    
    public void SetLock(bool b) { isLock = b; }
}
