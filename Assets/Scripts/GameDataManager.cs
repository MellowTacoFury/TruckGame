using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameDataManager : MonoBehaviour
{
    public enum GameState
    {
        None, Selecting, Playing, GO, Tutorial
    }
    public GameState currentState;
    private bool gamePaused = false;
    [Header("Sponsers")]
    [Tooltip("1 is default for ints")]
    public List<Sponser> sponsers;
    public Sponser currentSponser {get; private set;}
    [Header("Choosing Car")]
    private int currentFocusCar = 0;
    public Camera normalCamera;
    [Header("UI")]
    public GameObject carSelectionUI;
    public GameObject hudUI;
    public GameObject settingsHolder;
    public GameObject pauseWindow;
    public GameObject gameOverWindow;
    public GameObject tutorialHolder;
    private bool doingTutorial = false;
    //choose sponser

    void Awake()
    {
        Time.timeScale = 1;
        TurnOffAll();
        Focus();
        currentState = GameState.Selecting;
        gamePaused = false;
        if(PlayerPrefs.GetInt("Tutorial") == 0)
        {
            doingTutorial = false;
        }
        else
        {
            doingTutorial = true;
        }
    }


    //Selection Stuff


    private void Focus()
    {
        carSelectionUI.SetActive(true);
        //Still Need to Show Stats
        for (int i = 0; i < sponsers.Count-1; i++)
        {
            sponsers[i].camera.SetActive(false);
            //Weird way cause its a struct...
            Sponser temp = sponsers[i];
            temp.isPlayerSponser = false;
            sponsers[i] = temp;
        }
        
        Sponser temp2 = sponsers[currentFocusCar];
        temp2.isPlayerSponser = true;
        sponsers[currentFocusCar] = temp2;
        normalCamera.GetComponent<Camera>().enabled = false;
        sponsers[currentFocusCar].camera.SetActive(true);

        hudUI.SetActive(false);
        carSelectionUI.SetActive(true);
    }
    public void NextCar()
    {
        sponsers[currentFocusCar].camera.SetActive(false);
        Sponser temp = sponsers[currentFocusCar];
        temp.isPlayerSponser = false;
        sponsers[currentFocusCar] = temp;
        if(currentFocusCar == sponsers.Count-1)
        {
            currentFocusCar = 0;
        }
        else
        {
            currentFocusCar++;
        }
        Sponser temp2 = sponsers[currentFocusCar];
        temp2.isPlayerSponser = true;
        sponsers[currentFocusCar] = temp2;
        sponsers[currentFocusCar].camera.SetActive(true);
    }
    public void PrevCar()
    {
        sponsers[currentFocusCar].camera.SetActive(false);
        Sponser temp = sponsers[currentFocusCar];
        temp.isPlayerSponser = false;
        sponsers[currentFocusCar] = temp;
        if(currentFocusCar == 0)
        {
            currentFocusCar = sponsers.Count-1;
        }
        else
        {
            currentFocusCar--;
        }
        Sponser temp2 = sponsers[currentFocusCar];
        temp2.isPlayerSponser = true;
        sponsers[currentFocusCar] = temp2;
        sponsers[currentFocusCar].camera.SetActive(true);
    }

    public void StartGame()
    {
        //false, no tutorial
        if(doingTutorial == false)
        {
            currentState = GameState.Playing;
            hudUI.SetActive(true);
        }
        else
        {
            //true, do tutorial
            currentState = GameState.Tutorial;
            tutorialHolder.SetActive(true);
        }

        //stuff that happens no matter what
        carSelectionUI.SetActive(false);
        normalCamera.GetComponent<Camera>().enabled = true;
        SetSponser();
        sponsers[currentFocusCar].camera.SetActive(false);
        settingsHolder.GetComponentInParent<Settings>().StartZoom();
        normalCamera.GetComponent<CameraFollow>().StartCamera();
        
    }
    public void StartAfterTutorial()
    {
        SetEnemyCars();
        currentState = GameState.Playing;
        hudUI.SetActive(true);
        tutorialHolder.SetActive(false);
        ApplyMultipliers();
    }

    //Sponser stuff
    public void SetSponser()
    {
        foreach (var sponser in sponsers)
        {
            if(sponser.isPlayerSponser == true)
            {
                currentSponser = sponser;
                if(currentSponser.car.GetComponent<PlayerCarInput>() == null)currentSponser.car.AddComponent<PlayerCarInput>();
                if(currentSponser.car.GetComponent<CarCollision>() == null)currentSponser.car.AddComponent<CarCollision>();
                if(currentSponser.car.GetComponent<AirAndDriftTimer>() == null)currentSponser.car.AddComponent<AirAndDriftTimer>();
                if(currentSponser.car.GetComponent<AICarController>())Destroy(currentSponser.car.GetComponent<AICarController>());
                Camera.main.gameObject.GetComponent<CameraFollow>().carTransform = currentSponser.car.transform;
                //set the actual car stuff here
                //add and remove scripts
                //set camera
                //etc
                currentSponser.car.tag = "Car";
                currentSponser.car.gameObject.name = "Player";
                currentSponser.car.GetComponent<PlayerCarInput>().playing = true;
                break;
            }
        }
        if(!doingTutorial)SetEnemyCars();
    }
    private void SetEnemyCars()
    {
        foreach (var sponser in sponsers)
        {
            if(sponser.isPlayerSponser == false)
            {
                sponser.car.GetComponent<AICarController>().player = GameObject.FindGameObjectWithTag("Car").gameObject.transform;
                sponser.car.GetComponent<AICarController>().StartMatch();
                sponser.car.GetComponent<AICarController>().playing = true;
                sponser.car.gameObject.name = "Enemy";
                sponser.car.tag = "Enemy";
            }
        }
    }
    public void ApplyMultipliers()
    {
        var timers = currentSponser.car.GetComponent<AirAndDriftTimer>();
        timers.sponserAirMultiplier = currentSponser.AirMultiplier;
        timers.sponserDriftMultiplier = currentSponser.DriftMultiplier;
        GameObject.FindGameObjectWithTag("Car").GetComponent<CarCollision>().sponserHitMultiplier = currentSponser.HitMultiplier;

    }




    //UI
    private void TurnOffAll()
    {
        settingsHolder.SetActive(false);
        hudUI.SetActive(false);
        carSelectionUI.SetActive(false);
        pauseWindow.SetActive(false);
        gameOverWindow.SetActive(false);
        tutorialHolder.SetActive(false);
    }

    public void Pause()
    {
        if(gamePaused == true)
        {
            //is paused, unpasueing
            Debug.Log("Unpausing");
            Time.timeScale = 1f;
            TurnOffAll();
            if(currentState == GameState.Selecting)
            {
                carSelectionUI.SetActive(true);
            }
            else if(currentState == GameState.Playing)
            {
                hudUI.SetActive(true);
            }
            gamePaused = false;
        }
        else if(gamePaused == false)
        {
            //is unpaused, pausing
            Debug.Log("pausing");
            Time.timeScale = 0f;
            TurnOffAll();
            pauseWindow.SetActive(true);
            gamePaused = true;
        }
        
    }

    public void OpenSettings()
    {
        settingsHolder.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsHolder.SetActive(false);
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void GameDone()
    {
        currentState = GameState.GO;
        TurnOffAll();
        gameOverWindow.SetActive(true);
        Time.timeScale = 0;


        //if played with a tutorial, turn it off
        if(doingTutorial == true)
        {
            PlayerPrefs.SetInt("Tutorial", 0);
        }
    }

}


[Serializable]
public struct Sponser
{
    public string SponserName;
    public bool isPlayerSponser;
    public int DriftMultiplier;
    public int HitMultiplier;
    public int AirMultiplier;
    //car game object to be player - remove ai and put player car scripts
    [Tooltip("The car and the scene")]
    public GameObject? car;
    [Tooltip("Directly give a ref to the camera that is under the car object")]
    public GameObject camera;
}