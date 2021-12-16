using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BlockManController : MonoBehaviour
{
    [SerializeField]private float speed; //how fast the character moves
    private float direction; // tells if the player goes left or right

    #region Jump Properties

    [SerializeField]private float jumpHeight; //how high the player can jump
    private bool cmdJump; //Indicates a request to jump by player
    private bool canJump; //Can block a player from holding down spacebar to autojump
    private bool isGrounded;

    #endregion

    #region Meta Properties

    private new BoxCollider2D collider => transform.GetComponent<BoxCollider2D>();

    #endregion

    private Rigidbody2D rBody;
	
	
	void Start()
	{
		rBody = gameObject.GetComponent<Rigidbody2D>(); //get the rigidbody
    }
	
	void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
		{
			direction = -1;
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			direction = 1;
		}
		else
		{
			direction = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
            cmdJump = true;
            //Debug.Log("cmdJump is true");
		}
        else
        {
            cmdJump = false;
           // Debug.Log("cmdJump is false");
        }

        UpdateIsGrounded();
        UpdateCanJump();
    }
	
	void FixedUpdate()
	{
		rBody.velocity = (transform.right * direction * speed * Time.fixedDeltaTime); //left and right movement

        if(canJump && cmdJump && isGrounded)
        {
            Jump();
            Debug.Log("Jump is executed");
        }
	}

    #region Commands

    private void Jump()
    {
        rBody.AddForce(transform.up * jumpHeight * Time.fixedDeltaTime, ForceMode2D.Impulse);
        canJump = false;
        //Debug.Log("canJump is false");
    }

    #endregion

    #region Updates

    /// <summary>
    /// Used to confirm if the player is grounded and they have at least release the jump command before command a jump again
    /// </summary>
    private void UpdateCanJump()
    {
        if (!canJump && isGrounded && !cmdJump) //Player let go of jump in order for this to happen
        {
            canJump = true;
            //Debug.Log("canJump is true");
        } else if (!isGrounded)
        {
            canJump = false;
           // Debug.Log("canJump is false");
        }
    }

    private void UpdateIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,.1f, LayerMask.GetMask("ground")/*,  2 Ignore Raycast*/);
        Debug.DrawRay(transform.position, -Vector2.up, Color.white);
        if (hit.collider != null)
        {
            isGrounded = true;
          // Debug.Log("grounded is true");
        }
        else
        {
            isGrounded = false;
          // Debug.Log("grounded is false");
        }
    }

    #endregion

    #region Debug

    void OnDrawGizmos()
    {
        //DrawJumpRay();
        //DrawIsGrounded();
    }

    private void DrawJumpRay()
    {
        Gizmos.color = Color.yellow;
        Debug.DrawRay(transform.position, -Vector2.up * collider.size.y / 1.25f, Color.red);
    }

    private void DrawIsGrounded()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, Vector3.up);
    }

    #endregion
}
