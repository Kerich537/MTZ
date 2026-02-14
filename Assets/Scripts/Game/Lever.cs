using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Lever : MonoBehaviour
{
    private enum AxesEnum { x, y, z }

    [Header("Preferences")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _leverVisual;
    [SerializeField] private AxesEnum _rotationAxe;

    [Header("Settings")]
    [SerializeField] private bool _useBorders;
    [SerializeField] private bool _activatingByAngle;
    [SerializeField] private bool _activatingByTrigger;

    [Header("borders")]
    [SerializeField] private float _standardAngle, _maxRotationAngle;
    [SerializeField] private float _activateInfelicity;

    [Header("Events")]
    [SerializeField] private UnityEvent _activateEvent;

    [Tooltip("Event works with activatingByTrigger and activatingByAngle")]
    [SerializeField] private UnityEvent _activatedActionEvent;

    [Tooltip("Event works ONLY with activatingByTrigger")]
    [SerializeField] private UnityEvent _activatedInverseActionEvent;

    private BoxCollider _collider;
    private Vector3 _normalColliderCentre, _negativeColliderCentre;
    private bool _isInvoked;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _normalColliderCentre = _collider.center;
        _negativeColliderCentre = -_collider.center;
    }

    private void FixedUpdate()
    {
        RotateToTarget();
        CheckRotation();
        LimitRotation();
        ActivateLeverByAngle();
    }


    //private void RotateToTarget()
    //{
    //    Vector3 dir = _target.position - transform.position;

    //    if (_rotationAxe == AxesEnum.x)
    //    {
    //        dir.x = 0;
    //    }
    //    else if (_rotationAxe == AxesEnum.y)
    //    {
    //        dir.y = 0;
    //    }
    //    else if (_rotationAxe == AxesEnum.z)
    //    {
    //        dir.z = 0;
    //    }

    //    transform.localRotation = Quaternion.LookRotation(dir);
    //}

    private void RotateToTarget()
    {
        Vector3 localTargetPos = transform.parent.InverseTransformPoint(_target.position);
        Vector3 localCurrentPos = transform.parent.InverseTransformPoint(transform.position);

        Vector3 dir = localTargetPos - localCurrentPos;

        if (_rotationAxe == AxesEnum.x)
        {
            dir.x = 0;
        }
        else if (_rotationAxe == AxesEnum.y)
        {
            dir.y = 0;
        }
        else if (_rotationAxe == AxesEnum.z)
        {
            dir.z = 0;
        }

        if (dir.magnitude > 0.001f)
        {
            transform.localRotation = Quaternion.LookRotation(dir);
        }
    }

    private void CheckRotation()
    {
        if (_rotationAxe == AxesEnum.x)
        {
            float angleY = transform.localEulerAngles.y;
            float angleZ = transform.localEulerAngles.z;

            if ((angleY > 10 && angleY < 182) && angleZ < 170)
            {
                _leverVisual.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                _collider.center = new Vector3(_collider.center.x, _negativeColliderCentre.y, _collider.center.z);
            }
            else
            {
                _leverVisual.transform.localEulerAngles = Vector3.zero;
                _collider.center = _normalColliderCentre;
            }
        }
        else if (_rotationAxe == AxesEnum.y)
        {

        }
        else if (_rotationAxe == AxesEnum.z)
        {

        }
    }

    private void LimitRotation()
    {
        if (_useBorders)
        {
            if (_rotationAxe == AxesEnum.x)
            {
                float angle = transform.eulerAngles.x;
                angle = Mathf.Clamp(angle, _standardAngle - _maxRotationAngle, _standardAngle + _maxRotationAngle);
                transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else if (_rotationAxe == AxesEnum.y)
            {
                float angle = transform.eulerAngles.y;
                angle = Mathf.Clamp(angle, _standardAngle - _maxRotationAngle, _standardAngle + _maxRotationAngle);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            }
            else if (_rotationAxe == AxesEnum.z)
            {
                float angle = transform.eulerAngles.z;
                angle = Mathf.Clamp(angle, _standardAngle - _maxRotationAngle, _standardAngle + _maxRotationAngle);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            }
        }
    }

    private void ActivateLeverByAngle()
    {
        if (!_isInvoked && _activatingByAngle)
        {
            if (_rotationAxe == AxesEnum.x &&
                (transform.eulerAngles.x > (_standardAngle + _maxRotationAngle - _activateInfelicity) ||
                transform.eulerAngles.x < (_standardAngle - _maxRotationAngle + _activateInfelicity)))
            {
                _isInvoked = true;
                _activateEvent.Invoke();
                _activatedActionEvent.Invoke();
            }

            if (_rotationAxe == AxesEnum.y &&
                (transform.eulerAngles.y > (_standardAngle + _maxRotationAngle - _activateInfelicity) ||
                transform.eulerAngles.y < (_standardAngle - _maxRotationAngle + _activateInfelicity)))
            {
                _isInvoked = true;
                _activateEvent.Invoke();
                _activatedActionEvent.Invoke();
            }

            if (_rotationAxe == AxesEnum.z &&
                (transform.eulerAngles.z > (_standardAngle + _maxRotationAngle - _activateInfelicity) ||
                transform.eulerAngles.z < (_standardAngle - _maxRotationAngle + _activateInfelicity)))
            {
                _isInvoked = true;
                _activateEvent.Invoke();
                _activatedActionEvent.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LeverTrigger leverTrigger) && !_isInvoked && _activatingByTrigger)
        {
            if (leverTrigger.directionIsForward)
            {
                _isInvoked = true;
                _activateEvent.Invoke();
                _activatedActionEvent.Invoke();
            }
            else if (!leverTrigger.directionIsForward)
            {
                _isInvoked = true;
                _activateEvent.Invoke();
                _activatedInverseActionEvent.Invoke();
            }
        }
    }
    public void IsInvokedFalse() { _isInvoked = false; }
}
