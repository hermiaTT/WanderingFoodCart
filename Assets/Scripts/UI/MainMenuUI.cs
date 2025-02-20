using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button startButton;    // 开始游戏按钮
    public Button settingsButton; // 设置按钮
    public Button quitButton;     // 退出按钮

    private void Start()
    {
        // 添加按钮点击事件
        startButton.onClick.AddListener(OnStartButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnStartButtonClick()
    {
        // 不直接加载 Countryside，而是通过 GameFlowManager 开始第一章
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.StartChapter1();
        }
        else
        {
            Debug.LogError("GameFlowManager instance not found!");
        }
    }

    private void OnSettingsButtonClick()
    {
        Debug.Log("Settings button clicked");
        // 后续添加设置面板逻辑
    }

    private void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 