using UnityEngine;

public class CarCollision : MonoBehaviour
{
    public int sponserHitMultiplier = 1;
    private Viewers viewers;
    void Start()
    {
        viewers = GameObject.Find("GameManager").GetComponent<Viewers>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hittable"))
        {
            Debug.Log("Hit: " + other.name);
            viewers.DoTrick(
                other.GetComponent<ItemSOscript>().data.PointsIfHit * sponserHitMultiplier,
                other.GetComponent<ItemSOscript>().data.TimeToAdd
                );
            //get the itemso, give the int to the viewers function
        }
            
        if(other.CompareTag("Car") || other.CompareTag("Enemy"))
        {
            Debug.Log("Hit: " + other.name);
            viewers.DoTrick(5,5);
            //if hittable - objects and cars, not ground
            //grab speed. Depending on speed, add some points?
        }
            
    }
}
