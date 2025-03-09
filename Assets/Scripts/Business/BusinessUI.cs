using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusinessUI : MonoBehaviour
{
    [Header("Business Control")]
    public Button startBusinessButton;
    public Button endBusinessButton;

    [Header("Business Info")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI incomeText;

    [Header("Debug")]
    public TextMeshProUGUI customerCountText;

    [Header("Cooking")]
    public Button completeOrderButton;

    private BusinessManager businessManager;

    private void Start()
    {
        businessManager = BusinessManager.Instance;
        
        if (businessManager == null)
        {
            Debug.LogError("找不到BusinessManager实例");
            return;
        }
        
        // 设置按钮事件
        if (startBusinessButton != null)
            startBusinessButton.onClick.AddListener(OnStartBusinessClicked);
        
        if (endBusinessButton != null)
            endBusinessButton.onClick.AddListener(OnEndBusinessClicked);
        
        if (completeOrderButton != null)
            completeOrderButton.onClick.AddListener(OnCompleteOrderClicked);
        
        // 初始化UI状态
        UpdateUI();
        
        // 默认禁用结束按钮
        if (endBusinessButton != null)
            endBusinessButton.interactable = false;
    }

    private void Update()
    {
        // 实时更新UI
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (businessManager == null) return;
        
        // 更新金钱显示
        if (moneyText != null)
            moneyText.text = $"总资金: {businessManager.totalMoney:F2}元";
        
        // 更新收入显示
        if (incomeText != null)
            incomeText.text = $"今日收入: {businessManager.dailyIncome:F2}元";
        
        // 更新按钮状态
        if (startBusinessButton != null)
            startBusinessButton.interactable = !businessManager.isOperating;
        
        if (endBusinessButton != null)
            endBusinessButton.interactable = businessManager.isOperating;
    }

    private void OnStartBusinessClicked()
    {
        if (businessManager != null)
        {
            businessManager.StartBusiness();
        }
    }

    private void OnEndBusinessClicked()
    {
        if (businessManager != null)
        {
            businessManager.EndBusiness();
        }
    }

    private void OnCompleteOrderClicked()
    {
        if (businessManager != null)
        {
            businessManager.CompleteFirstCustomerOrder();
        }
    }
} 