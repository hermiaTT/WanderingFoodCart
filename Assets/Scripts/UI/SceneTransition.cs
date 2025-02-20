using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    public Image fadeImage;  // 场景切换时的淡入淡出效果图片
    
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

    public void FadeIn()
    {
        // 淡入效果
        fadeImage.CrossFadeAlpha(1, 1f, false);
    }

    public void FadeOut()
    {
        // 淡出效果
        fadeImage.CrossFadeAlpha(0, 1f, false);
    }
} 