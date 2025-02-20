using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    public enum GameScene
    {
        MainMenu = 0,
        Opening = 1,
        BusToChengdu = 2,
        Kuanzhai = 3,
        CookingSchool = 4,
        CookingGame = 5,
        MasterShop,
        Suburbs
    }

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

    public void LoadScene(GameScene scene, bool useLoadingScreen = false, System.Action onSceneLoaded = null)
    {
        Debug.Log($"Loading scene: {scene}, with callback: {onSceneLoaded != null}");
        StartCoroutine(LoadSceneAsync(scene, useLoadingScreen, onSceneLoaded));
    }

    private IEnumerator LoadSceneAsync(GameScene scene, bool useLoadingScreen, System.Action onSceneLoaded)
    {
        // 获取场景名称和车费
        string sceneName = GetSceneName(scene);
        int travelCost = GetTravelCost(scene);

        Debug.Log($"Starting to load scene: {scene}, scene name: {sceneName}");

        // 显示过渡动画
        if (useLoadingScreen && SceneTransitionUI.Instance != null)
        {
            Debug.Log("Showing transition UI");
            yield return StartCoroutine(SceneTransitionUI.Instance.ShowTransition(sceneName, travelCost));
        }

        // 开始加载场景
        Debug.Log($"Loading scene async: {scene}");
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.ToString());
        
        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to start loading scene: {scene}. Make sure the scene is added to Build Settings!");
            yield break;
        }

        // 先禁止自动激活场景
        asyncLoad.allowSceneActivation = false;

        // 更新加载进度
        float progress = 0f;
        while (!asyncLoad.isDone)
        {
            // 将 0-0.9 的进度映射到 0-1
            progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            if (SceneTransitionUI.Instance != null)
            {
                Debug.Log($"Loading progress: {progress}");
                SceneTransitionUI.Instance.UpdateProgress(progress);
            }

            // 当进度达到 100% 时激活场景
            if (Mathf.Approximately(asyncLoad.progress, 0.9f))
            {
                // 等待一小段时间让用户看到 100%
                yield return new WaitForSeconds(0.2f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log($"Scene loaded: {scene}");

        // 等待一帧确保场景完全初始化
        yield return new WaitForEndOfFrame();

        // 隐藏过渡动画
        if (useLoadingScreen && SceneTransitionUI.Instance != null)
        {
            Debug.Log("Hiding transition UI");
            yield return StartCoroutine(SceneTransitionUI.Instance.HideTransition());
        }

        Debug.Log($"Scene loading complete: {scene}");
        // 调用回调
        onSceneLoaded?.Invoke();
    }

    private string GetSceneName(GameScene scene)
    {
        switch (scene)
        {
            case GameScene.BusToChengdu: return "成都";
            case GameScene.Kuanzhai: return "宽窄巷子";
            case GameScene.CookingSchool: return "烹饪学校";
            case GameScene.MasterShop: return "陈麻婆店";
            default: return scene.ToString();
        }
    }

    private int GetTravelCost(GameScene scene)
    {
        switch (scene)
        {
            case GameScene.BusToChengdu: return 200;
            case GameScene.Kuanzhai: return 20;
            case GameScene.CookingSchool: return 10;
            case GameScene.MasterShop: return 15;
            default: return 0;
        }
    }
} 