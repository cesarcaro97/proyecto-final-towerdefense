using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance { get; set; } = null;

    [SerializeField] TMP_Text winnerText = null;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetWinner(string winner)
    {
        winnerText.text = $"{winner} wins the game.";
    }

    public void OnExit_ButtonClick()
    {
        Application.Quit();
    }

    public void OnPlayAgain_ButtonClick()
    {
        SceneManager.LoadScene("DesignScene");
    }
}