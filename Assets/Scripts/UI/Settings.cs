using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Image dimmer;
    [SerializeField] Slider brightness;
    [SerializeField] Slider volume;
    [SerializeField] Slider zoom;
    [SerializeField] Toggle tutorialToggle;

    private void Start()
    {
        //set the default playerprefs - if does not exist, set to a default
        //brightness
        if(PlayerPrefs.GetInt("Tutorial", -1) == -1)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
        }
        tutorialToggle.isOn = IntToBool(PlayerPrefs.GetInt("Tutorial"));
        //brightness
        if(PlayerPrefs.GetFloat("Brightness", -1) == -1)
        {
            PlayerPrefs.SetFloat("Brightness", 0);
        }
        dimmer.color = new Color(dimmer.color.r, dimmer.color.g, dimmer.color.b, PlayerPrefs.GetFloat("Brightness"));
        brightness.value = PlayerPrefs.GetFloat("Brightness");
        //------------------------------------------------------
        if(PlayerPrefs.GetFloat("Volume", -1) == -1)
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }
        foreach (var audio in GameObject.FindGameObjectsWithTag("AudioSources"))
        {
            audio.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume");
        }
        volume.value = PlayerPrefs.GetFloat("Volume");
        


    }
    public void StartZoom()
    {
        //By itself, beacause the camera component is off at start
        //10 - 30ish, 25
        if(PlayerPrefs.GetFloat("Zoom", -1) == -1)
        {
            PlayerPrefs.SetFloat("Zoom", 25);
        }
        zoom.value = PlayerPrefs.GetFloat("Zoom");
        Camera.main.GetComponent<Camera>().fieldOfView = PlayerPrefs.GetFloat("Zoom");
    }

    public void ChangeBrightness()
    {
        dimmer.color = new Color(dimmer.color.r, dimmer.color.g, dimmer.color.b, brightness.value);
        PlayerPrefs.SetFloat("Brightness", brightness.value);
    }
    public void ChangeVolume()
    {
        foreach (var audio in GameObject.FindGameObjectsWithTag("AudioSources"))
        {
            audio.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume");
        }
        PlayerPrefs.SetFloat("Volume", volume.value);
    }
    public void ChangeZoom()
    {
        Camera.main.GetComponent<Camera>().fieldOfView = zoom.value;
        PlayerPrefs.SetFloat("Zoom", zoom.value);
    }
    
    public void SetTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", BoolToInt(tutorialToggle.isOn));
    }
    private bool IntToBool(int value)
    {
        switch (value)
        {
            case 0:
            return false;
            case 1:
            return true;
            default:
            return false;
        }
    }
    private int BoolToInt(bool value)
    {
        switch (value)
        {
            case true:
            return 1;
            case false:
            return 0;
        }
    }
}
