using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public enum GameplayType
    {
        None,
        CookingTutorial,    // 基础烹饪教程
        MaPoTofu,           // 麻婆豆腐小游戏
        DoubanjiangMaking,  // 制作豆瓣酱
        FinalCooking,       // 最终烹饪挑战
        BusinessGame,         // 商业游戏
        CookingChallenge,    // 烹饪挑战
        FindRecipe          // 寻找配方任务
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

    public void StartGameplay(string gameplayType)
    {
        Debug.Log($"StartGameplay called with type: {gameplayType}");
        if (System.Enum.TryParse(gameplayType, out GameplayType type))
        {
            Debug.Log($"Parsed gameplay type: {type}");
            switch (type)
            {
                case GameplayType.CookingTutorial:
                    StartCookingTutorial();
                    break;
                case GameplayType.MaPoTofu:
                    StartMaPoTofuGame();
                    break;
                case GameplayType.DoubanjiangMaking:
                    StartDoubanjiangMaking();
                    break;
                case GameplayType.FinalCooking:
                    StartFinalCooking();
                    break;
                case GameplayType.BusinessGame:
                    StartBusinessGame();
                    break;
                case GameplayType.CookingChallenge:
                    StartCookingChallenge();
                    break;
                case GameplayType.FindRecipe:
                    StartFindRecipeGame();
                    break;
            }
        }
        else
        {
            Debug.LogError($"Failed to parse gameplay type: {gameplayType}");
        }
    }

    private void StartCookingTutorial()
    {
        Debug.Log("Starting cooking tutorial");
        // 加载烹饪教程场景
        SceneManager.Instance.LoadScene(SceneManager.GameScene.CookingGame, true, () => {
            Debug.Log("Cooking game scene loaded, setting up tutorial");
            SetupCookingTutorial();
        });
    }

    private void SetupCookingTutorial()
    {
        Debug.Log("Setting up cooking tutorial");
        
        // 创建或获取Canvas
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.Log("Creating new Canvas");
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvasComponent = canvas.GetComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // 确保有 EventSystem
            if (FindObjectOfType<EventSystem>() == null)
            {
                Debug.Log("Creating EventSystem");
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }

        // 创建完成按钮
        GameObject finishButton = new GameObject("FinishButton", typeof(RectTransform), typeof(Button), typeof(Image));
        finishButton.transform.SetParent(canvas.transform, false);
        Debug.Log("Created finish button");

        // 设置按钮位置和大小
        RectTransform rectTransform = finishButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // 改为屏幕中心
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(200, 60);
        rectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image image = finishButton.GetComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加按钮文本
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(finishButton.transform, false);
        
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
        textRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = "完成烹饪";
        text.fontSize = 32;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        if (GameManager.Instance.chineseFont != null)
        {
            text.font = GameManager.Instance.chineseFont;
        }

        // 设置按钮点击事件
        Button button = finishButton.GetComponent<Button>();
        button.onClick.AddListener(() => {
            Debug.Log("Finish button clicked!");
            OnCookingTutorialComplete();
        });

        Debug.Log("Cooking tutorial setup complete");
    }

    private void OnCookingTutorialComplete()
    {
        Debug.Log("OnCookingTutorialComplete called");
        if (SceneManager.Instance == null)
        {
            Debug.LogError("SceneManager.Instance is null!");
            return;
        }
        
        // 返回烹饪学校场景并继续对话
        SceneManager.Instance.LoadScene(SceneManager.GameScene.CookingSchool, true, () => {
            Debug.Log("Back to cooking school scene");
            if (GameFlowManager.Instance == null)
            {
                Debug.LogError("GameFlowManager.Instance is null!");
                return;
            }
            
            Debug.Log("Starting first success dialogue");
            GameFlowManager.Instance.StartFirstSuccess();
        });
    }

    private void StartMaPoTofuGame()
    {
        Debug.Log("开始麻婆豆腐小游戏");
        // TODO: 实现麻婆豆腐小游戏
    }

    private void StartDoubanjiangMaking()
    {
        Debug.Log("开始制作豆瓣酱");
        // TODO: 实现豆瓣酱制作
    }

    private void StartFinalCooking()
    {
        Debug.Log("开始最终烹饪挑战");
        // TODO: 实现最终烹饪挑战
    }

    private void StartBusinessGame()
    {
        Debug.Log("Starting business game");
        
        // 创建或获取Canvas
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.Log("Creating new Canvas");
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvasComponent = canvas.GetComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            
            if (FindObjectOfType<EventSystem>() == null)
            {
                Debug.Log("Creating EventSystem");
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }

        // 创建开始按钮
        GameObject startButton = new GameObject("StartButton", typeof(RectTransform), typeof(Button), typeof(Image));
        startButton.transform.SetParent(canvas.transform, false);
        Debug.Log("Created start button");

        // 设置按钮位置和大小
        RectTransform startRectTransform = startButton.GetComponent<RectTransform>();
        startRectTransform.anchorMin = new Vector2(0.5f, 0.6f);
        startRectTransform.anchorMax = new Vector2(0.5f, 0.6f);
        startRectTransform.pivot = new Vector2(0.5f, 0.5f);
        startRectTransform.sizeDelta = new Vector2(200, 60);
        startRectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image startImage = startButton.GetComponent<Image>();
        startImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加开始按钮文本
        GameObject startTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        startTextObj.transform.SetParent(startButton.transform, false);
        
        RectTransform startTextRectTransform = startTextObj.GetComponent<RectTransform>();
        startTextRectTransform.anchorMin = Vector2.zero;
        startTextRectTransform.anchorMax = Vector2.one;
        startTextRectTransform.sizeDelta = Vector2.zero;
        startTextRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI startText = startTextObj.GetComponent<TextMeshProUGUI>();
        startText.text = "开始经营";
        startText.fontSize = 32;
        startText.alignment = TextAlignmentOptions.Center;
        startText.color = Color.white;

        // 创建完成按钮
        GameObject finishButton = new GameObject("FinishButton", typeof(RectTransform), typeof(Button), typeof(Image));
        finishButton.transform.SetParent(canvas.transform, false);
        Debug.Log("Created finish button");

        // 设置按钮位置和大小
        RectTransform finishRectTransform = finishButton.GetComponent<RectTransform>();
        finishRectTransform.anchorMin = new Vector2(0.5f, 0.4f);
        finishRectTransform.anchorMax = new Vector2(0.5f, 0.4f);
        finishRectTransform.pivot = new Vector2(0.5f, 0.5f);
        finishRectTransform.sizeDelta = new Vector2(200, 60);
        finishRectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image finishImage = finishButton.GetComponent<Image>();
        finishImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加完成按钮文本
        GameObject finishTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        finishTextObj.transform.SetParent(finishButton.transform, false);
        
        RectTransform finishTextRectTransform = finishTextObj.GetComponent<RectTransform>();
        finishTextRectTransform.anchorMin = Vector2.zero;
        finishTextRectTransform.anchorMax = Vector2.one;
        finishTextRectTransform.sizeDelta = Vector2.zero;
        finishTextRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI finishText = finishTextObj.GetComponent<TextMeshProUGUI>();
        finishText.text = "完成经营";
        finishText.fontSize = 32;
        finishText.alignment = TextAlignmentOptions.Center;
        finishText.color = Color.white;

        if (GameManager.Instance.chineseFont != null)
        {
            startText.font = GameManager.Instance.chineseFont;
            finishText.font = GameManager.Instance.chineseFont;
        }

        // 设置按钮点击事件
        Button startBtn = startButton.GetComponent<Button>();
        startBtn.onClick.AddListener(() => {
            Debug.Log("Start business button clicked!");
            startButton.SetActive(false);  // 隐藏开始按钮
            finishButton.SetActive(true);  // 显示完成按钮
            OnBusinessStart();
        });

        Button finishBtn = finishButton.GetComponent<Button>();
        finishBtn.onClick.AddListener(() => {
            Debug.Log("Finish business button clicked!");
            OnBusinessComplete();
        });

        // 初始状态只显示开始按钮
        finishButton.SetActive(false);
    }

    private void OnBusinessStart()
    {
        Debug.Log("Business started");
        // TODO: 开始经营游戏的具体逻辑
    }

    private void OnBusinessComplete()
    {
        Debug.Log("Business completed");
        
        // 找到并隐藏完成按钮
        GameObject finishButton = GameObject.Find("FinishButton");
        if (finishButton != null)
        {
            finishButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Finish button not found!");
        }

        // 找到并隐藏开始按钮
        GameObject startButton = GameObject.Find("StartButton");
        if (startButton != null)
        {
            startButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Start button not found!");
        }

        // 结束经营，进入下一段剧情
        GameFlowManager.Instance.EndBusiness();
    }

    private void StartCookingChallenge()
    {
        Debug.Log("开始烹饪挑战");
        // TODO: 实现烹饪挑战
    }

    private void StartFindRecipeGame()
    {
        Debug.Log("Starting find recipe game");
        
        // 创建或获取Canvas
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.Log("Creating new Canvas");
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvasComponent = canvas.GetComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            
            if (FindObjectOfType<EventSystem>() == null)
            {
                Debug.Log("Creating EventSystem");
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }

        // 创建接受任务按钮
        GameObject acceptButton = new GameObject("AcceptButton", typeof(RectTransform), typeof(Button), typeof(Image));
        acceptButton.transform.SetParent(canvas.transform, false);
        Debug.Log("Created accept button");

        // 设置按钮位置和大小
        RectTransform acceptRectTransform = acceptButton.GetComponent<RectTransform>();
        acceptRectTransform.anchorMin = new Vector2(0.5f, 0.6f);
        acceptRectTransform.anchorMax = new Vector2(0.5f, 0.6f);
        acceptRectTransform.pivot = new Vector2(0.5f, 0.5f);
        acceptRectTransform.sizeDelta = new Vector2(200, 60);
        acceptRectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image acceptImage = acceptButton.GetComponent<Image>();
        acceptImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加接受任务按钮文本
        GameObject acceptTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        acceptTextObj.transform.SetParent(acceptButton.transform, false);
        
        RectTransform acceptTextRectTransform = acceptTextObj.GetComponent<RectTransform>();
        acceptTextRectTransform.anchorMin = Vector2.zero;
        acceptTextRectTransform.anchorMax = Vector2.one;
        acceptTextRectTransform.sizeDelta = Vector2.zero;
        acceptTextRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI acceptText = acceptTextObj.GetComponent<TextMeshProUGUI>();
        acceptText.text = "接受任务";
        acceptText.fontSize = 32;
        acceptText.alignment = TextAlignmentOptions.Center;
        acceptText.color = Color.white;

        // 创建完成任务按钮
        GameObject completeButton = new GameObject("CompleteButton", typeof(RectTransform), typeof(Button), typeof(Image));
        completeButton.transform.SetParent(canvas.transform, false);
        Debug.Log("Created complete button");

        // 设置按钮位置和大小
        RectTransform completeRectTransform = completeButton.GetComponent<RectTransform>();
        completeRectTransform.anchorMin = new Vector2(0.5f, 0.4f);
        completeRectTransform.anchorMax = new Vector2(0.5f, 0.4f);
        completeRectTransform.pivot = new Vector2(0.5f, 0.5f);
        completeRectTransform.sizeDelta = new Vector2(200, 60);
        completeRectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image completeImage = completeButton.GetComponent<Image>();
        completeImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加完成任务按钮文本
        GameObject completeTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        completeTextObj.transform.SetParent(completeButton.transform, false);
        
        RectTransform completeTextRectTransform = completeTextObj.GetComponent<RectTransform>();
        completeTextRectTransform.anchorMin = Vector2.zero;
        completeTextRectTransform.anchorMax = Vector2.one;
        completeTextRectTransform.sizeDelta = Vector2.zero;
        completeTextRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI completeText = completeTextObj.GetComponent<TextMeshProUGUI>();
        completeText.text = "完成任务";
        completeText.fontSize = 32;
        completeText.alignment = TextAlignmentOptions.Center;
        completeText.color = Color.white;

        if (GameManager.Instance.chineseFont != null)
        {
            acceptText.font = GameManager.Instance.chineseFont;
            completeText.font = GameManager.Instance.chineseFont;
        }

        // 设置按钮点击事件
        Button acceptBtn = acceptButton.GetComponent<Button>();
        acceptBtn.onClick.AddListener(() => {
            Debug.Log("Accept quest button clicked!");
            acceptButton.SetActive(false);  // 隐藏接受任务按钮
            completeButton.SetActive(true);  // 显示完成任务按钮
            OnQuestAccepted();
        });

        Button completeBtn = completeButton.GetComponent<Button>();
        completeBtn.onClick.AddListener(() => {
            Debug.Log("Complete quest button clicked!");
            OnQuestComplete();
        });

        // 初始状态只显示接受任务按钮
        completeButton.SetActive(false);
    }

    private void OnQuestAccepted()
    {
        Debug.Log("Quest accepted");
        // TODO: 开始寻找配方的具体逻辑
    }

    private void OnQuestComplete()
    {
        Debug.Log("Quest completed");
        
        // 找到并隐藏完成按钮
        GameObject completeButton = GameObject.Find("CompleteButton");
        if (completeButton != null)
        {
            completeButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Complete button not found!");
        }

        // 找到并隐藏接受按钮
        GameObject acceptButton = GameObject.Find("AcceptButton");
        if (acceptButton != null)
        {
            acceptButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Accept button not found!");
        }

        // 开始结局对话
        GameFlowManager.Instance.StartReunion();
    }
} 