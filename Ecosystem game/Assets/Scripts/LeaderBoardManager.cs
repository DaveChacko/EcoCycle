using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerRecord
{
    public string playerName;
    public int daysSurvived;
    public int speciesAlive;

    public PlayerRecord(string name, int days, int species)
    {
        playerName = name;
        daysSurvived = days;
        speciesAlive = species;
    }
}

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leaderboardText;

    private List<PlayerRecord> leaderboard = new List<PlayerRecord>();

    void Start()
    {
        LoadLeaderboard();
        DisplayLeaderboard();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ClearLeaderboard();
        }
    }

    public void AddRecord(string playerName, int daysSurvived, int speciesAlive)
    {
        leaderboard.Add(new PlayerRecord(playerName, daysSurvived, speciesAlive));

        leaderboard = leaderboard
            .OrderByDescending(p => p.daysSurvived)
            .ThenByDescending(p => p.speciesAlive)
            .ToList();

        SaveLeaderboard();
        DisplayLeaderboard();
    }

    void DisplayLeaderboard()
    {
        leaderboardText.text = "LEADERBOARD\n\n";
        int rank = 1;

        foreach (var record in leaderboard.Take(5))
        {
            leaderboardText.text += $"{rank}. {record.playerName} - {record.daysSurvived} days, {record.speciesAlive} species\n";
            rank++;
        }
    }

    void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new Wrapper { records = leaderboard });
        PlayerPrefs.SetString("Leaderboard", json);
    }

    void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey("Leaderboard"))
        {
            string json = PlayerPrefs.GetString("Leaderboard");
            leaderboard = JsonUtility.FromJson<Wrapper>(json).records;
        }
    }
    private void ClearLeaderboard()
    {
        if (PlayerPrefs.HasKey("Leaderboard"))
    {
        PlayerPrefs.DeleteKey("Leaderboard"); 
        PlayerPrefs.Save();
        leaderboard.Clear();
        DisplayLeaderboard(); 
        Debug.Log("Leaderboard cleared via keyboard!");
    }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<PlayerRecord> records;
    }
}
