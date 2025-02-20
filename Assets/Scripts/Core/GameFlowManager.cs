using UnityEngine;
using System.Collections;
using UnityEngine.UI;  // 添加 UI 命名空间
using UnityEngine.EventSystems;  // 添加 EventSystem 命名空间
using TMPro;  // 如果需要使用 TextMeshPro

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }
    public DialogueDataCollection dialogueCollection;

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

    private void Start()
    {
        // 创建对话数据
        if (dialogueCollection == null)
        {
            dialogueCollection = ScriptableObject.CreateInstance<DialogueDataCollection>();
            DialogueDataCreator creator = ScriptableObject.CreateInstance<DialogueDataCreator>();
            
            // 创建所有对话数据
            dialogueCollection.openingDialogue = creator.CreateOpeningDialogue();
            dialogueCollection.busToChengduDialogue = creator.CreateBusToChengduDialogue();
            dialogueCollection.kuanzhaiIntroDialogue = creator.CreateKuanzhaiDialogue();
            dialogueCollection.cookingSchoolDialogue = creator.CreateCookingSchoolDialogue();
            dialogueCollection.firstSuccessDialogue = creator.CreateFirstSuccessDialogue();
            dialogueCollection.businessStartDialogue = creator.CreateBusinessStartDialogue();
            dialogueCollection.businessEndDialogue = creator.CreateBusinessEndDialogue();
            dialogueCollection.chenShopDialogue = creator.CreateChenShopDialogue();
            dialogueCollection.reunionDialogue = creator.CreateReunionDialogue();
            dialogueCollection.newJourneyDialogue = creator.CreateNewJourneyDialogue();
            
            Debug.Log($"Created all dialogue data. First success dialogue: {dialogueCollection.firstSuccessDialogue != null}, Business dialogues: Start={dialogueCollection.businessStartDialogue != null}, End={dialogueCollection.businessEndDialogue != null}, Chen shop dialogue: {dialogueCollection.chenShopDialogue != null}, Reunion dialogue: {dialogueCollection.reunionDialogue != null}, New journey dialogue: {dialogueCollection.newJourneyDialogue != null}");
        }
    }

    // 添加对话完成的事件监听
    private void OnEnable()
    {
        Debug.Log("GameFlowManager OnEnable"); // 添加日志
        DialogueManager.OnDialogueComplete += HandleDialogueComplete;
    }

    private void OnDisable()
    {
        Debug.Log("GameFlowManager OnDisable"); // 添加日志
        DialogueManager.OnDialogueComplete -= HandleDialogueComplete;
    }

    private void HandleDialogueComplete(DialogueData completedDialogue)
    {
        Debug.Log($"HandleDialogueComplete called for dialogue type: {completedDialogue.type}");
        
        if (completedDialogue.type == DialogueData.DialogueType.Opening)
        {
            Debug.Log("Opening dialogue completed, starting bus scene");
            StartBusToChengdu();
        }
        else if (completedDialogue.type == DialogueData.DialogueType.BusToChengdu)
        {
            Debug.Log("Bus dialogue completed, starting kuanzhai scene");
            StartKuanzhaiScene();
        }
        else if (completedDialogue.type == DialogueData.DialogueType.WideAlley)
        {
            Debug.Log("Kuanzhai dialogue completed, starting cooking school scene");
            StartCookingSchoolScene();
        }
        else if (completedDialogue.type == DialogueData.DialogueType.FirstSuccess)
        {
            Debug.Log("First success dialogue completed, starting business scene");
            StartBusiness();  // 进入经营阶段
        }
        else if (completedDialogue.type == DialogueData.DialogueType.Business)
        {
            Debug.Log("Business dialogue completed, starting Chen shop scene");
            StartChenShop();  // 进入陈麻婆店剧情
        }
        else if (completedDialogue.type == DialogueData.DialogueType.Truth)
        {
            Debug.Log("Truth dialogue completed, showing continue button");
            ShowContinueButton();
        }
        else if (completedDialogue.type == DialogueData.DialogueType.NewJourney)
        {
            Debug.Log("New journey dialogue completed, showing return button");
            ShowReturnButton();
        }
        // ... 其他对话完成的处理
    }

    private void ShowContinueButton()
    {
        // 创建或获取Canvas
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

        // 创建继续按钮
        GameObject continueButton = new GameObject("ContinueButton", typeof(RectTransform), typeof(Button), typeof(Image));
        continueButton.transform.SetParent(canvas.transform, false);

        // 设置按钮位置和大小
        RectTransform rectTransform = continueButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(200, 60);
        rectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image image = continueButton.GetComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加按钮文本
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(continueButton.transform, false);
        
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
        textRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = "继续故事";
        text.fontSize = 32;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        if (GameManager.Instance.chineseFont != null)
        {
            text.font = GameManager.Instance.chineseFont;
        }

        // 设置按钮点击事件
        Button button = continueButton.GetComponent<Button>();
        button.onClick.AddListener(() => {
            Debug.Log("Continue button clicked!");
            continueButton.SetActive(false);
            StartNewJourney();
        });
    }

    private void ShowReturnButton()
    {
        // 创建或获取Canvas
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

        // 创建返回按钮
        GameObject returnButton = new GameObject("ReturnButton", typeof(RectTransform), typeof(Button), typeof(Image));
        returnButton.transform.SetParent(canvas.transform, false);

        // 设置按钮位置和大小
        RectTransform rectTransform = returnButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(200, 60);
        rectTransform.anchoredPosition = Vector2.zero;

        // 设置按钮外观
        Image image = returnButton.GetComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        // 添加按钮文本
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(returnButton.transform, false);
        
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
        textRectTransform.anchoredPosition = Vector2.zero;

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = "回到菜单";
        text.fontSize = 32;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        if (GameManager.Instance.chineseFont != null)
        {
            text.font = GameManager.Instance.chineseFont;
        }

        // 设置按钮点击事件
        Button button = returnButton.GetComponent<Button>();
        button.onClick.AddListener(() => {
            Debug.Log("Return button clicked!");
            returnButton.SetActive(false);
            ReturnToMainMenu();
        });
    }

    private void ReturnToMainMenu()
    {
        // 重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载主菜单场景
        SceneManager.Instance.LoadScene(SceneManager.GameScene.MainMenu);
    }

    #region Chapter 1: Opening
    public void StartChapter1()
    {
        Debug.Log("Starting Chapter 1");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载开场场景
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Opening, true, () => {
            Debug.Log("Opening scene loaded, starting dialogue");
            StartCoroutine(StartOpeningDialogue());
        });
    }

    private IEnumerator StartOpeningDialogue()
    {
        Debug.Log("Starting opening dialogue coroutine");
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            Debug.Log("Creating dialogue panel");
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 先检查是否已存在对话面板
            GameObject existingPanel = GameObject.Find("DialoguePanel(Clone)");
            if (existingPanel != null)
            {
                DestroyImmediate(existingPanel);
            }

            // 创建新的对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, false);  // 设置为 false，因为我们已经手动处理了销毁

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.openingDialogue != null)
            {
                Debug.Log("Starting opening dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.openingDialogue);
            }
            else
            {
                Debug.LogError("Opening dialogue data is missing!");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }

    public void StartBusToChengdu()
    {
        Debug.Log("StartBusToChengdu called");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载场景并设置回调
        SceneManager.Instance.LoadScene(SceneManager.GameScene.BusToChengdu, true, () => {
            Debug.Log("Bus scene loaded callback executed");
            StartCoroutine(StartBusDialogue());
        });
    }

    private IEnumerator StartBusDialogue()
    {
        Debug.Log("Starting bus dialogue");
        
        // 等待确保场景完全加载
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            Debug.Log("Creating dialogue panel");
            
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.Log("Creating new Canvas");
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.busToChengduDialogue != null)
            {
                Debug.Log("Starting bus to chengdu dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.busToChengduDialogue);
            }
            else
            {
                Debug.LogError("Bus dialogue data is missing!");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }
    #endregion

    #region Chapter 2: Kuanzhai
    public void StartKuanzhaiScene()
    {
        Debug.Log("Starting Kuanzhai scene");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载场景并设置回调
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Kuanzhai, true, () => {
            Debug.Log("Kuanzhai scene loaded, starting dialogue");
            StartCoroutine(StartKuanzhaiDialogue());
        });
    }

    private IEnumerator StartKuanzhaiDialogue()
    {
        Debug.Log("Starting Kuanzhai dialogue");
        
        // 等待确保场景完全加载
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            Debug.Log("Creating dialogue panel for Kuanzhai");
            
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.Log("Creating new Canvas for Kuanzhai");
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.kuanzhaiIntroDialogue != null)
            {
                Debug.Log("Starting Kuanzhai intro dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.kuanzhaiIntroDialogue);
            }
            else
            {
                Debug.LogError("Kuanzhai dialogue data is missing!");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }

    public void StartMeetOldMan()
    {
        StartDialogue(dialogueCollection.meetOldManDialogue);
    }
    #endregion

    #region Chapter 3: Learning
    public void StartCookingSchoolScene()
    {
        Debug.Log("Starting Cooking School scene");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载场景并设置回调
        SceneManager.Instance.LoadScene(SceneManager.GameScene.CookingSchool, true, () => {
            Debug.Log("Cooking School scene loaded, starting dialogue");
            StartCoroutine(StartCookingSchoolDialogue());
        });
    }

    private IEnumerator StartCookingSchoolDialogue()
    {
        Debug.Log("Starting cooking school dialogue");
        
        // 等待确保场景完全加载
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            Debug.Log("Creating dialogue panel for cooking school");
            
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.Log("Creating new Canvas for cooking school");
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.cookingSchoolDialogue != null)
            {
                Debug.Log("Starting cooking school dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.cookingSchoolDialogue);
            }
            else
            {
                Debug.LogError("Cooking school dialogue data is missing!");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }

    public void StartFirstSuccess()
    {
        Debug.Log("StartFirstSuccess called");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance is null!");
            return;
        }
        
        DialogueManager.Instance.HideDialogue();
        DialogueManager.Instance.ResetState();

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            Debug.Log("Creating dialogue panel for first success");
            
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.Log("Creating new Canvas");
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.firstSuccessDialogue != null)
            {
                Debug.Log("Starting first success dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.firstSuccessDialogue);
            }
            else
            {
                Debug.LogError($"First success dialogue data is missing! Collection: {dialogueCollection != null}, Dialogue: {dialogueCollection?.firstSuccessDialogue != null}");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }
    #endregion

    #region Chapter 4: Business
    public void StartBusiness()
    {
        Debug.Log("Starting business scene");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载场景并设置回调
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Kuanzhai, true, () => {
            Debug.Log("Business scene loaded, starting dialogue");
            StartCoroutine(StartBusinessDialogue());
        });
    }

    private IEnumerator StartBusinessDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.businessStartDialogue != null)
            {
                Debug.Log("Starting business dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.businessStartDialogue);
            }
            else
            {
                Debug.LogError("Business dialogue data is missing!");
            }
        }
    }

    public void EndBusiness()
    {
        StartDialogue(dialogueCollection.businessEndDialogue);
    }
    #endregion

    #region Chapter 5: Secret Recipe
    public void StartOldManCritique()
    {
        StartDialogue(dialogueCollection.oldManCritiqueDialogue);
    }

    public void StartLegendaryRecipe()
    {
        StartDialogue(dialogueCollection.legendaryRecipeDialogue);
    }
    #endregion

    #region Chapter 6: Truth
    public void StartChenShop()
    {
        Debug.Log("Starting Chen shop scene");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载场景并设置回调
        SceneManager.Instance.LoadScene(SceneManager.GameScene.MasterShop, true, () => {
            Debug.Log("Chen shop scene loaded, starting dialogue");
            StartCoroutine(StartChenShopDialogue());
        });
    }

    private IEnumerator StartChenShopDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.chenShopDialogue != null)
            {
                Debug.Log("Starting Chen shop dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.chenShopDialogue);
            }
            else
            {
                Debug.LogError("Chen shop dialogue data is missing!");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }

    public void StartFindingFather()
    {
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Suburbs);
        StartDialogue(dialogueCollection.findingFatherDialogue);
    }

    public void StartReunion()
    {
        StartDialogue(dialogueCollection.reunionDialogue);
    }
    #endregion

    #region Chapter 7: New Journey
    public void StartSuccess()
    {
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Kuanzhai);
        StartDialogue(dialogueCollection.successDialogue);
    }

    public void StartNewJourney()
    {
        Debug.Log("Starting new journey scene");
        
        // 先重置对话管理器状态
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideDialogue();
            DialogueManager.Instance.ResetState();
        }

        // 加载场景并设置回调
        SceneManager.Instance.LoadScene(SceneManager.GameScene.Kuanzhai, true, () => {
            Debug.Log("New journey scene loaded, starting dialogue");
            StartCoroutine(StartNewJourneyDialogue());
        });
    }

    private IEnumerator StartNewJourneyDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        // 确保对话面板已创建
        if (GameManager.Instance.dialoguePanelPrefab != null)
        {
            // 创建或获取Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                Canvas canvasComponent = canvas.GetComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.sortingOrder = 1;

                if (FindObjectOfType<EventSystem>() == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }
            }

            // 创建对话面板
            GameObject dialoguePanel = Instantiate(GameManager.Instance.dialoguePanelPrefab, canvas.transform);
            DialogueManager.Instance.SetDialoguePanel(dialoguePanel, true);

            // 开始对话
            if (dialogueCollection != null && dialogueCollection.newJourneyDialogue != null)
            {
                Debug.Log("Starting new journey dialogue");
                DialogueManager.Instance.StartDialogue(dialogueCollection.newJourneyDialogue);
            }
            else
            {
                Debug.LogError("New journey dialogue data is missing!");
            }
        }
        else
        {
            Debug.LogError("Dialogue panel prefab is missing!");
        }
    }
    #endregion

    private void StartDialogue(DialogueData dialogue)
    {
        if (DialogueManager.Instance != null && dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogError("DialogueManager instance or dialogue data is missing!");
        }
    }
} 