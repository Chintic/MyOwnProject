using UnityEngine;

public class TrashCan : Interactable    
{
    // Implementa o método abstrato herdado
    public override void Interact(GameObject interactor)
    {
        if (interactor != null && interactor.TryGetComponent<PlayerInventory>(out var player))
        {
            if (player.ItemNaMao != ItemType.Nenhum)
            {
                player.ItemNaMao = ItemType.Nenhum; // Remove o item da mão do jogador
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.ShowMessage("Seu item foi descartado no lixo!");
                }
            }
            else
            {
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.ShowMessage("Não há itens para serem descartados!");
                }
            }
        }
    }
}

