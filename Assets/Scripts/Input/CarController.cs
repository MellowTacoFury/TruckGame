using UnityEngine;
using UnityEngine.InputSystem;
public class CarController : MonoBehaviour
{
    private PlayerInput carInputs;
    // wheel colliders
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] float acceleration = 500f;
    [SerializeField] float breakingForce = 300f;
    [SerializeField] float maxTurnAngle = 15f;

    float currentAcceleration = 0;
    float currentBreakForce = 0;
    float currentTurnAngle = 0;

    void Awake()
    {
        carInputs = new PlayerInput();
        carInputs.Truck.Enable();
    }
    void OnEnable()
    {
        carInputs.Truck.Enable();
        //subscribe to the events
        // carInputs.Truck.HandBrake.performed += OnHandbrakePerformed;
        // carInputs.Truck.HandBrake.canceled += OnHandbrakeCanceled;
    }
    private void OnDisable()
    {
        carInputs.Truck.Disable();
        // Unsubscribe from events
        // carInputs.Truck.HandBrake.performed -= OnHandbrakePerformed;
        // carInputs.Truck.HandBrake.canceled -= OnHandbrakeCanceled;
    }

    private void OnHandbrakePerformed(InputAction.CallbackContext context)
    {
        // Handle handbrake engaged logic
    }

    private void OnHandbrakeCanceled(InputAction.CallbackContext context)
    {
        // Handle handbrake released logic
    }

    void Update()
    {
        //read input
        // Debug.Log(carInputs.Truck.Movement.ReadValue<Vector2>());
    }
    void FixedUpdate()
    {
        //do input
        currentAcceleration = acceleration * carInputs.Truck.Movement.ReadValue<Vector2>().y;
        currentTurnAngle = maxTurnAngle * carInputs.Truck.Movement.ReadValue<Vector2>().x;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
    }
}
