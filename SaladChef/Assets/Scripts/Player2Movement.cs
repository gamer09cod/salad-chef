using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : Movement
{
    public override void Update()
    {
        if (!canMove)
            return;

        HandleMovement();
        Animate();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            moveY += 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            moveY -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveX -= 1f;
        if (Input.GetKey(KeyCode.RightArrow))
            moveX += 1f;

        movement = new Vector2(moveX, moveY).normalized;
    }
}
