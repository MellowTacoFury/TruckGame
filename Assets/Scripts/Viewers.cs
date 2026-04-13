using System.Collections;
using UnityEngine;
using TMPro;

public class Viewers : MonoBehaviour
{
    public bool Testing = false;
    public int viewerCount = 50;
    public int maxViewerCount;
    public float timeSinceLastTrick = 5f;
    public float maxTime = 60;
    public TextMeshProUGUI viewersText;
    public TextMeshProUGUI TimeText;
    private bool isLoosingViewers;
    /*
    level timer - gas gage?
    */
    void Start()
    {
        viewersText.text = "Viewers: " + viewerCount.ToString();
        maxViewerCount = viewerCount;
        TimeText.text = "Time Until Viewers Get Bored: \n" + timeSinceLastTrick.ToString("F2");
        timeSinceLastTrick = maxTime/2;
    }

    private void Update()
    {
        if(Testing == true)
        {
            return;
        }
        if(GetComponent<GameDataManager>().currentState == GameDataManager.GameState.Playing)
        {
            if(viewerCount <= 0 && GetComponent<GameDataManager>().currentState != GameDataManager.GameState.GO)
            {
                Debug.LogError("Loose");
                GetComponent<GameDataManager>().GameDone();
            }
            
            //---------------

            if(timeSinceLastTrick > 0)
            {
                timeSinceLastTrick -= Time.deltaTime;
                TimeText.text = "Time Until Viewers Get Bored: \n" + timeSinceLastTrick.ToString("F2");
                StopCoroutine(LooseViewers());
                isLoosingViewers = false;
            }
            else
            {
                timeSinceLastTrick = 0;
                TimeText.text = "Time Until Viewers Get Bored: \n" + timeSinceLastTrick.ToString("F2");
                //start the courutine
                if(isLoosingViewers == false)
                {
                    isLoosingViewers = true;
                    StartCoroutine(LooseViewers());
                }
            }
        }
    }
    public void DoTrick(int viewersToAdd, float timeToAdd)
    {
        viewerCount += viewersToAdd;
        viewersText.text = "Viewers: " + viewerCount.ToString();

        if(timeSinceLastTrick+timeToAdd >= maxTime)
        {
            timeSinceLastTrick = maxTime;
        }
        else
        {
            timeSinceLastTrick += timeToAdd;
        }
        TimeText.text = "Time Until Viewers Get Bored: \n" + timeSinceLastTrick.ToString("F2");
        if(viewerCount > maxViewerCount)
        {
            maxViewerCount = viewerCount;
        }
        
    }
    private IEnumerator LooseViewers()
    {
        while(isLoosingViewers)
        {
            yield return new WaitForSecondsRealtime(1);
            viewerCount -= Random.Range(0, 10);
            viewersText.text = "Viewers: " + viewerCount.ToString();
        }
    }
}
