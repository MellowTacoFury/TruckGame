using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] Settings settings;
    [SerializeField] string[] levelNames;
    private StudioEventEmitter emitter;

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
        
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settings.StartZoom();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(Random.Range(0, levelNames.Length));
    }
    public void Quit()
    {
        emitter.Stop();
        Application.Quit();
    }

}
