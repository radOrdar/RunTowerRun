using System;

namespace Services.Events
{
    public interface IEventService : IService
    {
        event Action GatePassed;
        event Action GateCollided;
        event Action FinishPassed;
        event Action<bool> HasteSwitch;
        event Action AdsRemoved;
        void OnGatePassed();
        void OnGateCollided();
        void OnFinishPassed();
        void OnHasteSwitch(bool enable);
        void OnAdsRemoved();
    }
}