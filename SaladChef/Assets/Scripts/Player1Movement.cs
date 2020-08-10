using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Movement : Movement
{
    public override void Update()
    {
        HandleMovement();
        Animate();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.W))
            moveY += 1f;
        if (Input.GetKey(KeyCode.S))
            moveY -= 1f;
        if (Input.GetKey(KeyCode.A))
            moveX -= 1f;
        if (Input.GetKey(KeyCode.D))
            moveX += 1f;

        movement = new Vector2(moveX, moveY).normalized;
    }
}
