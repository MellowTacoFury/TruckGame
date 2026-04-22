using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]

public class NearMiss : MonoBehaviour
{
    public bool somethingHitCar;
    public GameObject UIPopup;
    private StudioEventEmitter emitter;

    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Car") || other.CompareTag("Hittable"))
        {
            WasThereAMiss();
        }
    }
    void WasThereAMiss()
    {
        if(somethingHitCar == true)
        {
            // Debug.Log("HIT");
        }
        else
        {
            // Debug.Log("MISS!!!!!");
            GameObject.Find("GameManager").GetComponent<Viewers>().DoTrick(5, 15);
            var g = Instantiate(UIPopup, transform.position + Vector3.up, Quaternion.identity);
            g.GetComponent<UIPopup>().DoText(5);
            emitter.Play();
        }
        somethingHitCar = false;
    }
}
