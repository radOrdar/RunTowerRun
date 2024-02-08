using System;
using UnityEngine;

namespace Core
{
    public class PersistentDataProvider
    {
        private DateTime _dateTimeExpirationSub;
        private int _level;
        
        public bool TryGetSubscriptionExpirationDate(out DateTime dateTime)
        {
            string s = PlayerPrefs.GetString(Constants.PrefsKeys.SUBSCRIPTION_EXPIRATION);
            if (string.IsNullOrEmpty(s) == false)
            {
                dateTime = JsonUtility.FromJson<DateTime>(s);
                return true;
            }

            dateTime = default;
            return false;
        }

        public void SaveSubscriptionExpirationDate(DateTime dateTime)
        {
            PlayerPrefs.SetString(Constants.PrefsKeys.SUBSCRIPTION_EXPIRATION, JsonUtility.ToJson(dateTime));
            PlayerPrefs.Save();
        }

        public void ResetProgress()
        {
            PlayerPrefs.SetInt(Constants.PrefsKeys.LEVEL, 0);
            PlayerPrefs.Save();
        }

        public int GetLevel() => 
            PlayerPrefs.GetInt(Constants.PrefsKeys.LEVEL);

        public void SaveLevel(int level)
        {
            PlayerPrefs.SetInt(Constants.PrefsKeys.LEVEL, level);
            PlayerPrefs.Save();
        }
    }
}