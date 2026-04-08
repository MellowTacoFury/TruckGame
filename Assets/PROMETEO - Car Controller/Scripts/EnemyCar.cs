using UnityEngine;

[RequireComponent(typeof(PrometeoCarController))]
public class AICarController : MonoBehaviour
{
    public Transform player;

    [Header("Vision")]
    public float viewDistance = 40f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Combat")]
    public float ramDistance = 12f;
    public float aggressiveSteerMultiplier = 1.8f;

    [Header("Wander")]
    public float wanderRadius = 60f;
    public float wanderReach = 10f;
    public float wanderSpeed = 0.5f;

    private PrometeoCarController car;
    private Rigidbody rb;
    private Rigidbody playerRb;

    private Vector3 wanderTarget;
    private bool canSeePlayer;

    // 🧠 STUCK SYSTEM (IMPROVED)
    private float stuckTimer = 0f;
    private bool isReversing = false;
    private float reverseTimer = 0f;
    private Vector3 lastMoveDir;

    public bool playing = false;
    public void StartMatch()
    {
        car = GetComponent<PrometeoCarController>();
        rb = GetComponent<Rigidbody>();

        // 🔒 FORCE AI MODE
        car.useAI = true;

        if (player != null)
            playerRb = player.GetComponent<Rigidbody>();
        PickNewWanderPoint();
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

        // 🚧 STUCK DETECTION (SMART)
        float speed = rb.linearVelocity.magnitude;

        float forwardDot = 0f;
        if (speed > 0.1f)
            forwardDot = Vector3.Dot(transform.forward, rb.linearVelocity.normalized);

        if ((speed < 2f || forwardDot < 0.2f) && car.accelerationInput > 0.5f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer > 1.2f && !isReversing)
        {
            isReversing = true;
            reverseTimer = Random.Range(1.5f, 2.5f);
            lastMoveDir = rb.linearVelocity;
        }

        // 🔙 HANDLE REVERSING (HARD OVERRIDE)
        if (isReversing)
        {
            ReverseOut();
            return;
        }

        // 👀 VISION
        canSeePlayer = CheckVision();

        if (canSeePlayer)
            Chase();
        else
            Wander();
        }
        
    }

    // 👀 PLAYER DETECTION
    bool CheckVision()
    {
        Vector3 dir = player.position - transform.position;
        float dist = dir.magnitude;

        if (dist > viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, dist, obstacleMask))
            return false;

        return true;
    }

    // 🚗 CHASE + RAM
    void Chase()
    {
        Vector3 target = player.position;

        if (playerRb != null)
            target += playerRb.linearVelocity * 0.5f;

        DriveTowards(target, 1f, true);
    }

    // 🧭 WANDER
    void Wander()
    {
        float dist = Vector3.Distance(transform.position, wanderTarget);

        if (dist < wanderReach)
            PickNewWanderPoint();

        DriveTowards(wanderTarget, wanderSpeed, false);
    }

    void PickNewWanderPoint()
    {
        Vector2 random = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(random.x, transform.position.y, random.y);
    }

    // 🚙 DRIVING LOGIC
    void DriveTowards(Vector3 target, float accelMultiplier, bool aggressive)
    {
        Vector3 localTarget = transform.InverseTransformPoint(target);

        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float accel = accelMultiplier;

        float distance = Vector3.Distance(transform.position, target);

        if (aggressive && distance < ramDistance)
        {
            steer *= aggressiveSteerMultiplier;
            accel = 1f;
        }
        else
        {
            if (Mathf.Abs(steer) > 0.5f)
                accel *= 0.6f;
        }

        // Small randomness
        steer += Random.Range(-0.03f, 0.03f);

        car.steerInput = steer;
        car.accelerationInput = accel;
    }

    // 🔙 ESCAPE WALLS (FIXED)
    void ReverseOut()
    {
        reverseTimer -= Time.deltaTime;
        // FULL reverse
        car.accelerationInput = -1f;

        // 🧠 STEER AWAY FROM STUCK DIRECTION
        float steerDir = Vector3.Dot(transform.right, lastMoveDir) > 0 ? -1f : 1f;
        car.steerInput = steerDir;

        // 🔥 reduce sticking/sliding
        rb.linearVelocity *= 0.9f;

        // stop Prometeo braking logic
        CancelInvoke("DecelerateCar");

        if (reverseTimer <= 0f)
        {
            isReversing = false;
            stuckTimer = 0f;
        }

    }

    // 💥 RAM FORCE
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player)
        {
            Rigidbody otherRb = collision.rigidbody;
            if (otherRb != null)
            {
                Vector3 force = transform.forward * 5000f;
                otherRb.AddForce(force);
            }
        }
    }
}