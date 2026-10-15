using UnityEngine;

public class Counter : Interactable
{
    [SerializeField] private ItemType itemFornecido;

    public ItemType ItemFornecido => itemFornecido;

    public override void Interact(GameObject interactor)
    {
        if (interactor != null && interactor.TryGetComponent<PlayerInventory>(out var player))
        {
            if (player.ItemNaMao == ItemType.Nenhum)
            {
                player.ItemNaMao = itemFornecido;
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.ShowMessage($"Você pegou: {itemFornecido}!");
                }
            }
            else
            {
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.ShowMessage("Você já está segurando um item!");
                }
            }
        }
    }
}