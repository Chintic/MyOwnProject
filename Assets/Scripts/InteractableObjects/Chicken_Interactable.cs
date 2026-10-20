using UnityEngine;

public class Chicken_Interactable : Interactable
{
    private bool follow = false;
    public Animator anim;
    [SerializeField] private float speed = 2f; // Adjust the speed as needed
    [SerializeField] private Vector3 followOffset = new Vector3(-1f, -0.5f, 0f);
    private PlayerInteraction interactor; // <-- campo para guardar o interactor
    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interact(PlayerInteraction interactor)
    {
        DialogueManager.Instance?.ShowMessage("Piu!");
        this.interactor = interactor; // <-- armazena para uso posterior
        follow = true;
    }

    private void Update()
    {
        if (follow && interactor != null && interactor.GetComponentInParent<Transform>().position != transform.position - followOffset)
        {
            anim.SetBool("isMoving", true);
            // Move the chicken towards the player
            Vector3 direction = (interactor.transform.position + followOffset - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        else
        {
            anim.SetBool("isMoving", false);
        }
    }

}
