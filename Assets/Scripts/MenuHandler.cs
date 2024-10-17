using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

[DefaultExecutionOrder(1000)]
public class MenuHandler : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TMP_InputField playerNameInput;

    void Start()
    {
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

    public void OnPlayerNameInputEditEnd()
    {
        if (!string.IsNullOrEmpty(playerNameInput.text) && playerNameInput.text.All(char.IsLetterOrDigit))
        {
            MainManager.Instance.CurrentPlayerName = playerNameInput.text;
        }
        else
        {
            Debug.Log("Invalid player name: " + playerNameInput.text + "!");
            playerNameInput.text = null;
        }
    }

    public void OnStartButtonClick()
    {
        if (string.IsNullOrEmpty(MainManager.Instance.CurrentPlayerName))
        {
            Debug.Log("Setting the default player name to \"Anonymous\".");
            MainManager.Instance.CurrentPlayerName = "Anonymous";
        }
        SceneManager.LoadScene("Game");
    }

    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
