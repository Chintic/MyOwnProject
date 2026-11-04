using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable interactableProximo;
    public PlayerInventory Inventory { get; private set; }
    [SerializeField] private Transform heldItem;
    private Vector3 initialHeldItemLocation;
    private Vector3 thrownItemTarget;
    private PlayerMovement playerMovement;
    private float fallItemHeight;
    private bool isThrowing;

    private void Awake()
    {
        Inventory = GetComponent<PlayerInventory>();
        playerMovement = GetComponent<PlayerMovement>();
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
        if (isThrowing) return;

        isThrowing = true;

        if(playerMovement.FacingDirection.y > 0)
        {
            fallItemHeight = 0;
        }
        else
        {
            fallItemHeight = 1;
        }

        // with localPosition we access this GO relatively to parent's position
        initialHeldItemLocation = heldItem.localPosition;
        thrownItemTarget = heldItem.localPosition + new Vector3(playerMovement.FacingDirection.x, playerMovement.FacingDirection.y - fallItemHeight, 0);
        // Here I have got to reference player's child object transform "Held Item"

        StartCoroutine(ThrowItemRoutine());
    }

    // Detecta quando o jogador entra na área de gatilho (Trigger) do interagível
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Interactable>(out var interactable))
        {
            interactableProximo = interactable;
        }
    }

    private IEnumerator ThrowItemRoutine()
    {
        while (heldItem.localPosition != thrownItemTarget)
        {
            heldItem.localPosition = Vector3.MoveTowards(heldItem.localPosition, thrownItemTarget, 5f * Time.deltaTime);
            yield return null;
        }
        Inventory.ItemNaMao = null;
        heldItem.localPosition = initialHeldItemLocation;

        isThrowing = false;

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

