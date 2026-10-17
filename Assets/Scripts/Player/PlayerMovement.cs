using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public Animator anim;
    private bool moving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Lê os comandos de direção do teclado moderno (WASD / Setas)
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
        }

        // Normaliza para não andar mais rápido na diagonal
        moveInput = input.normalized;
        Animate();
    }

    private void FixedUpdate()
    {
        // No Unity 6, usamos linearVelocity para movimentação física suave
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Animate()
    {
        if(moveInput.magnitude > 0.1f || moveInput.magnitude < -0.1f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            anim.SetFloat("X", moveInput.x);
            anim.SetFloat("Y", moveInput.y);
        }

        anim.SetBool("isMoving", moving);
    }
}