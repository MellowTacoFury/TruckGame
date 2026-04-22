using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class BGMusic : MonoBehaviour
{
    private StudioEventEmitter emitter;
    public static BGMusic instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if(AudioManager.instance.isPlayingBGMusic == true)
        {
            //dont play 
            Debug.Log("Playing twice");
        }
        else
        {
            Debug.Log("Playing once");
            emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.backgroundMusic, gameObject);
            AudioManager.instance.isPlayingBGMusic = true;
            emitter.Play();
        }
    }
}