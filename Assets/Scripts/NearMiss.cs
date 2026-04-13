using UnityEngine;

public class NearMiss : MonoBehaviour
{
    public bool somethingHitCar;
    public GameObject UIPopup;
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
        }
        somethingHitCar = false;
    }
}
