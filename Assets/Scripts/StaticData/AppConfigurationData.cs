using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "StaticData/AppConfig")]
    public class AppConfigurationData : ScriptableObject
    {
        public int targetFPS;
    }
}