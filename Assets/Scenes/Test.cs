using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Test : XRBaseInteractable
{
    [SerializeField] private TESTTractorMovementController _controller;
    [SerializeField] private Transform _rotateTransform;

    [SerializeField] private float _maxAngle;
    private float _currentAngle;

    private float _baseAngle = 0.0f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _baseAngle = FindAngle();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _baseAngle = 0;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                Rotate(); 
            }
        }
    }

    private void Rotate()
    {
        _currentAngle = FindAngle();
        float _angleFromZeroToMax = _currentAngle - (360 - _maxAngle); 
        print(_currentAngle + " " + _baseAngle + " " + FindAngle());
        if (_angleFromZeroToMax > _maxAngle)
        {
            ApplyRotation(_angleFromZeroToMax + (360 - _maxAngle));
        }
        else if (_angleFromZeroToMax < 1)
        {
            ApplyRotation(1);
        }
        else
        {
            ApplyRotation(_currentAngle);
        }
    }

    private void ApplyRotation(float angle)
    {
        _rotateTransform.localEulerAngles = new Vector3(_rotateTransform.localEulerAngles.x, angle, _rotateTransform.localEulerAngles.z);
    }

    private float FindAngle()
    {
        return interactorsSelecting.ToArray()[0].transform.parent.localEulerAngles.z;
    }

}
