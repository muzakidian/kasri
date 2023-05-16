using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Vector2 inputMovement;
    public VectorValue startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (change != Vector3.zero)
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
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
