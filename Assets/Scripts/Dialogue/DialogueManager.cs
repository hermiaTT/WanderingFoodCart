using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject dialoguePanel;           // 对话面板
    public TextMeshProUGUI dialogueText;      // 对话文本
    public TextMeshProUGUI speakerNameText;   // 说话者名字
    public Button continueButton;             // 继续按钮
    public GameObject transitionButton;        // 过渡按钮引用
    private TextMeshProUGUI transitionText;    // 改为私有变量，我们会在代码中获取它
    
    private Queue<string> sentences;           // 存储对话句子的队列
    public bool isDialogueActive = false;

    [Header("Input Settings")]
    private bool canContinue = true;    // 控制点击间隔
    private float clickCooldown = 0.1f;  // 点击冷却时间
    private float lastClickTime;         // 上次点击时间
    private bool isProcessingClick = false; // 添加此变量来追踪点击处理状态

    private DialogueData currentDialogue;
    private int currentSectionIndex = 0;
    private DialogueData.DialogueSection currentSection;
    private bool isWaitingForTransition = false;

    // 添加对话完成事件
    public static event System.Action<DialogueData> OnDialogueComplete;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sentences = new Queue<string>();
            CreateTransitionButton();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        // 如果正在等待过渡，不处理对话输入
        if (isWaitingForTransition) return;

        // 添加空引用检查
        if (dialoguePanel == null) return;
        
        // 如果对话不活跃，直接返回
        if (!isDialogueActive) return;

        // 检查空格键或鼠标点击
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // 如果是鼠标点击，需要检查是否点击在对话面板上
            if (Input.GetMouseButtonDown(0) && !IsClickOnDialoguePanel())
            {
                return;
            }

            // 处理点击事件
            ProcessClick();
        }
    }

    private bool IsClickOnDialoguePanel()
    {
        // 添加空引用检查
        if (dialoguePanel == null || EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == dialoguePanel || 
                (result.gameObject.transform.parent != null && 
                 result.gameObject.transform.IsChildOf(dialoguePanel.transform)))
            {
                return true;
            }
        }
        return false;
    }

    // 新增方法：处理点击事件
    private void ProcessClick()
    {
        // 如果正在处理点击或冷却中，直接返回
        if (isProcessingClick || Time.time - lastClickTime < clickCooldown)
        {
            return;
        }

        // 更新点击时间和状态
        lastClickTime = Time.time;
        isProcessingClick = true;

        // 显示下一句对话
        DisplayNextSentence();

        // 立即重置处理状态，改用更短的冷却时间
        isProcessingClick = false;
    }

    // 开始新的对话
    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentSectionIndex = 0;
        isDialogueActive = true;
        isProcessingClick = false;
        lastClickTime = 0f;
        
        sentences.Clear();
        if (dialogue.sections.Length > 0)
        {
            StartDialogueSection(dialogue.sections[0]);
        }
    }

    private void StartDialogueSection(DialogueData.DialogueSection section)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = section.speakerName;
        
        sentences.Clear();
        foreach (string sentence in section.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
    }

    // 显示下一句对话
    public void DisplayNextSentence()
    {
        Debug.Log($"DisplayNextSentence: Sentences count = {sentences.Count}, Section index = {currentSectionIndex}");

        if (sentences.Count == 0)
        {
            Debug.Log($"No more sentences in current section"); // 添加日志
            
            // 检查当前对话部分是否需要触发场景切换或游戏玩法
            if (currentDialogue != null)
            {
                Debug.Log($"Current dialogue sections count: {currentDialogue.sections.Length}"); // 添加日志
                Debug.Log($"Current section index: {currentSectionIndex}"); // 添加日志
                
                if (currentSectionIndex < currentDialogue.sections.Length)
                {
                    currentSection = currentDialogue.sections[currentSectionIndex];
                    Debug.Log($"Current section: Speaker = {currentSection.speakerName}, triggerSceneChange = {currentSection.triggerSceneChange}");
                    
                    if (currentSection.triggerSceneChange)
                    {
                        Debug.Log($"Triggering scene change to: {currentSection.nextScene}");
                        ShowTransitionButton();
                        return;
                    }
                }
            }

            // 检查是否需要触发游戏玩法
            if (currentSection.triggerGameplay)
            {
                Debug.Log($"Triggering gameplay: {currentSection.gameplayType}");
                GameplayManager.Instance.StartGameplay(currentSection.gameplayType);
                return;  // 不要继续处理对话
            }

            // 检查是否还有下一段对话
            currentSectionIndex++;
            if (currentSectionIndex < currentDialogue.sections.Length)
            {
                Debug.Log($"Moving to next section: {currentSectionIndex}");
                StartDialogueSection(currentDialogue.sections[currentSectionIndex]);
                return;
            }

            Debug.Log("No more sections, ending dialogue");
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    private void ShowTransitionButton()
    {
        Debug.Log("Showing transition button");

        if (transitionButton == null)
        {
            Debug.Log("Creating new transition button because it was null");
            CreateTransitionButton();
        }

        if (transitionText == null && transitionButton != null)
        {
            transitionText = transitionButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (transitionButton != null && transitionText != null)
        {
            isWaitingForTransition = true;
            dialoguePanel.SetActive(false);
            
            transitionText.text = "继续故事";
            
            Button button = transitionButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    Debug.Log("Transition button clicked");
                    transitionButton.SetActive(false);
                    isWaitingForTransition = false;
                    if (SceneManager.Instance != null)
                    {
                        Debug.Log($"Loading scene: {currentSection.nextScene}");
                        // 先触发对话完成事件
                        EndDialogue();
                        // 等待一帧后再加载新场景
                        StartCoroutine(LoadNextSceneAfterDelay(currentSection.nextScene));
                    }
                    else
                    {
                        Debug.LogError("SceneManager instance not found!");
                    }
                });
            }
            else
            {
                Debug.LogError("Button component not found on transition button!");
            }

            transitionButton.SetActive(true);
            Debug.Log("Transition button should now be visible");
        }
        else
        {
            Debug.LogError($"Failed to show transition button. Button: {transitionButton}, Text: {transitionText}");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(SceneManager.GameScene nextScene)
    {
        // 等待更长时间确保事件处理完成
        yield return new WaitForSeconds(0.5f);
        SceneManager.Instance.LoadScene(nextScene);
    }

    private void ShowGameplayTransitionButton()
    {
        isWaitingForTransition = true;
        dialoguePanel.SetActive(false);
        transitionButton.SetActive(true);

        // 确保我们有 transitionText 引用
        if (transitionText == null)
        {
            transitionText = transitionButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        transitionText.text = "开始挑战";
        
        Button button = transitionButton.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            transitionButton.SetActive(false);
            isWaitingForTransition = false;
            GameplayManager.Instance.StartGameplay(currentSection.gameplayType);
        });
    }

    // 结束对话
    void EndDialogue()
    {
        Debug.Log($"EndDialogue called for dialogue type: {currentDialogue?.type}");
        if (sentences.Count == 0)
        {
            isDialogueActive = false;
            dialoguePanel.SetActive(false);
            
            var dialogueToComplete = currentDialogue;
            // 清除当前对话引用，防止重复触发
            currentDialogue = null;
            
            // 触发对话完成事件
            if (dialogueToComplete != null)
            {
                Debug.Log($"Invoking OnDialogueComplete for {dialogueToComplete.type}");
                OnDialogueComplete?.Invoke(dialogueToComplete);
            }
        }
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        
        if (transitionButton != null)
        {
            transitionButton.SetActive(false);
        }
        
        isDialogueActive = false;
        isProcessingClick = false;
        isWaitingForTransition = false;
        sentences.Clear();
    }

    public void SetDialoguePanel(GameObject panel, bool destroyOld = true)
    {
        if (destroyOld && dialoguePanel != null)
        {
            try
            {
                // 如果是预制体实例，使用 Destroy
                if (dialoguePanel.scene.IsValid())
                {
                    if (Application.isPlaying)
                    {
                        Destroy(dialoguePanel);
                    }
                    else
                    {
                        DestroyImmediate(dialoguePanel);
                    }
                }
                // 否则不进行销毁
                else
                {
                    Debug.LogWarning("Skipping destroy for prefab asset");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error while destroying dialogue panel: {e.Message}");
            }
        }

        SetupDialoguePanel(panel);
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void SetupDialoguePanel(GameObject panel)
    {
        if (panel == null) return;

        dialoguePanel = panel;

        // 通过名字查找特定的文本组件
        TextMeshProUGUI[] texts = panel.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var text in texts)
        {
            if (text.gameObject.name == "DialogueText")
            {
                dialogueText = text;
                if (GameManager.Instance != null && GameManager.Instance.chineseFont != null)
                {
                    dialogueText.font = GameManager.Instance.chineseFont;
                    dialogueText.fontSize = 32;
                }
            }
            else if (text.gameObject.name == "SpeakerNameText")
            {
                speakerNameText = text;
                if (GameManager.Instance != null && GameManager.Instance.chineseFont != null)
                {
                    speakerNameText.font = GameManager.Instance.chineseFont;
                    speakerNameText.fontSize = 36;
                }
            }
        }

        // 获取继续按钮
        continueButton = panel.GetComponentInChildren<Button>(true);
        if (continueButton != null)
        {
            TextMeshProUGUI buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>(true);
            if (buttonText != null && GameManager.Instance != null && GameManager.Instance.chineseFont != null)
            {
                buttonText.font = GameManager.Instance.chineseFont;
                buttonText.fontSize = 28;
            }
        }

        Debug.Log($"Setup complete - DialogueText: {dialogueText != null}, SpeakerNameText: {speakerNameText != null}, ContinueButton: {continueButton != null}");
    }

    private void CreateTransitionButton()
    {
        Debug.Log("Creating transition button"); // 添加日志

        // 找到或创建Canvas
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.Log("Creating new Canvas"); // 添加日志
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvasComponent = canvas.GetComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasComponent.sortingOrder = 999; // 确保显示在最上层
        }

        // 创建过渡按钮
        transitionButton = new GameObject("TransitionButton", typeof(RectTransform), typeof(Button), typeof(Image));
        transitionButton.transform.SetParent(canvas.transform, false);

        // 设置按钮的RectTransform
        RectTransform rectTransform = transitionButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(300, 100); // 设置按钮大小
        rectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮的Image组件
        Image image = transitionButton.GetComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);  // 深灰色背景

        // 创建文本对象
        GameObject textObj = new GameObject("TransitionText", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(transitionButton.transform, false);

        // 设置文本的RectTransform
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
        textRectTransform.anchoredPosition = Vector2.zero;

        // 设置文本组件
        transitionText = textObj.GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance.chineseFont != null)
        {
            // 设置字体为动态模式
            GameManager.Instance.chineseFont.atlasPopulationMode = AtlasPopulationMode.Dynamic;
            transitionText.font = GameManager.Instance.chineseFont;
        }
        transitionText.fontSize = 36;
        transitionText.alignment = TextAlignmentOptions.Center;
        transitionText.color = Color.white;

        // 添加按钮点击音效组件（可选）
        transitionButton.AddComponent<AudioSource>();

        // 初始状态为隐藏
        transitionButton.SetActive(false);

        Debug.Log("Transition button created"); // 添加日志
    }

    // 添加一个新方法来处理场景切换后的对话完成
    public void CompleteDialogueAfterTransition()
    {
        Debug.Log("Completing dialogue after transition");
        if (currentDialogue != null)
        {
            OnDialogueComplete?.Invoke(currentDialogue);
        }
    }

    public void ResetState()
    {
        isDialogueActive = false;
        isProcessingClick = false;
        isWaitingForTransition = false;
        currentSectionIndex = 0;
        sentences.Clear();
    }
} 