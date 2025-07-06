using NUnit.Framework;
using UnityEngine;

public class TestTest : MonoBehaviour
{
    public Test[] tests;

    private void Start()
    {
        for (int i = 0; i < tests.Length; i++)
        {
            tests[i].ShowCustomerParfaitUI(CSVManager.instance.normalParfaitRecipeDic[320 + i]);
        }
    }
}
