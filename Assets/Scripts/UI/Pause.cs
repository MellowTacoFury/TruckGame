using UnityEngine;

public class Pause : MonoBehaviour
{
    void Update()
    {
        // Check for the pause/unpause key press (e.g., Escape or P)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 0f)
            {
                GetComponent<GameDataManager>().Pause();
            }
            else
            {
                GetComponent<GameDataManager>().Pause();
            }
        }
    }
}
