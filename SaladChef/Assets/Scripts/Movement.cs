using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Abstract Class to control the player and its movement.
/// </summary>
public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 movement;

    private Rigidbody2D rigidBody;
    private bool canMove = true;

    public bool CanMove { set { canMove = value; } }

    #region Animation
    private Animator animator;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    #endregion

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        if (!canMove)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Animate();
    }

    protected void Animate()
    {
        animator.SetFloat(Horizontal, movement.x);
        animator.SetFloat(Vertical, movement.y);
        animator.SetFloat(Speed, movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        //Movement
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void StopPlayer()
    {
        movement.x = 0;
        movement.y = 0;
        canMove = false;
        //rigidBody.MovePosition(new Vector2(0, 0));
    }
}
