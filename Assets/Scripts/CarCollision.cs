using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class CarCollision : MonoBehaviour
{
    public int sponserHitMultiplier = 1;
    private Viewers viewers;
    public NearMiss nearMiss;
    void Start()
    {
        viewers = GameObject.Find("GameManager").GetComponent<Viewers>();
        GetComponent<GetCarEmitter>().crashEmitter.Stop();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hittable"))
        {
            viewers.DoTrick(
                other.GetComponent<ItemSOscript>().data.PointsIfHit * sponserHitMultiplier,
                other.GetComponent<ItemSOscript>().data.TimeToAdd
                );
            //get the itemso, give the int to the viewers function
        }
            
        if(other.CompareTag("Car") || other.CompareTag("Enemy"))
        {
            viewers.DoTrick(5,5);
            //if hittable - objects and cars, not ground
            //grab speed. Depending on speed, add some points?
        }
            
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hittable"))
        {
            nearMiss.somethingHitCar = true;
            GetComponent<GetCarEmitter>().crashEmitter.Play();
        }
    }
}
