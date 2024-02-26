using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "StaticData/AppConfig")]
    public class AppConfigurationData : ScriptableObject
    {
        public int targetFPS;
        
        public bool isAppodealTestMode;
        public string appodealAppKey = "a287d5f56949919de334e67dc0188e8f68c19f09704bea5a";
    }
}