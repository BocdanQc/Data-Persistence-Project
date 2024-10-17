using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public BestScoreData BestScore { get; set; }
    public string CurrentPlayerName { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class BestScoreData
    {
        public string playerName;
        public int score;
        public BestScoreData(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }

    public void UpdateBestScore(string playerName, int score)
    {
        if (BestScore == null || BestScore.score <= score)
        {
            BestScore = new BestScoreData(playerName, score);
            string json = JsonUtility.ToJson(BestScore);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    public void LoadBestScore()
    {
        string saveFilepath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(saveFilepath))
        {
            string json = File.ReadAllText(saveFilepath);
            BestScore = JsonUtility.FromJson<BestScoreData>(json);
        }
    }
}
