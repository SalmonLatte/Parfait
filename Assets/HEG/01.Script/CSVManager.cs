using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class IngredientData
{
    public string name;
    public int id;
    public int price;
    public int buyPrice;
}

[System.Serializable]
public class ParfaitRecipeData
{
    public string name;
    public int id;
    public int price;
    public int[] ingredientIds = new int[7];
}

public class CSVManager : MonoBehaviour
{
    public static CSVManager instance;

    public Dictionary<int, IngredientData> ingredientsDic = new Dictionary<int, IngredientData>();
    public Dictionary<int, ParfaitRecipeData> parfaitRecipeDic = new Dictionary<int, ParfaitRecipeData>();

    private void Awake()
    {
        instance = this;

        Read_IngredientInfoCSV("Ingredient");
        Read_PrafaitRecipeCSV("ParfaitReciepe");
    }

    private void Read_IngredientInfoCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다!");
            return;
        }
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) 
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            IngredientData data = new IngredientData
            {
                name = values[0],
                id = int.Parse(values[1]),
                price = int.Parse(values[2]),
                buyPrice = int.Parse(values[3])
            };

            ingredientsDic[data.id] = data;
        }

        Debug.Log("재료 로드 완료: " + ingredientsDic.Count + "개");
    }

    private void Read_PrafaitRecipeCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다!");
            return;
        }
        string[] lines = csvFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            if (values.Length < 10)
            {
                Debug.LogWarning($"줄 {i + 1}: 필드 개수 부족 ({values.Length}) → 건너뜀");
                continue;
            }

            ParfaitRecipeData data = new ParfaitRecipeData();
            data.name = values[0];
            data.id = int.Parse(values[1]);
            data.price = int.Parse(values[2]);

            for (int j = 0; j < 7; j++)
            {
                data.ingredientIds[j] = int.Parse(values[3 + j]);
            }

            parfaitRecipeDic[data.id] = data;
        }

        Debug.Log("총 파르페 개수: " + parfaitRecipeDic.Count + "개");
    }

}
