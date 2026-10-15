using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Coroutine hideCoroutine;

    public bool IsShowing => dialogueBox != null && dialogueBox.activeSelf;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }

    /// <summary>
    /// Exibe uma mensagem na caixa de diálogo por uma duração especificada em segundos.
    /// Se a duração for <= 0, a mensagem permanece até ser fechada manualmente.
    /// </summary>
    public void ShowMessage(string message, float duration = 2.5f)
    {
        if (dialogueBox == null || dialogueText == null)
        {
            Debug.LogWarning("[DialogueManager] Referências de UI não configuradas.");
            return;
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        dialogueText.text = message;
        dialogueBox.SetActive(true);

        if (duration > 0f)
        {
            hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
        }
    }

    /// <summary>
    /// Oculta a caixa de diálogo imediatamente.
    /// </summary>
    public void HideDialogue()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
        hideCoroutine = null;
    }
}
