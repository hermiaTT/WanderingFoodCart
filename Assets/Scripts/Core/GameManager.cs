using UnityEngine;
using UnityEngine.SceneManagement;  // Unity的场景管理
using System.Collections;
using UnityEngine.UI;  // 添加这行来引用UI组件
using UnityEngine.EventSystems;  // 添加这行来引用EventSystem相关组件
using TMPro;

public class GameManager : MonoBehaviour
{
    // 单例模式，确保游戏中只有一个GameManager
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanelPrefab;  // 添加这行，用于引用对话面板预制体
    public TMP_FontAsset chineseFont; // 添加中文字体资源引用

    [Header("Game State")]
    public float playerMoney = 1000f;  // 初始资金
    public bool isGamePaused = false;  // 游戏是否暂停
    private bool hasStartedDialogue = false;  // 添加此变量来追踪对话是否已开始

    [Header("Dialogue Data")]
    public DialogueData openingDialogue;  // 在Inspector中设置开场对话
    public DialogueData wideAlleyDialogue; // 宽窄巷子对话
    public DialogueData cookingDialogue;   // 烹饪课程对话

    private DialogueDataCollection dialogueCollection;

    [Header("UI Prefabs")]
    public GameObject sceneTransitionPrefab; // 添加场景过渡预制体引用

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 创建场景过渡UI
            if (sceneTransitionPrefab != null && SceneTransitionUI.Instance == null)
            {
                Instantiate(sceneTransitionPrefab);
            }

            // 确保所有必要的管理器都存在
            InitializeManagers();
            
            // 等待一帧后再加载主菜单场景
            StartCoroutine(InitializeGame());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        // 创建 SceneManager
        if (FindObjectOfType<SceneManager>() == null)
        {
            GameObject sceneManagerObj = new GameObject("SceneManager");
            sceneManagerObj.AddComponent<SceneManager>();
            DontDestroyOnLoad(sceneManagerObj);
        }

        // 创建 DialogueManager
        if (FindObjectOfType<DialogueManager>() == null)
        {
            GameObject dialogueManagerObj = new GameObject("DialogueManager");
            dialogueManagerObj.AddComponent<DialogueManager>();
            DontDestroyOnLoad(dialogueManagerObj);
        }

        // 创建 GameplayManager
        if (FindObjectOfType<GameplayManager>() == null)
        {
            GameObject gameplayManagerObj = new GameObject("GameplayManager");
            gameplayManagerObj.AddComponent<GameplayManager>();
            DontDestroyOnLoad(gameplayManagerObj);
        }

        // 创建 GameFlowManager
        if (FindObjectOfType<GameFlowManager>() == null)
        {
            GameObject gameFlowManagerObj = new GameObject("GameFlowManager");
            gameFlowManagerObj.AddComponent<GameFlowManager>();
            DontDestroyOnLoad(gameFlowManagerObj);
        }
    }

    private IEnumerator InitializeGame()
    {
        // 等待一帧，确保所有管理器都已初始化
        yield return null;

        // 确保从主菜单开始
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadScene(SceneManager.GameScene.MainMenu);
        }
        else
        {
            Debug.LogError("SceneManager instance not found after initialization!");
        }
    }

    private void Start()
    {
        // 确保所有必要的管理器都存在
        if (FindObjectOfType<DialogueManager>() == null)
        {
            GameObject dialogueManagerObj = new GameObject("DialogueManager");
            dialogueManagerObj.AddComponent<DialogueManager>();
            DontDestroyOnLoad(dialogueManagerObj);
        }

        if (FindObjectOfType<SceneManager>() == null)
        {
            GameObject sceneManagerObj = new GameObject("SceneManager");
            sceneManagerObj.AddComponent<SceneManager>();
            DontDestroyOnLoad(sceneManagerObj);
        }

        if (FindObjectOfType<GameplayManager>() == null)
        {
            GameObject gameplayManagerObj = new GameObject("GameplayManager");
            gameplayManagerObj.AddComponent<GameplayManager>();
            DontDestroyOnLoad(gameplayManagerObj);
        }

        // 创建对话数据
        if (dialogueCollection == null)
        {
            dialogueCollection = ScriptableObject.CreateInstance<DialogueDataCollection>();
            DialogueDataCreator creator = ScriptableObject.CreateInstance<DialogueDataCreator>();
            
            dialogueCollection.openingDialogue = creator.CreateOpeningDialogue();
            dialogueCollection.busToChengduDialogue = creator.CreateBusToChengduDialogue();
            dialogueCollection.kuanzhaiIntroDialogue = creator.CreateKuanzhaiDialogue();
            dialogueCollection.cookingSchoolDialogue = creator.CreateCookingSchoolDialogue();
            dialogueCollection.firstSuccessDialogue = creator.CreateFirstSuccessDialogue();  // 添加这行
            
            Debug.Log("Created all dialogue data");
        }
    }

    private void OnEnable()
    {
        // 使用Unity的SceneManager来监听场景加载事件
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 取消注册场景加载事件
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 场景加载完成时的回调
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 如果是主菜单场景，确保对话框不显示
        if (scene.buildIndex == 0)  // MainMenu
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.HideDialogue();
            }
            hasStartedDialogue = false;
        }
    }

    // 修改 StartGameDialogue 方法
    private void StartGameDialogue()
    {
        // 如果不是由 GameFlowManager 调用，则直接返回
        if (GameFlowManager.Instance != null)
        {
            return;
        }

        if (hasStartedDialogue) return;
        StartCoroutine(StartDialogueAfterDelay());
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        yield return null;
        yield return null;

        if (dialoguePanelPrefab != null)
        {
            // 先检查并销毁可能存在的旧对话面板
            GameObject oldPanel = GameObject.Find("DialoguePanel(Clone)");
            if (oldPanel != null)
            {
                Destroy(oldPanel);
            }

            // 确保有Canvas和EventSystem
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, false);  // 使用 false 参数

            // 开始开场对话
            if (openingDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(openingDialogue);
                hasStartedDialogue = true;
            }
        }
    }

    // 暂停游戏
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    // 继续游戏
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
} 