using System;
using System.Collections.Generic;
using System.Linq;
using AASave;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    [SerializeField] private SaveSystem saveSystem;

    //TODO 나중에 private로 변경
    public int Day;
    public int Money;
    public List<int> OpenRecipe;
    public List<int> OpenIngredient;
    
    public Action<int> onGetRecipe;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
        Init();
    }

    private void Init()
    {
        Day = 21;
        Money = 15000;
        OpenRecipe = new List<int>();
        // OpenIngredient = new List<int> { 100, 102, 103, 104, 105, 106, 107,108, 109, 110, 111 };
         OpenIngredient = new List<int> { 100, 106, 107, };

    }

    //데이터 저장
    public void Save()
    {
        saveSystem.Save("Day", Day);
        saveSystem.Save("Money", Money);
        saveSystem.Save("OpenRecipe", OpenRecipe.ToArray());
        saveSystem.Save("OpenIngredient", OpenIngredient.ToArray());
    }

    //데이터 불러오기
    public void Load()
    {
        Day = saveSystem.Load("Day").AsInt();
        Money = saveSystem.Load("Money").AsInt();
        OpenRecipe = new List<int>(saveSystem.LoadArray("OpenRecipe").AsIntArray());
        OpenIngredient = new List<int>(saveSystem.LoadArray("OpenIngredient").AsIntArray());
    }

    //데이터 리셋
    public void Reset()
    {
        Init();
        Load();
        Save();
    }

    public void AddRecipe(int id)
    {
        OpenRecipe.Add(id);
        onGetRecipe?.Invoke(id);
    }
}
