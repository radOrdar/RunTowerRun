using UnityEngine;

namespace Services.Input
{
    public class InputService : IInputService
    {
        public bool GetMouseButtonDown(int button) => 
            UnityEngine.Input.GetMouseButtonDown(button);

        public bool GetMouseButtonUp(int button) => 
            UnityEngine.Input.GetMouseButtonUp(button);

        public Vector3 MousePosition =>
            UnityEngine.Input.mousePosition;
        public bool GetMouseButton(int button) =>
            UnityEngine.Input.GetMouseButton(button);
    }
}