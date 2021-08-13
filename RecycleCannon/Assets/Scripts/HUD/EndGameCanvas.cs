using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameCanvas : MonoBehaviour
{
    public TextMeshProUGUI killText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI rewardText;

    private void OnEnable()
    {
        UpdateInformations();
    }

    public void UpdateInformations()
    {
        killText.text = GameManager.Instance.kills.ToString();
        timeText.text = GameManager.Instance.time.ToString("00:00");
        rewardText.text = ((int)(GameManager.Instance.kills * 20 + GameManager.Instance.kills * 20 * GameManager.Instance.time)).ToString();
        Time.timeScale = 0;
    }

    public void RestartClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GamePlay");
    }

    public void MenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}
