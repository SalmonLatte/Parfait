using NUnit.Framework;
using UnityEngine;

public class TestTest : MonoBehaviour
{
    public ParfaitUI[] tests;

    private void Start()
    {
        for (int i = 0; i < tests.Length; i++)
        {
            tests[i].ShowCustomerParfaitUI(CSVManager.instance.normalParfaitRecipeDic[300 + i]);
        }
    }
}
