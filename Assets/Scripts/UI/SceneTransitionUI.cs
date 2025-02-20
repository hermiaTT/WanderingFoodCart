using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneTransitionUI : MonoBehaviour
{
    public static SceneTransitionUI Instance { get; private set; }

    [Header("UI References")]
    public CanvasGroup fadePanel;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI tipText;
    public Slider progressBar;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public string[] loadingTips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 确保初始状态是隐藏的
            if (fadePanel != null)
            {
                fadePanel.alpha = 0;
                fadePanel.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 添加一个Start方法来确保初始状态
    private void Start()
    {
        // 重置所有UI元素
        if (fadePanel != null)
        {
            fadePanel.alpha = 0;
            fadePanel.gameObject.SetActive(false);
        }
        
        if (loadingText != null)
        {
            loadingText.text = "";
        }
        
        if (tipText != null)
        {
            tipText.text = "";
        }
        
        if (progressBar != null)
        {
            progressBar.value = 0;
        }
    }

    public IEnumerator ShowTransition(string sceneName, int cost = 0)
    {
        // 显示过渡面板
        fadePanel.gameObject.SetActive(true);
        fadePanel.alpha = 0;

        // 淡入
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1;

        // 显示加载提示
        if (cost > 0)
        {
            loadingText.text = $"正在前往{sceneName}...（车费 -{cost}元）";
            // 更新玩家金钱
            GameManager.Instance.playerMoney -= cost;
        }
        else
        {
            loadingText.text = $"正在前往{sceneName}...";
        }

        // 显示随机提示
        if (loadingTips != null && loadingTips.Length > 0)
        {
            tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }
    }

    public IEnumerator HideTransition()
    {
        // 淡出
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0;
        fadePanel.gameObject.SetActive(false);
    }

    public void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            // 将进度值转换为百分比
            progressBar.value = progress;
            
            // 如果有进度文本，也可以显示百分比
            if (loadingText != null)
            {
                int percentage = Mathf.RoundToInt(progress * 100);
                loadingText.text = $"正在加载... {percentage}%";
            }
        }
    }
} 