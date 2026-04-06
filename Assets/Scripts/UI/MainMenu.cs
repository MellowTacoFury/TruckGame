using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;


    void Start()
    {
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
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
        Application.Quit();
    }

}
