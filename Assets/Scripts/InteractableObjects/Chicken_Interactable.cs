using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Chicken_Interactable : Interactable
{
    private bool follow = false;
    public Animator anim;
    private Vector3 spawnLocation;
    [SerializeField] private float maximumDistance = 5f;
    [SerializeField] private float speed = 2f; // Adjust the speed as needed
    [SerializeField] private Vector3 followOffset = new Vector3(-1f, -0.5f, 0f);
    private PlayerInteraction interactor; // <-- campo para guardar o interactor
    private Vector3 direction;

    private void Awake()
    {
        spawnLocation = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interact(PlayerInteraction interactor)
    {
        DialogueManager.Instance?.ShowMessage("Piu!");
        this.interactor = interactor; // <-- armazena para uso posterior
        follow = true;
    }

    private void Update()
    {
        if (follow && 
            interactor != null && 
            Vector3.Distance(interactor.GetComponentInParent<Transform>().position, transform.position - followOffset) > 0.1f)
        {
            anim.SetBool("isMoving", true);
            // Move the chicken towards the player
            direction = (interactor.transform.position + followOffset - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if(Vector3.Distance(interactor.GetComponentInParent<Transform>().position, transform.position) > maximumDistance)
            {
                follow = false;                
            }
        }

        else if(!follow)
        {
            // Quero garantir que quando ela estiver voltando para casa, a animação isMoving esteja ativa. Atualmente é true por coincidencia de que ela estava seguindo o player. Mas caso o player de repente se teleportasse, ela voltaria com o idle
            anim.SetBool("isMoving", true);
            direction = (spawnLocation - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(spawnLocation, transform.position) < 0.1f)
            {
                anim.SetBool("isMoving", false);
            }
        }


        else 
        {
            anim.SetBool("isMoving", false);
        }
    }

}
