using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDataCollection", menuName = "Game/Dialogue Data Collection")]
public class DialogueDataCollection : ScriptableObject
{
    [Header("Chapter 1: Opening")]
    public DialogueData openingDialogue;  // 开场对话
    public DialogueData busToChengduDialogue;  // 去成都的路上

    [Header("Chapter 2: Kuanzhai")]
    public DialogueData kuanzhaiIntroDialogue;  // 宽窄巷子初到
    public DialogueData cookingSchoolDialogue;  // 添加烹饪学校对话
    public DialogueData firstSuccessDialogue;   // 第一次烹饪成功
    public DialogueData meetOldManDialogue;  // 遇见老头

    [Header("Chapter 3: Learning")]
    public DialogueData cookingLessonDialogue;  // 学习做菜

    [Header("Chapter 4: Business")]
    public DialogueData businessStartDialogue;  // 开始经营
    public DialogueData businessEndDialogue;  // 经营结束

    [Header("Chapter 5: Secret Recipe")]
    public DialogueData oldManCritiqueDialogue;  // 老头点评
    public DialogueData legendaryRecipeDialogue;  // 传说配方

    [Header("Chapter 6: Truth")]
    public DialogueData chenShopDialogue;  // 陈记店
    public DialogueData findingFatherDialogue;  // 寻找父亲
    public DialogueData reunionDialogue;  // 重逢对话

    [Header("Chapter 7: New Journey")]
    public DialogueData successDialogue;  // 成功对话
    public DialogueData newJourneyDialogue;  // 新的旅程
} 