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
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동에도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 중복 방지
        }
        
        Init();
    }

    private void Init()
    {
        Day = 1;
        Money = 100;
        OpenRecipe = new List<int>();
        OpenIngredient = new List<int>();
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
}
