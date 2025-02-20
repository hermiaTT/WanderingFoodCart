using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Game/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public enum DialogueType
    {
        Opening,         // 开场
        BusToChengdu,   // 去成都的大巴上
        WideAlley,      // 宽窄巷子
        CookingSchool,  // 烹饪学校
        FirstSuccess,   // 第一次烹饪成功
        Business,       // 经营阶段
        ChenShop,       // 陈麻婆店
        FindRecipe,     // 寻找配方
        SecretRecipe,   // 秘密配方
        Truth,          // 真相
        NewJourney      // 新的旅程
    }

    [System.Serializable]
    public class DialogueSection
    {
        public string speakerName;
        [TextArea(3, 10)]
        public string[] sentences;
        public bool triggerSceneChange;  // 是否触发场景切换
        public SceneManager.GameScene nextScene;  // 下一个场景
        public bool triggerGameplay;     // 是否触发游戏玩法
        public string gameplayType;      // 游戏玩法类型
    }

    public DialogueType type;
    public DialogueSection[] sections;
} 