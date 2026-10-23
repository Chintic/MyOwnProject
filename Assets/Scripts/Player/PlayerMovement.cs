using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public event Action<Vector2> OnMovementChanged;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // public Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Lê os comandos de direção do teclado moderno (WASD / Setas)
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y -= 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1f;

        // Normaliza para não andar mais rápido na diagonal
        moveInput = moveInput.normalized;

        // Para o uso de Action, events e etc, irei retirar o Animate daqui, criar um Action para gritar que o player está se movendo,
        // e o Animator irá escutar esse Action para animar o player. Nesse action irei passar o vetor de movimento, e o Animator irá usar esse vetor para animar o player.
        //Animate();

        OnMovementChanged?.Invoke(moveInput);
    }

    private void FixedUpdate()
    {
        // No Unity 6, usamos linearVelocity para movimentação física suave
        rb.linearVelocity = moveInput * moveSpeed;
    }


    //Esse método irá sumir, e será substituído por um Action que o Animator irá escutar para animar o player.
    //private void Animate()
    //{
    //    isMoving = moveInput.sqrMagnitude > 0.01f;

    //    if (isMoving)
    //    {
    //        anim.SetFloat("X", moveInput.x);
    //        anim.SetFloat("Y", moveInput.y);
    //    }

    //    anim.SetBool("isMoving", isMoving);
    //}
}