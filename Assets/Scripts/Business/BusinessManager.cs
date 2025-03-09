using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    public static BusinessManager Instance { get; private set; }

    [Header("Business State")]
    public bool isOperating = false;         // 是否在营业
    public float dailyIncome = 0f;           // 当日收入
    public float dailyExpense = 0f;          // 当日支出
    public float totalMoney = 1000f;         // 总资金

    [Header("Customer Settings")]
    public float baseCustomerSpawnInterval = 15f;  // 基础顾客生成间隔
    public int maxQueueLength = 8;                // 最大队列长度
    public Transform spawnPoint;                  // 顾客生成点
    public Transform queueStartPoint;             // 排队起点
    public float queueSpacing = 1.5f;             // 排队间距

    [Header("Prefabs")]
    public GameObject customerPrefab;         // 顾客预制体

    [Header("Orders")]
    public int maxOrdersAtOnce = 3;  // 同时最多处理的订单数
    public List<Recipe> availableRecipes = new List<Recipe>();  // 可供选择的菜单

    private Queue<Customer> customerQueue = new Queue<Customer>();
    private List<Customer> activeCustomers = new List<Customer>();
    private bool isSpawningCustomers = false;

    // 添加订单列表
    private List<Order> pendingOrders = new List<Order>();
    private List<Order> activeOrders = new List<Order>();

    [System.Serializable]
    public class Order
    {
        public Customer customer;
        public Recipe recipe;
        public float orderTime;
        
        public Order(Customer customer, Recipe recipe)
        {
            this.customer = customer;
            this.recipe = recipe;
            this.orderTime = Time.time;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 开始营业
    public void StartBusiness()
    {
        if (isOperating) return;

        Debug.Log("开始营业");
        isOperating = true;
        dailyIncome = 0f;
        dailyExpense = 0f;
        
        isSpawningCustomers = true;
        StartCoroutine(SpawnCustomersRoutine());
    }

    // 结束营业
    public void EndBusiness()
    {
        if (!isOperating) return;

        Debug.Log("结束营业");
        isOperating = false;
        isSpawningCustomers = false;

        // 清理所有顾客
        foreach (var customer in activeCustomers)
        {
            if (customer != null)
            {
                Destroy(customer.gameObject);
            }
        }
        activeCustomers.Clear();
        customerQueue.Clear();

        // 计算今日收益
        float dailyProfit = dailyIncome - dailyExpense;
        totalMoney += dailyProfit;

        Debug.Log($"今日收入: {dailyIncome}，支出: {dailyExpense}，利润: {dailyProfit}");
    }

    // 生成顾客
    private IEnumerator SpawnCustomersRoutine()
    {
        while (isSpawningCustomers)
        {
            // 等待生成间隔
            yield return new WaitForSeconds(baseCustomerSpawnInterval);

            // 检查队列长度
            if (customerQueue.Count >= maxQueueLength)
            {
                // 队列太长，可能不生成新顾客
                float chanceToLeave = 0.7f;
                if (Random.value < chanceToLeave)
                {
                    Debug.Log("队列太长，顾客离开了");
                    continue;
                }
            }

            // 生成顾客
            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
    {
        if (customerPrefab != null && spawnPoint != null)
        {
            GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            
            // 确保对象是激活的
            customerObj.SetActive(true);
            
            Customer customer = customerObj.GetComponent<Customer>();
            
            if (customer != null)
            {
                // 设置顾客属性
                customer.Initialize(CalculateCustomerPatience(), DetermineSpendingPower());
                
                // 加入队列
                customerQueue.Enqueue(customer);
                activeCustomers.Add(customer);
                
                // 计算队列位置
                UpdateQueuePositions();
                
                Debug.Log($"生成了新顾客，队列长度: {customerQueue.Count}");
            }
        }
        else
        {
            Debug.LogError("缺少顾客预制体或生成点");
        }
    }

    // 更新排队位置
    private void UpdateQueuePositions()
    {
        if (queueStartPoint == null) return;

        // 创建顾客队列的临时列表
        Customer[] queueArray = customerQueue.ToArray();
        
        for (int i = 0; i < queueArray.Length; i++)
        {
            if (queueArray[i] != null)
            {
                // 计算目标位置 - 修改这里的计算方式
                // 使用right而不是forward，确保顾客水平排列
                // 或者根据你的场景需要调整方向
                Vector3 targetPosition = queueStartPoint.position + Vector3.right * i * queueSpacing;
                
                // 让顾客移动到该位置
                queueArray[i].MoveToPosition(targetPosition);
                
                // 调试信息
                Debug.Log($"顾客 {i} 目标位置: {targetPosition}");
            }
        }
    }

    // 顾客下单
    public void CustomerOrder(Customer customer, Recipe wantedDish)
    {
        if (!isOperating) return;
        
        // 创建新订单
        Order newOrder = new Order(customer, wantedDish);
        pendingOrders.Add(newOrder);
        
        // 如果活跃订单数量未达到上限，可以立即处理
        TryProcessNextOrder();
        
        Debug.Log($"顾客点了 {wantedDish.name}，待处理订单: {pendingOrders.Count}，正在处理订单: {activeOrders.Count}");
    }

    // 尝试处理下一个订单
    private void TryProcessNextOrder()
    {
        // 如果活跃订单数量已达上限，不处理新订单
        if (activeOrders.Count >= maxOrdersAtOnce) return;
        
        // 如果有待处理的订单，处理第一个
        if (pendingOrders.Count > 0)
        {
            Order orderToProcess = pendingOrders[0];
            pendingOrders.RemoveAt(0);
            activeOrders.Add(orderToProcess);
            
            Debug.Log($"开始处理 {orderToProcess.recipe.name} 订单");
        }
    }

    // 完成订单
    public void CompleteOrder(Customer customer, float qualityScore)
    {
        if (!isOperating || customer == null) return;
        
        // 查找对应的订单
        Order order = null;
        foreach (var o in activeOrders)
        {
            if (o.customer == customer)
            {
                order = o;
                break;
            }
        }
        
        // 如果找不到订单，尝试处理第一个活跃订单
        if (order == null && activeOrders.Count > 0)
        {
            order = activeOrders[0];
        }
        
        // 计算收入并完成订单
        if (order != null)
        {
            activeOrders.Remove(order);
            
            // 计算收入
            float income = CalculateIncome(order.customer, qualityScore);
            dailyIncome += income;
            
            Debug.Log($"完成订单 {order.recipe.name}，收入: {income}");
            
            // 顾客离开
            activeCustomers.Remove(order.customer);

            // 从队列中移除该顾客（使用新的方法）
            customerQueue = RemoveCustomerFromQueue(customerQueue, order.customer);

            Destroy(order.customer.gameObject);
            
            // 尝试处理下一个订单
            TryProcessNextOrder();
            
            // 更新队列位置
            UpdateQueuePositions();
        }
    }

    // 计算收入
    private float CalculateIncome(Customer customer, float qualityScore)
    {
        // 基础收入 + 根据质量和顾客消费能力计算的小费
        float baseIncome = 15f;  // 基本菜价
        float tip = customer.spendingPower * qualityScore * 5f;
        
        return baseIncome + tip;
    }

    // 计算顾客耐心值
    private float CalculateCustomerPatience()
    {
        // 基础耐心值 + 随机波动
        float basePatience = 120f;
        float randomFactor = Random.Range(0.8f, 1.2f);
        
        // 有小概率出现特别有耐心的顾客
        bool isVeryPatient = Random.value < 0.2f;
        float patienceFactor = isVeryPatient ? 2f : 1f;
        
        return basePatience * randomFactor * patienceFactor;
    }

    // 决定顾客消费能力
    private float DetermineSpendingPower()
    {
        // 基础消费能力 + 随机波动
        float baseSpending = 1.0f;
        float randomFactor = Random.Range(0.7f, 1.5f);
        
        return baseSpending * randomFactor;
    }

    // 模拟完成第一个排队顾客的订单
    public void CompleteFirstCustomerOrder()
    {
        if (!isOperating) 
        {
            Debug.Log("未在营业");
            return;
        }
        
        // 如果有活跃订单，完成第一个
        if (activeOrders.Count > 0)
        {
            Order order = activeOrders[0];
            activeOrders.RemoveAt(0);
            
            // 随机生成一个菜品质量评分(0.5-1.0)
            float qualityScore = Random.Range(0.5f, 1.0f);
            
            // 计算收入
            float income = CalculateIncome(order.customer, qualityScore);
            dailyIncome += income;
            
            Debug.Log($"完成订单 {order.recipe.name}，收入: {income}，质量评分: {qualityScore:F2}");
            
            // 顾客离开
            activeCustomers.Remove(order.customer);
            
            // 从队列中移除该顾客
            customerQueue = RemoveCustomerFromQueue(customerQueue, order.customer);
            
            Destroy(order.customer.gameObject);
            
            // 尝试处理下一个订单
            TryProcessNextOrder();
            
            // 更新队列位置
            UpdateQueuePositions();
        }
        else if (pendingOrders.Count > 0)
        {
            Debug.Log("有待处理订单，但尚未开始制作");
            TryProcessNextOrder();
        }
        else if (customerQueue.Count > 0)
        {
            Debug.Log("没有活跃订单，但有顾客在排队");
        }
        else
        {
            Debug.Log("没有顾客或活跃订单");
        }
    }

    // 将扩展方法改为普通的实例方法
    private Queue<Customer> RemoveCustomerFromQueue(Queue<Customer> queue, Customer customer)
    {
        // 创建一个新队列，不包含要移除的顾客
        Queue<Customer> newQueue = new Queue<Customer>();
        foreach (var c in queue)
        {
            if (c != customer)
            {
                newQueue.Enqueue(c);
            }
        }
        
        return newQueue;
    }
} 