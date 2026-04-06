using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] List<string> tutorialLevelTexts;
    [SerializeField] string buttonText;
    private int tutorialLevel = 0;
    private bool done = false;
    void Start()
    {
        tutorialLevel = 0;
        done = false;
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
            GameObject.Find("/GameManager").GetComponent<GameDataManager>().StartAfterTutorial();
        }
        else
        {
            tutorialText.text = tutorialLevelTexts[tutorialLevel] + "\n" + buttonText;
        }
    }
}
