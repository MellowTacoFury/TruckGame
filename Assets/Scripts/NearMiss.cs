using UnityEngine;

public class NearMiss : MonoBehaviour
{
    public bool somethingHitCar;

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
            Debug.Log("HIT");
        }
        else
        {
            Debug.Log("MISS!!!!!");
            GameObject.Find("GameManager").GetComponent<Viewers>().DoTrick(5, 15);
        }
        somethingHitCar = false;
    }
}
