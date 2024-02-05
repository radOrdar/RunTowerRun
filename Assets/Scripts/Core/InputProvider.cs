using UnityEngine;

namespace Core
{
    public class InputProvider
    {
        public bool GetMouseButtonDown(int button) => 
            Input.GetMouseButtonDown(button);

        public bool GetMouseButtonUp(int button) => 
            Input.GetMouseButtonUp(button);

        public Vector3 MousePosition =>
            Input.mousePosition;
        public bool GetMouseButton(int button) =>
            Input.GetMouseButton(button);
    }
}