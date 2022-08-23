using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public TMP_Text runPoints;
    public TMP_Text highScorePoints;

    public void ScoreSetup(int runScore, int highScore)
    {
        runPoints.text = "Score:\n" + runScore.ToString();
        highScorePoints.text = "High Score:\n" + highScore.ToString();
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
