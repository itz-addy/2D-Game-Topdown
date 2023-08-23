using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator anim;
    List<RaycastHit2D> castCollision = new List<RaycastHit2D>();
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canMove)
        {
            if(movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if(!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0f));
                }

                if(!success)
                {
                    TryMove(new Vector2(0f, movementInput.y));
                }

                anim.SetBool("isMoving", success);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            //set direction of the sprite to movement direction
            if(movementInput.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(movementInput.x), 1f, 1f);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            int count = rb.Cast
            (direction, movementFilter, castCollision, moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;            
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        Debug.Log("Player Attack!");
        anim.SetTrigger("swordAttack");
    }

    void LockMovement()
    {
        canMove = false;
    }

    void UnlockMovement()
    {
        canMove = true;
    }
}
