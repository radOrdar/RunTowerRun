using System;

namespace Services.Save
{
    public interface IPersistentDataService : IService
    {
        bool TryGetSubscriptionExpirationDate(out DateTime dateTime);
        void SaveSubscriptionExpirationDate(DateTime dateTime);
        void ResetProgress();
        int GetLevel();
        void SaveLevel(int level);
    }
}