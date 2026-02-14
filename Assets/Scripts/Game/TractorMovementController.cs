using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TractorMovementController : MonoBehaviour
{
    [Header("Speed of gears")]
    [SerializeField] private float reverseSpeed = -20f;
    [SerializeField] private float gear1 = 10f;
    [SerializeField] private float gear2 = 15f;
    [SerializeField] private float gear3 = 20f;
    [SerializeField] private float gear4 = 30f;
    [SerializeField] private float gear5 = 40f;

    [Header("Движение")]
    [SerializeField] private float accelerationSmooth = 1.2f;
    [SerializeField] private float neutralBrakeSmooth = 2.5f;
    [SerializeField] private float handbrakeBrakeSmooth = 0.4f;

    [Header("Физика")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float brakeForce = 4000f;
    [SerializeField] private float maxSteerAngle = 28f;
    [SerializeField] private float downforce = 8f;

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

    private Rigidbody rb;

    private readonly string[] gearNames = { "R", "N", "1", "2", "3", "4", "5" };
    private float[] gearSpeeds;

    private int currentGear = 1;
    private float currentSpeed;
    private float targetSpeed;
    private float speedVelocity;
    private bool handbrake;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 2500f;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 1.2f;
        rb.centerOfMass = new Vector3(0, -0.6f, 0);

        gearSpeeds = new float[]
        {
            reverseSpeed,
            0f,
            gear1,
            gear2,
            gear3,
            gear4,
            gear5
        };

        Debug.Log("Gear: N");
    }
    void Update()
    {
        HandleInput();
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentGear < gearSpeeds.Length - 1)
        {
            currentGear++;
            Debug.Log("Gear: " + gearNames[currentGear]);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentGear > 0)
        {
            currentGear--;
            Debug.Log("Gear: " + gearNames[currentGear]);
        }

        handbrake = Input.GetKey(KeyCode.Space);
    }
    void UpdateTargetSpeed()
    {
        targetSpeed = handbrake ? 0f : gearSpeeds[currentGear];
        float smooth = currentGear == 1
            ? neutralBrakeSmooth
            : accelerationSmooth;
        if (handbrake)
            smooth = handbrakeBrakeSmooth;
        currentSpeed = Mathf.SmoothDamp(
            currentSpeed,
            targetSpeed,
            ref speedVelocity,
            smooth
        );
    }

    void ApplyMovement()
    {
        float currentForwardSpeed =
            Vector3.Dot(rb.linearVelocity, transform.forward) * 3.6f;

        float speedDiff = currentSpeed - currentForwardSpeed;
        float motorTorque = speedDiff * motorForce;
        motorTorque = Mathf.Clamp(motorTorque, -motorForce, motorForce);
        float brakeTorque = 0f;
        if (Mathf.Abs(currentForwardSpeed) > Mathf.Abs(currentSpeed) + 0.5f)
        {
            brakeTorque = brakeForce;
        }
        if (handbrake)
        {
            brakeTorque = brakeForce;
            motorTorque = 0f;
        }
        rearLeft.motorTorque = motorTorque;
        rearRight.motorTorque = motorTorque;
        rearLeft.brakeTorque = brakeTorque;
        rearRight.brakeTorque = brakeTorque;
        frontLeft.brakeTorque = brakeTorque * 0.7f;
        frontRight.brakeTorque = brakeTorque * 0.7f;
    }
    void ApplySteering()
    {
        float steer = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) steer = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) steer = 1f;
        float steerAngle = steer * maxSteerAngle;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;
    }
    void ApplyDownforce()
    {
        rb.AddForce(-transform.up * downforce * rb.linearVelocity.magnitude);
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
