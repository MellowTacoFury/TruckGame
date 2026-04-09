using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] Settings settings;
    private StudioEventEmitter emitter;


    void Start()
    {
        Time.timeScale = 1;
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.backgroundMusic, this.gameObject);
        emitter.Play();
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
    public void Play(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void Quit()
    {
        emitter.Stop();
        Application.Quit();
    }

}
