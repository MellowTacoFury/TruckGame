using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class Viewers : MonoBehaviour
{
    public bool Testing = false;
    public int viewerCount = 50;
    public int maxViewerCount;
    public float timeSinceLastTrick = 5f;
    public float maxTime = 60;
    [Header("UI")]
    public Slider viewerSlider;
    public TextMeshProUGUI viewerText;
    public Image emojiImage;
    public Sprite[] emojis;
    private bool ableToChange = false;
    private bool alreadyZero = false;
    private bool AlreadyPlayedHigh = false;
    private bool AlreadyPlayedLow = false;
    private bool AlreadyPlayedMod = false;
    private bool AlreadyPlayed69 = false;
    
    private bool isLoosingViewers;
    /*
    level timer - gas gage?
    */
    void Start()
    {
        maxViewerCount = viewerCount;
        timeSinceLastTrick = maxTime;
        viewerSlider.maxValue = maxViewerCount;
        viewerSlider.value = viewerCount;
        viewerText.text = $"Viewers: {viewerCount}";
    }

    private void Update()
    {
        if(Testing == true || GetComponent<GameDataManager>().gamePaused == true)
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

            if(timeSinceLastTrick >= 0)
            {
                
                timeSinceLastTrick -= Time.deltaTime;
                StopCoroutine(LooseViewers());
                isLoosingViewers = false;
                alreadyZero = false;
            }
            else
            {
                timeSinceLastTrick = 0;
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
        viewerSlider.value = viewerCount;
        viewerText.text = $"Viewers: {viewerCount}";

        if(timeSinceLastTrick+timeToAdd >= maxTime)
        {
            timeSinceLastTrick = maxTime;
        }
        else
        {
            timeSinceLastTrick += timeToAdd;
        }
        if(viewerCount > maxViewerCount)
        {
            maxViewerCount = viewerCount;
            viewerSlider.maxValue = maxViewerCount;
        }
        ImageChanger(true);
        
    }
    private IEnumerator LooseViewers()
    {
        while(isLoosingViewers)
        {
            yield return new WaitForSecondsRealtime(1);
            viewerCount -= Random.Range(0, 10);
            viewerSlider.value = viewerCount;
            viewerText.text = $"Viewers: {viewerCount}";
            ImageChanger();
        }
    }
    private IEnumerator HappyImageTimer()
    {
        yield return new WaitForSecondsRealtime(1);
        ableToChange = true;
        ImageChanger();
    }

    private void ImageChanger(bool gotPoints = false)
    {
        if(gotPoints == true)
        {
            //set to hell yeah happy, for a few seconds
            emojiImage.sprite = emojis[0];
            ableToChange = false;
            StartCoroutine(HappyImageTimer());
            //start timer for it
        }
        else if(ableToChange == true && timeSinceLastTrick > 0 && !(viewerCount <= (maxViewerCount/2)))
        {
            //set image to happy
            emojiImage.sprite = emojis[1];
        }
        else if(timeSinceLastTrick <= 0 && alreadyZero == false)
        {
            //set image to meh
            emojiImage.sprite = emojis[2];
            alreadyZero = true;
            if(AlreadyPlayedMod == false && GetComponent<GameDataManager>().currentState == GameDataManager.GameState.Playing)
            {
                AlreadyPlayedMod = true;
                GetComponent<GameDataManager>().PlaySound(FMODEvents.instance.Moderate);
            }
        }
        else if(viewerCount <= (maxViewerCount/3))
        {
            //set image to danger
            emojiImage.sprite = emojis[3];
            if(AlreadyPlayedLow == false && GetComponent<GameDataManager>().currentState == GameDataManager.GameState.Playing)
            {
                AlreadyPlayedLow = true;
                GetComponent<GameDataManager>().PlaySound(FMODEvents.instance.Low);
            }
        }


        //Check specific value
        if(viewerCount == 69 && AlreadyPlayed69 == false && GetComponent<GameDataManager>().currentState == GameDataManager.GameState.Playing)
        {
            AlreadyPlayed69 = true;
            GetComponent<GameDataManager>().PlaySound(FMODEvents.instance.Viewers68);
        }
        if(viewerCount > 100 && AlreadyPlayedHigh == false && GetComponent<GameDataManager>().currentState == GameDataManager.GameState.Playing)
        {
            AlreadyPlayedHigh = true;
            GetComponent<GameDataManager>().PlaySound(FMODEvents.instance.High);
        }
    }
}
