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

    public bool HappyEndTest;
    public bool NormalEndTest;
    
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
        //빌드용
        Day = 1;
        Money = 0;
        OpenRecipe = new List<int> {  }; 
        OpenIngredient = new List<int> { 100, 106, 107, };

        //해피엔딩 테스트용
        // Day = 1;
        // Money = 0;
        // OpenRecipe = new List<int> {  200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211 };
        // OpenIngredient = new List<int> { 100, 101, 102, 103, 104, 105, 106, 107,108, 109, 110, 111 };

        //노말엔딩 테스트용
        // Day = 31;
        // Money = 0;
        // OpenRecipe = new List<int> { 200, 205 }; 
        // OpenIngredient = new List<int> { 100, 106, 107, };
        
        // OpenIngredient = new List<int> { 100, 101, 102, 103, 104, 105, 106, 107,108, 109, 110, 111 };
    }

    //데이터 저장
    public void Save()
    {
        print("[Save]");
        print("[Day] " + Day);
        print("[Money] " + Money);
        print("[OpenRecipe] " + OpenRecipe.Count);
        print("[OpenIngredient] " + OpenIngredient.Count);
        saveSystem.Save("Day", Day);
        saveSystem.Save("Money", Money);
        saveSystem.Save("OpenRecipe", OpenRecipe.ToArray());
        saveSystem.Save("OpenIngredient", OpenIngredient.ToArray());
    }

    //데이터 불러오기
    public void Load()
    {
        if (!saveSystem.DoesDataExists("Day"))
        {
            Reset();
            return;
        }
        Day = saveSystem.Load("Day").AsInt();
        Money = saveSystem.Load("Money").AsInt();
        OpenRecipe = new List<int>(saveSystem.LoadArray("OpenRecipe").AsIntArray());
        OpenIngredient = new List<int>(saveSystem.LoadArray("OpenIngredient").AsIntArray());
        
        
        // OpenRecipe = new List<int> {  200, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211 };
        // OpenIngredient = new List<int> { 100, 101, 102, 103, 104, 105, 106, 107,108, 109, 110, 111 };
        print("[LOAD]");
        print("[Day] " + Day);
        print("[Money] " + Money);
        print("[OpenRecipe] " + OpenRecipe.ToArray());
        print("[OpenIngredient] " + OpenIngredient.ToArray());
    }

    //데이터 리셋
    public void Reset()
    {
        Init();
        Save();
        Load();
    }

    public void AddRecipe(int id)
    {
        OpenRecipe.Add(id);
        onGetRecipe?.Invoke(id);
    }
}
