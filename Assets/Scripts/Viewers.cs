using System.Collections;
using UnityEngine;

public class Viewers : MonoBehaviour
{
    public int viewerCount = 50;
    public float timeSinceLastTrick = 5f;
    private bool isLoosingViewers;
    /*
    level timer - gas gage?
    int for viewer count
    timer for how long since last cool trick - coutotine to lower it
    PUBLIC cool trick function - adds viewers, resets cool trick timer
    if viewer count == 0, loose
    
    */


    private void Update()
    {
        if(viewerCount <= 0)
        {
            Debug.LogError("Loose");
        }
        
        //---------------

        if(timeSinceLastTrick > 0)
        {
            timeSinceLastTrick -= Time.deltaTime;
            StopCoroutine(LooseViewers());
            isLoosingViewers = false;
        }
        else
        {
            //start the courutine
            if(isLoosingViewers == false)
            {
                isLoosingViewers = true;
                StartCoroutine(LooseViewers());
            }
        }

    }
    public void DoTrick(int viewersToAdd, float timeToAdd)
    {
        viewerCount += viewersToAdd;
        timeSinceLastTrick += timeToAdd;
    }
    private IEnumerator LooseViewers()
    {
        while(isLoosingViewers)
        {
            yield return new WaitForSecondsRealtime(1);
            viewerCount -= 1;
        }
    }
}
