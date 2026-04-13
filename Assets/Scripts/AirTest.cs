using UnityEngine;

public class AirTest : MonoBehaviour
{
    public int sponserAirMultiplier = 1;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool isInAir = false;
    public GameObject UIPopup;
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            isInAir = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            
            isInAir = false;
            DoAirTrick();
                
        }
    }
    void Update()
    {
        if(isInAir == true)
        {
            timer += Time.deltaTime;
        }
    }
    private void DoAirTrick()
    {
        if(timer > 1)
        {
            isInAir = false;
            int amount = (int)Mathf.Ceil(timer);
            GameObject.Find("GameManager").GetComponent<Viewers>().DoTrick(amount*sponserAirMultiplier, amount);
            Debug.Log($"Air {amount*sponserAirMultiplier}");
            timer = 0;
            var g = Instantiate(UIPopup, transform.position + Vector3.up, Quaternion.identity);
            g.GetComponent<UIPopup>().DoText(amount*sponserAirMultiplier);
        }
        
    }
}