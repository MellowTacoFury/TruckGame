using UnityEngine;

public class AirAndDriftTimer : MonoBehaviour
{
    public int sponserDriftMultiplier = 1;
    // public int sponserAirMultiplier = 1;
    // [SerializeField] private float timer = 0;
    // [SerializeField] private bool isInAir = false;
    private PrometeoCarController car;
    private float driftTime = 0;
    private Viewers viewers;
    public GameObject UIPopup;
    void Start()
    {
        car = GameObject.Find("GameManager").GetComponent<GameDataManager>().currentSponser.car.GetComponent<PrometeoCarController>();
        viewers = GameObject.Find("GameManager").GetComponent<Viewers>();
    }
    // void OnTriggerExit(Collider other)
    // {
    //     if(other.CompareTag("Ground"))
    //     {
    //         isInAir = true;
    //     }
    // }
    // void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("Ground"))
    //     {
            
    //         isInAir = false;
    //         DoAirTrick();
                
    //     }
    // }
    void Update()
    {
        if(car.isDrifting == true)
        {
            driftTime += Time.deltaTime;
            if(GetComponent<GetCarEmitter>().driftEmitter.IsPlaying() == false)
                GetComponent<GetCarEmitter>().driftEmitter.Play();
        }
        else
        {
            DoDrift();
            GetComponent<GetCarEmitter>().driftEmitter.Stop();
        }


        // if(isInAir == true)
        // {
        //     timer += Time.deltaTime;
        // }
    }
    // private void DoAirTrick()
    // {
    //     if(timer > 1)
    //     {
    //         isInAir = false;
    //         int amount = (int)Mathf.Ceil(timer);
    //         viewers.DoTrick(amount*sponserAirMultiplier, amount);
    //         Debug.Log($"Air {amount*sponserAirMultiplier}");
    //         timer = 0;
    //         var g = Instantiate(UIPopup, transform.position + Vector3.up, Quaternion.identity);
    //         g.GetComponent<UIPopup>().DoText(amount*sponserAirMultiplier);
    //     }
        
    // }
    private void DoDrift()
    {
        if(driftTime != 0)
        {
            viewers.DoTrick((int)Mathf.Ceil(driftTime) * sponserDriftMultiplier, (int)driftTime*5);
           
            var g = Instantiate(UIPopup, transform.position + Vector3.up, Quaternion.identity);
            g.GetComponent<UIPopup>().DoText((int)Mathf.Ceil(driftTime)*sponserDriftMultiplier);
            
            
            driftTime = 0;
        }
        
    }
}
