using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]

public class BusJump : MonoBehaviour
{
    public StudioEventEmitter emitter;
    private bool playedOnce = false;
    void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            if(playedOnce == false)
            {
                if(emitter.IsPlaying() == false)
                {
                    emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.BusJump, gameObject);
                    emitter.Play();
                    playedOnce = true;
                }
            }
        }
    }
}
