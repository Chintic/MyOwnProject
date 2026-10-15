using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable interactableProximo;

    private void Update()
    {
        // Se apertar a tecla E no teclado
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TentarInteragir();
        }
    }

    private void TentarInteragir()
    {
        if (interactableProximo != null)
        {
            interactableProximo.Interact(gameObject);
        }
    }

    // Detecta quando o jogador entra na área de gatilho (Trigger) do interagível
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Interactable>(out var interactable))
        {
            interactableProximo = interactable;
            if (DialogueManager.Instance != null && !DialogueManager.Instance.IsShowing)
            {
                DialogueManager.Instance.ShowMessage(interactable.PromptMessage, 2.5f);
            }
        }
    }

    // Detecta quando o jogador se afasta do interagível
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Interactable>(out var interactable) && interactableProximo == interactable)
        {
            interactableProximo = null;
            if (DialogueManager.Instance != null && DialogueManager.Instance.IsShowing)
            {
                DialogueManager.Instance.HideDialogue();
            }
        }
    }
}

