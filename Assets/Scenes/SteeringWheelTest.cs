using UnityEngine;

public class SteeringWheelTest : MonoBehaviour
{
    [SerializeField] private GameObject _steeringWheel;
    [SerializeField] private float _maxTurnAngle = 90f;
    private float _currentTurnAngle;

    public void RotateSteeringWheel(float ratio)
    {
        _currentTurnAngle = _maxTurnAngle * ratio;
        ApplyRotate(_currentTurnAngle);
    }

    private void ApplyRotate(float angle)
    {
        Vector3 wheelTransform = _steeringWheel.transform.localEulerAngles;

        _steeringWheel.transform.localRotation = Quaternion.Euler(wheelTransform.x, angle, wheelTransform.z);
        
    }
}
