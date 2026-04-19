using UnityEngine;

[RequireComponent(typeof(PrometeoCarController))]
public class AICarController : MonoBehaviour
{
    public Transform player;

    [Header("Driving")]
    public float accel = 1f;
    public float steerStrength = 1f;

    [Header("Aggression Tuning")]
    public float baseAggression = 0.7f; //Somewhere between 0.3 and 0.7 is usually good
    public float turnDamping = 0.7f;

    [Header("Raycast Avoidance")]
    public int rayCount = 5;
    public float raySpread = 60f;
    public float rayDistance = 12f;
    public float avoidWeight = 2f;
    public LayerMask obstacleMask;

    [Header("Forward Stuck Detection")]
    public float stuckCheckDistance = 3f;
    public float stuckTimeThreshold = 1.2f;

    [Header("Low Speed Stuck")]
    public float minMoveSpeed = 0.5f;
    public float noMoveTime = 1.0f;

    private PrometeoCarController car;
    private Rigidbody rb;

    // Reverse system
    private float stuckTimer = 0f;
    private bool isReversing = false;
    private float reverseTimer = 0f;

    // Panic system
    private float noMoveTimer = 0f;
    private bool isPanicking = false;
    private float panicTimer = 0f;
    public bool playing = false;
    public void StartMatch()
    {
        car = GetComponent<PrometeoCarController>();
        rb = GetComponent<Rigidbody>();
        car.useAI = true;
    }

    void Update()
    {
        if(playing == false)
        {
            return;
        }
        else
        {
        if (player == null) return;
        }
        // PRIORITY SYSTEM
        if (isReversing)
            return;

        if (isPanicking)
        {
            PanicMove();
            return;
        }

        HandleForwardStuck();
        HandleLowSpeedStuck();

        Drive();
    }

    void FixedUpdate()
    {
        if (isReversing)
            Reverse();
    }

    // =========================
    // MAIN DRIVE
    // =========================
    void Drive()
    {
        Vector3 targetDir = (player.position - transform.position).normalized;
        Vector3 localTarget = transform.InverseTransformDirection(targetDir);

        float steerToPlayer = Mathf.Clamp(localTarget.x * turnDamping, -1f, 1f);
        float avoidSteer = GetRaycastAvoidance();

        float finalSteer = steerToPlayer + avoidSteer;
        finalSteer = Mathf.Clamp(finalSteer, -1f, 1f);

        float finalAccel = accel * baseAggression;

        if (Mathf.Abs(finalSteer) > 0.5f || Mathf.Abs(avoidSteer) > 0.2f)
            finalAccel *= 0.6f;

        car.steerInput = finalSteer * steerStrength;
        car.accelerationInput = finalAccel;
    }

    // =========================
    // MULTI-RAY AVOIDANCE
    // =========================
    float GetRaycastAvoidance()
    {
        float steer = 0f;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-raySpread / 2f, raySpread / 2f, t);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(origin, dir, out RaycastHit hit, rayDistance, obstacleMask))
            {
                if (hit.transform == player)
                    continue;

                float side = Vector3.Dot(transform.right, dir);
                float strength = 1f - (hit.distance / rayDistance);

                steer -= side * strength * avoidWeight;

                Debug.DrawRay(origin, dir * hit.distance, Color.red);
            }
            else
            {
                Debug.DrawRay(origin, dir * rayDistance, Color.green);
            }
        }

        return steer;
    }

    // =========================
    // FORWARD STUCK → REVERSE
    // =========================
    void HandleForwardStuck()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, stuckCheckDistance, obstacleMask))
        {
            if (hit.transform != player)
            {
                stuckTimer += Time.deltaTime;

                if (stuckTimer > stuckTimeThreshold)
                {
                    isReversing = true;
                    reverseTimer = Random.Range(2f, 3f);
                }
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    void Reverse()
    {
        reverseTimer -= Time.deltaTime;

        car.accelerationInput = -1f;
        car.steerInput = Random.Range(-1f, 1f);

        rb.linearVelocity *= 0.9f;

        if (reverseTimer <= 0f)
        {
            isReversing = false;
            stuckTimer = 0f;
        }
    }

    // =========================
    // LOW SPEED STUCK → PANIC
    // =========================
    void HandleLowSpeedStuck()
    {
        float speed = rb.linearVelocity.magnitude;

        if (speed < minMoveSpeed)
        {
            noMoveTimer += Time.deltaTime;

            if (noMoveTimer > noMoveTime)
            {
                isPanicking = true;
                panicTimer = Random.Range(1f, 2f);
            }
        }
        else
        {
            noMoveTimer = 0f;
        }
    }

    void PanicMove()
    {
        panicTimer -= Time.deltaTime;

        car.steerInput = Random.Range(-1f, 1f);
        car.accelerationInput = Mathf.Sin(Time.time * 8f);

        if (Random.value < 0.1f)
            car.Jump(0.5f);

        rb.linearVelocity *= 0.95f;

        if (panicTimer <= 0f)
        {
            isPanicking = false;
            noMoveTimer = 0f;
        }
    }

    // =========================
    // RAM PLAYER
    // =========================
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player)
        {
            Rigidbody otherRb = collision.rigidbody;
            if (otherRb != null)
            {
                otherRb.AddForce(transform.forward * 5000f);
            }
        }
    }
}