using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.IO;
[Serializable]
public class ScoreEntry {
    public string name;
    public int score;
}

public class LeaderboardManager : MonoBehaviour
{
    [Serializable]
    private class ScoreListWrapper {
        public ScoreEntry[] highScores;
    }

    public char[] currentLetter;
    public TextMeshProUGUI[] rankTexts;   // 5 items
    public TextMeshProUGUI[] nameTexts;   // 5 items
    public TextMeshProUGUI[] scoreTexts;  // 5 items
    public TextMeshProUGUI playerRankText; 
    public TextMeshProUGUI[] entryLetters; // 3 items for name entry
    public GameObject letterIndicator;     // e.g. arrow under current letter
    public GameObject nameEntryPanel;      // parent of name-entry UI
    public GameObject leaderboardPanel;
    public List<ScoreEntry> leaderboard = new List<ScoreEntry>();
    public int currentScore;           // score of the player who just lost
    public int currentLetterIndex = 0; // 0-2 for which letter we are editing
    public InputAction navigateAction;
    public InputAction submitAction;
    public GameObject playAgainButton;
    public GameObject QuitButton;
    public GameObject EnterButton;
    public Viewers v;
    private bool entering = false;
    
    void OnEnable()
    {
        currentLetterIndex = 0;
        currentLetter = new char[4]; 
        // nameEntryPanel.SetActive(false);
    }
    public void Start() {
        LoadLeaderboard();
        UpdateLeaderboardUI(null);
        playAgainButton.SetActive(false);
        QuitButton.SetActive(false);
}
    void Update() {
        if (!nameEntryPanel.activeSelf) return; // only while entering name
        
        bool moved = false;
        currentScore = v.maxViewerCount;
        
       
        if (navigateAction.triggered)
        {
            Vector2 nav = navigateAction.ReadValue<Vector2>();
           // if (nav.x < 0 && currentLetterIndex > 0) { currentLetterIndex--; moved = true; }
           // else if (nav.x > 0 && currentLetterIndex < 2) { currentLetterIndex++; moved = true; }

            // Vertical movement: change letter character
            if (nav.y > 0) ChangeCurrentLetter(1);
            else if (nav.y < 0) ChangeCurrentLetter(-1);
            moved = true;
            
                UpdateLetterIndicator();
          
           
            Debug.Log(nav);
        }

       
        for (int i = 0; i < entryLetters.Length; i++)
        {
            entryLetters[i].text = currentLetter[i].ToString();
        }
        
       
        // Submit pressed: either move to next letter or finish
        if (submitAction.triggered || entering == true) {
            if (currentLetterIndex < 2) {
                Debug.Log(entryLetters[currentLetterIndex].text);
                Debug.Log("Submitted");
                currentLetterIndex++;
                UpdateLetterIndicator();
                
            } else {
                // Done entering 3 letters
                nameEntryPanel.SetActive(false);
                navigateAction.Disable();
                submitAction.Disable();
                string playerName = entryLetters[0].text + entryLetters[1].text + entryLetters[2].text;
                AddScore(playerName, currentScore);
                entryLetters[0].text = playerName;
                playAgainButton.SetActive(true);
                QuitButton.SetActive(true);
                EnterButton.SetActive(false);
            }
           entering = false;
          
        }
        
    }
    public void ChangeCurrentLetter(int delta) {
        if (entryLetters[currentLetterIndex] == null) {
            Debug.LogError("Entry letter UI at index " + currentLetterIndex + " is null!");
            return;
        }

        if (string.IsNullOrEmpty(entryLetters[currentLetterIndex].text)) {
            entryLetters[currentLetterIndex].text = "A";
        }

        char c = char.ToUpper(entryLetters[currentLetterIndex].text[0]);
        c = (char)(c + delta);

        if (c > 'Z') c = 'A';
        else if (c < 'A') c = 'Z';

        entryLetters[currentLetterIndex].text = c.ToString();
        Debug.Log($"Set letter {currentLetterIndex} to {c}");
        currentLetter[currentLetterIndex] = c;
    }

    public void EnterName()
    {
        entering = true;
    }

    void UpdateLetterIndicator() {
        // For example, position an arrow GameObject under the current letter:
        letterIndicator.transform.position = entryLetters[currentLetterIndex].transform.position;
    }
    void AddScore(string scoreName, int score) {
        ScoreEntry newEntry = new ScoreEntry { name = scoreName, score = score };
        leaderboard.Add(newEntry);
        // Sort descending by score
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        // Optionally trim to a max number of entries, e.g. 100:
        //if (leaderboard.Count > 100) leaderboard.RemoveRange(100, leaderboard.Count - 100);
        SaveLeaderboard();
        UpdateLeaderboardUI(newEntry);
    }


    void LoadLeaderboard() {
        string path = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
            if (wrapper != null && wrapper.highScores != null)
                leaderboard = new List<ScoreEntry>(wrapper.highScores);
            leaderboard.Sort((a,b) => b.score.CompareTo(a.score));
        } else {
            // Initialize and save an empty leaderboard
            leaderboard = new List<ScoreEntry>();
            SaveLeaderboard();
        }
    }

    void UpdateLeaderboardUI(ScoreEntry entry) {
        for (int i = 0; i < 5; i++) {
            if (i < leaderboard.Count) {
                rankTexts[i].text = (i+1).ToString() + ".";
                nameTexts[i].text = leaderboard[i].name;
                scoreTexts[i].text = leaderboard[i].score.ToString();
            } else {
                // No entry: clear the text
                rankTexts[i].text = "";
                nameTexts[i].text = "";
                scoreTexts[i].text = "";
            }
        }

        // Show player's rank if outside top 10:
        // (Assuming the last added entry is the player’s score)
        ScoreEntry playerEntry = leaderboard.Find(e => e.score == currentScore && e.name.Length == 3);
        if (entry != null) {
            int rank = leaderboard.IndexOf(playerEntry) + 1;
            if (rank > 5) {
                playerRankText.text = $"Your Score: {entry.score}  (Rank {rank})";
                playerRankText.gameObject.SetActive(true);
            } else {
                playerRankText.text = $"Your Score: {entry.score}  (Rank {rank})";
                playerRankText.gameObject.SetActive(true);
            }
        }
    }

    void SaveLeaderboard() {
        ScoreListWrapper wrapper = new ScoreListWrapper { highScores = leaderboard.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true);
        string path = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        File.WriteAllText(path, json);
    }

    
    public void OnPlayerLost() {
        // currentScore = score;
        leaderboardPanel.SetActive(true);
        nameEntryPanel.SetActive(true);
      
        // Initialize all letters to 'A'
      
        UpdateLetterIndicator();
    
        // Enable input actions (assuming we set them up)
        navigateAction.Enable();
        submitAction.Enable();
    }

    
    
}


