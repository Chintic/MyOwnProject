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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Chicken_Interactable>(out var enteredChicken))
            return;

        if (chicken != null)
            return;

        if (enteredChicken.TryAssignNest(this))
        {
            chicken = enteredChicken;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Chicken_Interactable>(out var exitingChicken))
            return;

        if (chicken == exitingChicken)
        {
            chicken = null;
            exitingChicken.LeaveNest(this);
        }
    }
}