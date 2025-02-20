using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueData dialogueData;  // 改为使用 DialogueData

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueData);  // 直接传入 DialogueData
        }
        else
        {
            Debug.LogError("DialogueManager instance not found!");
        }
    }
} 