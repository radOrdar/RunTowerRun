using Core;
using Infrastructure;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    [SerializeField] private float normalFov;
    [SerializeField] private float accelFov;
    [SerializeField] private float finishFov;
    [SerializeField] private float fovSpeed;
    [SerializeField] private float rotationSpeed;

    private EventsProvider eventsProvider;
    private Camera _mainCamera;
    
    private float _targetFov;
    private bool _rotating;

    private void Start()
    {
        _mainCamera = Camera.main;
        eventsProvider = ProjectContext.I.EventsProvider;
        eventsProvider.HasteSwitch += OnHasteSwitch;
        eventsProvider.FinishPassed += OnFinishPassed;
    }

    private void OnDestroy()
    {
        eventsProvider.HasteSwitch -= OnHasteSwitch;
        eventsProvider.FinishPassed -= OnFinishPassed;
    }

    private void Update()
    {
        _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, _targetFov, fovSpeed * Time.deltaTime);

        if (_rotating)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void OnHasteSwitch(bool enable)
    {
        _targetFov = enable ? accelFov : normalFov;
    }

    private void OnFinishPassed()
    {
        _rotating = true;
        _targetFov = finishFov;
    }
}