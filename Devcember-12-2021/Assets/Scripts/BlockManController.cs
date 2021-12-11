using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManController : MonoBehaviour
{
    [SerializeField]private float speed; //how fast the character moves
    private float direction; // tells if the player goes left or right
	private bool canJump; //verifies if the player can jump
    [SerializeField]private float jumpHeight; //how high the player can jump
	
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
		
		if(Input.GetKey(KeyCode.Space))
		{
			canJump = true;
		}
		else
		{
			canJump = false;
		}
    }
	
	void FixedUpdate()
	{
		rBody.velocity = (transform.right * direction * speed * Time.fixedDeltaTime); //left and right movement
		
		Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.065f), -Vector2.up, Color.red);
		
		if(canJump)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.066f), -Vector2.up * 0.001f);
			if(hit)
			{
				rBody.AddForce(transform.up * jumpHeight * Time.fixedDeltaTime, ForceMode2D.Impulse);
			}
		}
	}
}
