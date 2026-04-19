using UnityEngine;

public class LowResFullscreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(240, 180, FullScreenMode.ExclusiveFullScreen);        
    }


}
