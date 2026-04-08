using UnityEngine;

[RequireComponent(typeof(PrometeoCarController))]
public class PlayerCarInput : MonoBehaviour
{
    private PrometeoCarController car;
    public bool playing = false;

    void Awake()
    {
        car = GetComponent<PrometeoCarController>();
    }

    void Update()
    {
        if(playing == false)
        {
            return;
        }
        else
        {
            car.accelerationInput = Input.GetAxis("Vertical");
            car.steerInput = Input.GetAxis("Horizontal");
        }
        
    }

    // 💥 RAM FORCE
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform != null)
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