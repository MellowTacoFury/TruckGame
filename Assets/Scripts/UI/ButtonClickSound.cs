using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class ButtonClickSound : MonoBehaviour
{
    private StudioEventEmitter emitter;

    private void Start()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager.instance is null. Make sure AudioManager exists in the scene before ButtonClickSound runs.");
            return;
        }

        if (FMODEvents.instance == null)
        {
            Debug.LogError("FMODEvents.instance is null. Make sure FMODEvents exists in the scene.");
            return;
        }

        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.buttonClick, gameObject);

        if (emitter != null)
            emitter.Stop();
    }

    public void Click()
    {
        if (AudioManager.instance == null || FMODEvents.instance == null)
            return;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonClick);
    }
}