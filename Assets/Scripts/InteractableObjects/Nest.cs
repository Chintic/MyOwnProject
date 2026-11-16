using UnityEngine;

public class Nest : Interactable
{
    public Chicken_Interactable chicken;

    [SerializeField] private SpriteRenderer itemRenderer;

    private ItemsScriptableObject item;

    public ItemsScriptableObject Item
    {
        get => item;

        set
        {
            item = value;
            itemRenderer.sprite = item != null ? item.sprite : null;
        }
    }

    public override void Interact(PlayerInteraction interactor)
    {

        if (interactor.Inventory.ItemNaMao == null && item != null)
        {
            interactor.Inventory.ItemNaMao = item;
            item = null;
            itemRenderer.sprite = null;


        }
    }
}