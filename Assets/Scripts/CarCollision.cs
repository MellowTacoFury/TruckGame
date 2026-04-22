using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class CarCollision : MonoBehaviour
{
    public int sponserHitMultiplier = 1;
    private Viewers viewers;
    public NearMiss nearMiss;
    public AirTest airTest;
    public GameObject UIPopup;
    private bool hitInThisInstance;
    private StudioEventEmitter emitter;
    void Start()
    {
        viewers = GameObject.Find("GameManager").GetComponent<Viewers>();
        GetComponent<GetCarEmitter>().crashEmitter.Stop();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hittable") && hitInThisInstance == false)
        {
            hitInThisInstance = true;
            viewers.DoTrick(
                other.GetComponent<ItemSOscript>().data.PointsIfHit * sponserHitMultiplier,
                other.GetComponent<ItemSOscript>().data.TimeToAdd
                );
            var g = Instantiate(UIPopup, transform.position + Vector3.up, Quaternion.identity);
            g.GetComponent<UIPopup>().DoText(other.GetComponent<ItemSOscript>().data.PointsIfHit * sponserHitMultiplier);
            //get the itemso, give the int to the viewers function
        }
            
        if(other.CompareTag("Car") || other.CompareTag("Enemy") && hitInThisInstance == false)
        {
            hitInThisInstance = true;
            viewers.DoTrick(5,15);
            var g = Instantiate(UIPopup, transform.position + Vector3.up, Quaternion.identity);
            g.GetComponent<UIPopup>().DoText(5);
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
            emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.CrashVoice, gameObject);
            if(emitter.IsPlaying() == false)
                emitter.Play();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Hittable") || other.CompareTag("Car") || other.CompareTag("Enemy"))
        {
            hitInThisInstance = false;
        }
    }
}
