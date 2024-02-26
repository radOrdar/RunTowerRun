using UnityEngine;

namespace Services.Input
{
    public interface IInputService : IService
    {
        bool GetMouseButtonDown(int button);
        bool GetMouseButtonUp(int button);
        Vector3 MousePosition { get; }
        bool GetMouseButton(int button);
    }
}