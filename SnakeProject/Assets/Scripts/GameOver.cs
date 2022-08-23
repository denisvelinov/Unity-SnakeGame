using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text runPoints;
    public TMP_Text highScorePoints;
    public void Setup (int runScore, int highScore)
    {
        gameObject.SetActive(true);
        runPoints.text = "Run Score\n" + runScore.ToString();
        highScorePoints.text = "High Score\n" + highScore.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
