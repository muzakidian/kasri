using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public PlayerState currentState;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private Vector2 inputMovement;
    public VectorValue startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        currentState = PlayerState.walk;
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (currentState == PlayerState.walk)
        {
            UpdateAnimationAndMove();    
        }    
    }
    void Update()
    {
        // change = Vector3.zero;
        // change.x = Input.GetAxisRaw("Horizontal");
        // change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack )
        {
            StartCoroutine(AttackCo());
        }
        
        // else if (currentState == PlayerState.walk)
        // {
        //     UpdateAnimationAndMove();    
        // }
        
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        } else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }


    // private void FixedUpdate() 
    // {
    //     Vector2 delta = inputMovement * velocity * Time.deltaTime;
    //     Vector2 newPosition = characterBody.position + delta;
    //     characterBody.MovePosition(newPosition);    
    // }
}
