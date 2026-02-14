using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TESTTractorMovementController : MonoBehaviour
{
    [Header("Speed of gears")]
    private readonly string[] _gearNames = { "R", "N", "1", "2", "3", "4", "5", "6" };
    [SerializeField] private float[] _gearSpeeds;

    [Header("Движение")]
    [SerializeField] private float _accelerationSmooth = 1.2f;
    [SerializeField] private float _neutralBrakeSmooth = 2.5f;
    [SerializeField] private float _handbrakeBrakeSmooth = 0.4f;

    [Header("Физика")]
    [SerializeField] private float _motorForce = 1500f;
    [SerializeField] private float _brakeForce = 4000f;
    [SerializeField] private float _maxSteeringWheelAngle = 90f;
    [SerializeField] private float _maxSteerAngle = 28f;
    [SerializeField] private float _downforce = 8f;

    [Header("Wheel colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Visual of wheels")]
    public Transform frontLeftVisual;
    public Transform frontRightVisual;
    public Transform rearLeftVisual;
    public Transform rearRightVisual;

    [Header("Переменные")]
    [SerializeField] private TractorGearsUI _gearsUI;
    [SerializeField] private TractorSpeedometerUI _speedometerUI;
    [SerializeField] private Transform _steeringWheel;
    private Rigidbody _rb;

    private int _currentGear = 1;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedVelocity;
    private bool _handbrake;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        _rb.mass = 2500f;
        _rb.linearDamping = 0.05f;
        _rb.angularDamping = 1.2f;
        _rb.centerOfMass = new Vector3(0, -0.6f, 0);
        
        _gearsUI.UpdateGear(_gearNames[_currentGear]);
    }

    void Update()
    {
        UpdateWheelVisuals();
    }

    void FixedUpdate()
    {
        UpdateTargetSpeed();
        ApplyMovement();
        ApplySteering();
        ApplyDownforce();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _currentGear < _gearSpeeds.Length - 1)
        {
            _currentGear++;
            Debug.Log("Gear: " + _gearNames[_currentGear]);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && _currentGear > 0)
        {
            _currentGear--;
            Debug.Log("Gear: " + _gearNames[_currentGear]);
        }

        _handbrake = Input.GetKey(KeyCode.Space);
    }

    public void GearUp() 
    {
        if (_currentGear < _gearSpeeds.Length - 1)
        {
            _currentGear++;
            Debug.Log("Gear: " + _gearNames[_currentGear]);
            _gearsUI.UpdateGear(_gearNames[_currentGear]);
        }
    }
    public void GearDown() 
    {
        if (_currentGear > 0)
        {
            _currentGear--;
            Debug.Log("Gear: " + _gearNames[_currentGear]);
            _gearsUI.UpdateGear(_gearNames[_currentGear]);
        }
    }

    public void SetHandbrake(bool handbrake) { _handbrake = handbrake;  }

    void UpdateTargetSpeed()
    {
        _targetSpeed = _handbrake ? 0f : _gearSpeeds[_currentGear];

        float smooth = _currentGear == 1
            ? _neutralBrakeSmooth
            : _accelerationSmooth;

        if (_handbrake)
            smooth = _handbrakeBrakeSmooth;

        _currentSpeed = Mathf.SmoothDamp(
            _currentSpeed,
            _targetSpeed,
            ref _speedVelocity,
            smooth
        );

        _speedometerUI.UpdateSpeed(_rb.linearVelocity.magnitude);
    }

    void ApplyMovement()
    {
        float currentForwardSpeed =
            Vector3.Dot(_rb.linearVelocity, transform.forward) * 3.6f;

        float speedDiff = _currentSpeed - currentForwardSpeed;

        float motorTorque = speedDiff * _motorForce;
        motorTorque = Mathf.Clamp(motorTorque, -_motorForce, _motorForce);

        float brakeTorque = 0f;

        if (Mathf.Abs(currentForwardSpeed) > Mathf.Abs(_currentSpeed) + 0.5f)
        {
            brakeTorque = _brakeForce;
        }

        if (_handbrake)
        {
            brakeTorque = _brakeForce;
            motorTorque = 0f;
        }

        rearLeft.motorTorque = motorTorque;
        rearRight.motorTorque = motorTorque;

        rearLeft.brakeTorque = brakeTorque;
        rearRight.brakeTorque = brakeTorque;

        frontLeft.brakeTorque = brakeTorque * 0.7f;
        frontRight.brakeTorque = brakeTorque * 0.7f;
    }

    public float GetMaxSteeringWheelAngle() {  return _maxSteeringWheelAngle; }

    [Range(-1,1)]
    private float _ratio;

    public void RotateSteeringWheel(float ratio)
    {
        _ratio = ratio;
        _steeringWheel.localEulerAngles = new Vector3(0, _ratio * _maxSteeringWheelAngle, 0);
    }

    void ApplySteering()
    {
        float steerAngle = -_ratio * _maxSteerAngle;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;
    }

    void ApplyDownforce()
    {
        _rb.AddForce(-transform.up * _downforce * _rb.linearVelocity.magnitude);
    }

    void UpdateWheelVisuals()
    {
        UpdateWheel(frontLeft, frontLeftVisual);
        UpdateWheel(frontRight, frontRightVisual);
        UpdateWheel(rearLeft, rearLeftVisual);
        UpdateWheel(rearRight, rearRightVisual);
    }

    void UpdateWheel(WheelCollider col, Transform wheelRoot)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);

        wheelRoot.position = pos;

        float spinAngle = col.rpm * 6f * Time.deltaTime;

        Quaternion spinRotation = Quaternion.Euler(spinAngle, 0f, 0f);

        wheelRoot.rotation = rot * spinRotation;
    }
}
