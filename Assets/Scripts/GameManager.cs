using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const string WalletKey = "BlueArmy.WalletCoins.v1";

    public static GameManager Instance { get; private set; }
    [SerializeField] Text crowdText;
    [SerializeField] Text coinsText;
    [SerializeField] Slider bossHealthSlider;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;
    int runCoins;
    int walletCoins;
    bool ended;

    public int Coins => walletCoins;
    public int RunCoins => runCoins;
    public bool HasEnded => ended;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        walletCoins = Mathf.Max(0, PlayerPrefs.GetInt(WalletKey, 0));
        if (resultPanel) resultPanel.SetActive(false);
    }

    void Start()
    {
        RefreshCoins();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void SetCrowdCount(int count)
    {
        if (crowdText) crowdText.text = "SOLDIERS  " + count;
    }

    public void AddCoins(int amount)
    {
        if (ended || amount <= 0) return;
        runCoins += amount;
        walletCoins = (int)Mathf.Min(int.MaxValue, (long)walletCoins + amount);
        PlayerPrefs.SetInt(WalletKey, walletCoins);
        PlayerPrefs.Save();
        RefreshCoins();
    }

    public void SetBossHealth(int current, int max)
    {
        if (!bossHealthSlider) return;
        bossHealthSlider.maxValue = Mathf.Max(1, max);
        bossHealthSlider.value = current;
    }

    public void TargetDefeated(Damageable target, int reward, bool isBoss)
    {
        if (ended) return;
        AddCoins(Mathf.Max(0, reward));
        if (isBoss) EndRun(true);
    }

    public void EndRun(bool won)
    {
        if (ended) return;
        ended = true;
        Time.timeScale = 0f;
        if (resultPanel) resultPanel.SetActive(true);
        if (resultText) resultText.text = won
            ? "VICTORY!  +" + runCoins + " COINS"
            : "RUN OVER  +" + runCoins + " COINS";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void RefreshCoins()
    {
        if (coinsText) coinsText.text = "WALLET  " + walletCoins;
    }
}
