using System;

namespace Services.Events
{
    public class EventService : IEventService
    {
        public event Action GatePassed;
        public event Action GateCollided;
        public event Action FinishPassed;
        public event Action<bool> HasteSwitch;
        public event Action AdsRemoved;

        public void OnGatePassed()
        {
            GatePassed?.Invoke();
        }

        public void OnGateCollided()
        {
            GateCollided?.Invoke();
        }

        public void OnFinishPassed()
        {
            FinishPassed?.Invoke();
        }

        public void OnHasteSwitch(bool enable)
        {
            HasteSwitch?.Invoke(enable);
        }

        public void OnAdsRemoved()
        {
            AdsRemoved?.Invoke();
        }
    }
}
