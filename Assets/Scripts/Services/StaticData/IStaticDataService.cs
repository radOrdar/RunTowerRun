using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.StaticData
{
    public interface IStaticDataService : IService
    {
        UniTask<T> GetData<T>() where T : ScriptableObject;
    }
}