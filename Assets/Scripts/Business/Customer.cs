using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customer : MonoBehaviour
{
    [Header("Customer Properties")]
    public float patience = 120f;         // 耐心值（秒）
    public float waitingTime = 0f;        // 已等待时间
    public float spendingPower = 1f;      // 消费能力 (1.0 = 标准)
    public bool isPatient = false;        // 是否特别有耐心

    [Header("Movement")]
    public float moveSpeed = 2f;          // 移动速度

    [Header("Visual")]
    public GameObject thoughtBubble;      // 想法气泡
    public GameObject thoughtBubblePrefab; // 新增：思想气泡预制体
    public SpriteRenderer dishImage;      // 想要的菜的图片
    public TextMeshPro patienceText;      // 改为TextMeshPro，而不是TextMeshProUGUI

    private Vector3 targetPosition;       // 目标位置
    private bool isMoving = false;        // 是否在移动
    private bool isLeaving = false;       // 是否正在离开
    private bool hasReachedQueue = false; // 是否已到达排队位置
    private Recipe wantedDish;            // 想要的菜

    private void Awake()
    {
        // 如果没有耐心文本，创建一个
        if (patienceText == null)
        {
            CreatePatienceText();
        }
        
        // 如果没有思想气泡，并且有预制体，则实例化它
        if (thoughtBubble == null && thoughtBubblePrefab != null)
        {
            InstantiateThoughtBubble();
        }
        else if (thoughtBubble == null)
        {
            // 作为备选，如果没有预制体，则创建简单气泡
            CreateSimpleThoughtBubble();
        }
    }

    // 创建耐心文本
    private void CreatePatienceText()
    {
        // 创建一个新的游戏对象
        GameObject textObj = new GameObject("PatienceText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0, 1.0f, 0);
        
        // 添加TextMeshPro组件
        patienceText = textObj.AddComponent<TextMeshPro>();
        patienceText.alignment = TextAlignmentOptions.Center;
        patienceText.fontSize = 5;
        patienceText.text = "100%";
        patienceText.color = Color.green;
    }

    // 实例化预制体思想气泡
    private void InstantiateThoughtBubble()
    {
        // 实例化预制体
        thoughtBubble = Instantiate(thoughtBubblePrefab, transform);
        thoughtBubble.transform.localPosition = new Vector3(0, 1.3f, 0);
        thoughtBubble.name = "ThoughtBubble";
        
        // 确保它被禁用
        thoughtBubble.SetActive(false);
        
        Debug.Log("已从预制体创建思想气泡");
    }
    
    // 如果没有预制体，创建简单气泡
    private void CreateSimpleThoughtBubble()
    {
        // 创建一个空的游戏对象作为父对象
        GameObject bubbleObj = new GameObject("ThoughtBubble");
        bubbleObj.transform.SetParent(transform);
        bubbleObj.transform.localPosition = new Vector3(0, 1.3f, 0);
        
        // 创建一个简单的白色方块
        GameObject visualObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        visualObj.name = "BubbleVisual";
        visualObj.transform.SetParent(bubbleObj.transform);
        visualObj.transform.localPosition = Vector3.zero;
        visualObj.transform.localRotation = Quaternion.Euler(0, 180, 0); // 让它面向相机
        visualObj.transform.localScale = new Vector3(1f, 1f, 1f);
        
        // 设置颜色为明亮的黄色，使其更容易看见
        Renderer renderer = visualObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Unlit/Color"));
            renderer.material.color = Color.yellow;
        }
        
        // 创建文本对象
        GameObject textObj = new GameObject("OrderText");
        textObj.transform.SetParent(bubbleObj.transform);
        textObj.transform.localPosition = new Vector3(0, 0, -0.1f);
        
        // 添加TextMeshPro组件
        TextMeshPro textMesh = textObj.AddComponent<TextMeshPro>();
        textMesh.text = "???";
        textMesh.fontSize = 3;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = Color.black;
        textMesh.rectTransform.sizeDelta = new Vector2(1, 0.5f);
        
        // 默认隐藏气泡
        bubbleObj.SetActive(false);
        
        // 保存引用
        thoughtBubble = bubbleObj;
        
        Debug.Log("已创建简单思想气泡");
    }

    // 初始化顾客
    public void Initialize(float patienceValue, float spendingValue)
    {
        patience = patienceValue;
        spendingPower = spendingValue;
        isPatient = Random.value < 0.2f;
        
        // 隐藏思想气泡
        if (thoughtBubble != null)
        {
            thoughtBubble.SetActive(false);
        }
        
        // 确保游戏对象是激活的
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("顾客对象未激活，无法启动协程！");
            return;
        }
        
        // 开始等待计时
        StartCoroutine(WaitInQueue());
    }

    // 等待队列协程
    private IEnumerator WaitInQueue()
    {
        // 显示初始耐心值
        UpdatePatienceDisplay();
        
        while (waitingTime < patience && !isLeaving)
        {
            // 只有到达队列位置后才开始计算等待时间
            if (hasReachedQueue)
            {
                waitingTime += Time.deltaTime;
                
                // 更新耐心显示
                UpdatePatienceDisplay();
                
                // 如果已经等了一段时间，显示想要的菜
                if (waitingTime > 2f && thoughtBubble != null && !thoughtBubble.activeSelf)
                {
                    Debug.Log("准备点单和显示气泡...");
                    DecideWantedDish();
                    ShowThoughtBubble();
                    Debug.Log($"气泡状态: {(thoughtBubble != null ? thoughtBubble.activeSelf.ToString() : "null")}，位置: {thoughtBubble.transform.position}");
                }
                
                // 如果耐心快耗尽，有一定概率离开
                if (waitingTime > patience * 0.8f && !isPatient)
                {
                    float leaveChance = 0.001f * Time.deltaTime * 60f; // 每分钟0.1的概率
                    if (Random.value < leaveChance)
                    {
                        Leave();
                        yield break;
                    }
                }
            }
            
            yield return null;
        }
        
        // 耐心耗尽，离开
        if (waitingTime >= patience && !isLeaving)
        {
            Leave();
        }
    }

    // 决定想要的菜
    private void DecideWantedDish()
    {
        // 基本逻辑保持不变
        wantedDish = new Recipe { name = "麻婆豆腐", price = 15f, difficultyLevel = 2 };
        
        // 查找并更新文本
        if (thoughtBubble != null)
        {
            // 在整个层次结构中递归查找文本组件
            TextMeshPro[] textComponents = thoughtBubble.GetComponentsInChildren<TextMeshPro>(true);
            foreach (var text in textComponents)
            {
                if (text.gameObject.name == "OrderText" || text.transform.parent.name == "ThoughtBubble")
                {
                    text.text = wantedDish.name;
                    break;
                }
            }
        }
        
        // 通知BusinessManager顾客已点单
        if (BusinessManager.Instance != null)
        {
            BusinessManager.Instance.CustomerOrder(this, wantedDish);
        }
    }

    // 显示思想气泡
    private void ShowThoughtBubble()
    {
        if (thoughtBubble != null)
        {
            thoughtBubble.SetActive(true);
            Debug.Log("显示思想气泡: " + (thoughtBubble.activeSelf ? "可见" : "不可见"));
        }
        else
        {
            Debug.LogWarning("思想气泡对象为空，无法显示");
        }
    }

    // 更新耐心显示
    private void UpdatePatienceDisplay()
    {
        if (patienceText != null)
        {
            float patiencePercent = 1f - (waitingTime / patience);
            patienceText.text = Mathf.RoundToInt(patiencePercent * 100) + "%";
            
            // 根据耐心值改变颜色
            if (patiencePercent > 0.6f)
                patienceText.color = Color.green;
            else if (patiencePercent > 0.3f)
                patienceText.color = Color.yellow;
            else
                patienceText.color = Color.red;
        }
    }

    // 移动到指定位置
    public void MoveToPosition(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
        StartCoroutine(MoveRoutine());
    }

    // 移动协程
    private IEnumerator MoveRoutine()
    {
        while (isMoving && Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        // 到达目标位置
        transform.position = targetPosition;
        isMoving = false;
        
        // 标记为已到达队列位置
        hasReachedQueue = true;
    }

    // 离开
    public void Leave()
    {
        if (isLeaving) return;
        
        isLeaving = true;
        
        // 如果正在显示气泡，隐藏它
        if (thoughtBubble != null)
        {
            thoughtBubble.SetActive(false);
        }
        
        // 移动到屏幕外
        Vector3 exitPosition = transform.position + Vector3.left * 10f;
        MoveToPosition(exitPosition);
        
        // 销毁自己
        Destroy(gameObject, 3f);
        
        Debug.Log("顾客不耐烦离开了");
    }
} 