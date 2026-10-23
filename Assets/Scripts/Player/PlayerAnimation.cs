using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInventory playerInventory;


    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerMovement.OnMovementChanged += Animate;
    }

    private void OnDisable()
    {
        playerMovement.OnMovementChanged -= Animate;
    }
    private void Animate(Vector2 moveInput)
    {
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            anim.SetFloat("X", moveInput.x);
            anim.SetFloat("Y", moveInput.y);
        }

        anim.SetBool("isCarrying", playerInventory.ItemNaMao != null);

        anim.SetBool("isMoving", isMoving);
    }

}
