using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int combo = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddCombo()
    {
        combo += 1;
        UpdateUI();
    }

    public void AddScore()
    {
        // スコアはコンボ数の2乗を加算
        score += combo * combo;
        UpdateUI();
    }

    public void Miss()
    {
        combo = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        if (comboText != null)
            comboText.text = "Combo: " + combo.ToString();
    }
}
