using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SteeringWheel : XRBaseInteractable
{
    [SerializeField] private TESTTractorMovementController _controller;
    [SerializeField] private Transform _wheelTransform;

    [SerializeField] private float _maxWheelAngle;
    private int _maxWheelTurns;
    private float _maxWheelAngleInLastTurn;
    private float _currentAngle;

    public UnityEvent<float> OnWheelRotated;

    private float _baseAngle = 0.0f;

    [SerializeField] private Transform _target;
    private void Start()
    {
        _maxWheelAngleInLastTurn = _maxWheelAngle % 360.0f;
        _maxWheelTurns = (int)(_maxWheelAngle / 360);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _baseAngle = FindWheelAngle();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _baseAngle = FindWheelAngle();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                RotateWheel();
        }
    }

    private void RotateWheel()
    {
        float totalAngle = FindWheelAngle();
        float angleDifference = _baseAngle - totalAngle;

        angleDifference = NormalizeAngle(angleDifference);
        _currentAngle += angleDifference;

        LimitSteeringWheelRotation();
        _controller.RotateSteeringWheel(Angle1());

        _wheelTransform.Rotate(0, 0, -angleDifference, Space.Self);

        _baseAngle = totalAngle;
        OnWheelRotated?.Invoke(-angleDifference);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180) angle -= 360;
        else if (angle < -180) angle += 360;

        return angle;
    }

    private float Angle1()
    {
        return -_currentAngle / _maxWheelAngle;
    }

    private int _currentTurn;
    private void LimitSteeringWheelRotation()
    {
        _currentTurn = (int)(_currentAngle / 360.0f);
        if (_currentTurn == _maxWheelTurns)
        {
            if (_currentAngle > _maxWheelAngleInLastTurn && _maxWheelAngleInLastTurn >= 0)
            {
                _wheelTransform.localEulerAngles = new Vector3(_wheelTransform.localEulerAngles.x, _wheelTransform.localEulerAngles.y, AngleToEuler(_maxWheelAngleInLastTurn));
                _currentAngle = 360 * _currentTurn + _maxWheelAngleInLastTurn;
            }
        }
        else if (_currentTurn == -_maxWheelTurns)
        {
            if (_currentAngle < _maxWheelAngleInLastTurn && _maxWheelAngleInLastTurn <= 0)
            {
                _wheelTransform.localEulerAngles = new Vector3(_wheelTransform.localEulerAngles.x, _wheelTransform.localEulerAngles.y, AngleToEuler(-_maxWheelAngleInLastTurn));
                _currentAngle = 360 * _currentTurn - _maxWheelAngleInLastTurn;
            }
        }
    }

    private float AngleToEuler(float rotation)
    {
        if (rotation >= 0)
        {
            return 360 - rotation;
        }
        else
        {
            return -rotation;
        }
    }

    private float FindWheelAngle()
    {
        float totalAngle = 0;
        
        foreach (IXRSelectInteractor interactor in interactorsSelecting)
        {
            Vector2 direction = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
        }

        return totalAngle;
    }

    private Vector2 FindLocalPoint(Vector3 position)
    {
        return transform.InverseTransformPoint(position).normalized;
    }

    private float ConvertToAngle(Vector2 direction)
    {
        return Vector2.SignedAngle(Vector2.up, direction);
    }

    private float FindRotationSensitivity()
    {
        return 1.0f / interactorsSelecting.Count;
    }
}
