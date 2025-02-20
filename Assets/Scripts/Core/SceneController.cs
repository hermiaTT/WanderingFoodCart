using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        // 从开场场景开始
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Opening, true, () => {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.StartChapter1();
            }
        });
    }

    public void ReturnToMainMenu()
    {
        SceneManager.Instance.LoadScene(SceneManager.GameScene.MainMenu);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 