using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Brick brickPrefab;
    [SerializeField] Rigidbody ball;

    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text gameOverText;

    private int lineCount = 6;
    private int points = 0;
    private bool isStarted = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        
        MainManager.Instance.LoadBestScore();
        if (MainManager.Instance.BestScore != null)
        {
            bestScoreText.text = $"Score : {MainManager.Instance.BestScore.playerName} : {MainManager.Instance.BestScore.score}";
            bestScoreText.gameObject.SetActive(true);
        }
        else
        {
            bestScoreText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        points += point;
        scoreText.text = $"Score : {points}";
    }

    public void GameOver()
    {
        MainManager.Instance.UpdateBestScore(MainManager.Instance.CurrentPlayerName, points);
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
    }
}
