using UnityEngine;

public class AirAndDriftTimer : MonoBehaviour
{
    public int sponserDriftMultiplier = 1;
    public int sponserAirMultiplier = 1;
    private bool initialGround = true;
    private float timer = 0;
    private bool isInAir = false;
    private PrometeoCarController car;
    private float driftTime = 0;
    private Viewers viewers;
    void Start()
    {
        car = GameObject.Find("GameManager").GetComponent<GameDataManager>().currentSponser.car.GetComponent<PrometeoCarController>();
        viewers = GameObject.Find("GameManager").GetComponent<Viewers>();
    }
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
            if(initialGround == true)
            {
                initialGround = false;
                
            }
            else
            {
                isInAir = false;
                DoAirTrick();
            }
                
        }
    }
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


        if(isInAir == true)
        {
            timer += Time.deltaTime;
        }
    }
    private void DoAirTrick()
    {
        if(timer != 0)
        {
            isInAir = false;
            int amount = (int)Mathf.Ceil(timer)*2;
            viewers.DoTrick(amount*sponserAirMultiplier, amount);
            timer = 0;
        }
        
    }
    private void DoDrift()
    {
        if(driftTime != 0)
        {
            viewers.DoTrick((int)Mathf.Ceil(driftTime) * sponserDriftMultiplier, (int)driftTime);
            driftTime = 0;
        }
        
    }
}
