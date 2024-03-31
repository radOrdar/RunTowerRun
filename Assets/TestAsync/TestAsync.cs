using System;
using Cysharp.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;

public class TestAsync : MonoBehaviour

{
    private string result = "";    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            doWork();
        }
    }

    async UniTask doWork()
    {
        Debug.Log(" Start working ");
        UniTask<string> boilWaterAsync = BoilWaterAsync();
        UnityEngine.Debug.Log(" Put tea in cups ");
        int a = 0;
        for (long i = 0; i < 1000_000_0000; i++)
        {
            a += (int)i;
        }

        UnityEngine.Debug.Log(a);
        string waterAsync = await boilWaterAsync;
        UnityEngine.Debug.Log(waterAsync);
    }

    async UniTask<string> BoilWaterAsync()
    {
        Debug.Log("Start boiling");
        await UniTask.Delay(10);
        Debug.Log("Finished boiling");
        await UniTask.Delay(10);
        return await UniTask.FromResult("ready");
    }
}