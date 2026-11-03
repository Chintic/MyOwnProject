using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable interactableProximo;
    public PlayerInventory Inventory { get; private set; }
    [SerializeField] private Transform heldItem;

    private void Awake()
    {
        Inventory = GetComponent<PlayerInventory>();
    }

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
        // here, even when I interact with TrashCan, it will call this and move the held item. I can either reset held item every time or correct this behavior
        if (Inventory.ItemNaMao != null)
        {
            ThrowItem();
        }

        if (interactableProximo != null)
        {
            interactableProximo.Interact(this);
        }     

    }

    private void ThrowItem()
    {
        // Here I have got to reference player's child object transform "Held Item"
       
        heldItem.position += new Vector3(1, 0, 0);
    }

    // Detecta quando o jogador entra na área de gatilho (Trigger) do interagível
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Interactable>(out var interactable))
        {
            interactableProximo = interactable;
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

