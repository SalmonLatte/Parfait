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
        // 너무 많이 쌓음
        if (currentParfait.Count >= targetRecipe.Length)
        {
            Debug.Log("틀렸습니다!");
            ShowFail();
            return;
        }

        // 틀렸는지 실시간 확인
        if (targetRecipe[currentParfait.Count] != id)
        {
            Debug.Log("틀렸습니다!");
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
        // 전부 맞췄는지 확인
        for (int i = 0; i < targetRecipe.Length; i++)
        {
            if (targetRecipe[i] != 0)
            {
                cnt++;
            }
        }
        if (currentParfait.Count == cnt)
        {
            Debug.Log("전부 정확하게 쌓았습니다! 제출 가능");
        }
    }

    //초기화
    void ClearVisualStack()
    {
        click = 0;
    }

    //실패 보여주기
    void ShowFail()
    {

    }
}
