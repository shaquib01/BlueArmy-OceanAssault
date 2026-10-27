using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Text crowdText;
    [SerializeField] Text coinsText;
    [SerializeField] Slider bossHealthSlider;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;
    int coins;
    bool ended;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (resultPanel) resultPanel.SetActive(false);
    }
    void Start() { RefreshCoins(); }
    public void SetCrowdCount(int count) { if (crowdText) crowdText.text = "SOLDIERS  " + count; }
    public void SetBossHealth(int current, int max)
    {
        if (!bossHealthSlider) return;
        bossHealthSlider.maxValue = Mathf.Max(1, max);
        bossHealthSlider.value = current;
    }
    public void TargetDefeated(Damageable target, int reward, bool isBoss)
    {
        coins += Mathf.Max(0, reward);
        RefreshCoins();
        if (isBoss) EndRun(true);
    }
    public void EndRun(bool won)
    {
        if (ended) return;
        ended = true;
        Time.timeScale = 0f;
        if (resultPanel) resultPanel.SetActive(true);
        if (resultText) resultText.text = won ? "VICTORY!  +" + coins + " COINS" : "RUN OVER";
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void RefreshCoins() { if (coinsText) coinsText.text = "COINS  " + coins; }
}
