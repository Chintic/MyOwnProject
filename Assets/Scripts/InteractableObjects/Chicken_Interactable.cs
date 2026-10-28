using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    static List<Chicken_Interactable> chickensList = new List<Chicken_Interactable>();
    private Vector3 targetPosition = new Vector3();

    private void Awake()
    {
        spawnLocation = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interact(PlayerInteraction interactor)
    {
        DialogueManager.Instance?.ShowMessage("Piu!");
        this.interactor = interactor; // <-- armazena para uso posterior
        if (!chickensList.Contains(this))
        {
            chickensList.Add(this);
        }
        follow = true;
    }

    private void Update()
    {

        int index = chickensList.IndexOf(this);

        if (index == 0)
        {
            targetPosition = interactor.GetComponentInParent<Transform>().position;
        }
        else if (index > 0)
        {
            targetPosition = chickensList[index - 1].transform.position;
        }

        if (follow && 
            interactor != null && 
            Vector3.Distance(targetPosition, transform.position - followOffset) > 0.1f)
        {
            anim.SetBool("isMoving", true);
            // Move the chicken towards the player
            direction = (targetPosition + followOffset - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if(Vector3.Distance(targetPosition, transform.position) > maximumDistance)
            {
                follow = false;
                chickensList.Remove(this);
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
