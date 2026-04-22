using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] List<string> tutorialLevelTexts;
    [SerializeField] string buttonText;
    private int tutorialLevel = 0;
    private bool done = false;
    private StudioEventEmitter emitter;
    void Start()
    {
        tutorialLevel = 0;
        done = false;
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.EndOfTutorial, gameObject);
        emitter.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(done == true)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            tutorialLevel++;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if(tutorialLevel == tutorialLevelTexts.Count)
        {
            //end tutorial
            done = true;
            GameObject.Find("/AllPrefabPieces/GameManager").GetComponent<GameDataManager>().StartAfterTutorial();
            emitter.Play();
        }
        else
        {
            tutorialText.text = tutorialLevelTexts[tutorialLevel] + "\n" + buttonText;
        }
    }
}
