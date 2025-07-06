using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParfaitBuilder : MonoBehaviour
{
    private List<int> currentParfait = new List<int>();
    private int[] targetRecipe;

    [SerializeField] private Image[] parfaitIngredientLayers;
    [SerializeField] private Sprite[] ingredients;
    
    [SerializeField] private GameObject[] parfaitToppingLayers;

    public GameObject failPanel;

    public int click = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GiveTheParfait();
        }
    }

    public void StartNewRecipe(int[] recipe)
    {
        currentParfait.Clear();
        targetRecipe = recipe;
        ClearVisualStack();
    }

    public void OnIngredientClicked(int id)
    {
        // �ʹ� ���� ����
        if (currentParfait.Count >= targetRecipe.Length)
        {
            Debug.Log("Ʋ�Ƚ��ϴ�!");
            ShowFail();
            return;
        }

        // Ʋ�ȴ��� �ǽð� Ȯ��
        if (targetRecipe[currentParfait.Count] != id)
        {
            Debug.Log("Ʋ�Ƚ��ϴ�!");
            ShowFail();
            return;
        }

        currentParfait.Add(id);
        AddVisualLayer(id);
        click++;
    }

    void AddVisualLayer(int id)
    {
        if (click == 8 || targetRecipe[click + 1] == 0)
        {
            parfaitToppingLayers[click].transform.GetChild(id % 100).gameObject.SetActive(true);
            return;
        }
        else
        {
            parfaitIngredientLayers[click].enabled = true;
            parfaitIngredientLayers[click].sprite = ingredients[id % 100];
        }
    }

    public void GiveTheParfait()
    {
        int cnt = 0;
        // ���� ������� Ȯ��
        for (int i = 0; i < targetRecipe.Length; i++)
        {
            if (targetRecipe[i] != 0)
            {
                cnt++;
            }
        }
        if (currentParfait.Count == cnt)
        {
            Debug.Log("���� ��Ȯ�ϰ� �׾ҽ��ϴ�! ���� ����");
        }
    }

    //�ʱ�ȭ
    void ClearVisualStack()
    {
        click = 0;
    }

    //���� �����ֱ�
    void ShowFail()
    {

    }
}
