using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MultiPlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class MultiPlayerMovement : MonoBehaviour
{
    public float speed;
    public PlayerState currentState;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Sinyal playerHealthSignal;
    private Vector2 inputMovement;
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;
    public Sinyal reduceMagic;

    [Header("Peluru")]
    public GameObject peluru;
    public Item watergun;

    [Header("Fungsi Kebal")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;
    [SerializeField] private AudioSource swordAttackSound;
    [SerializeField] private AudioSource damagedSound;
    [SerializeField] private AudioSource interactSound;
    [SerializeField] private AudioSource nembakSound;

    // Game Over
    // private bool isDead;
    public GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }
    void Update()
    {
        // Apakah player sedang dalam ineraksi?
        if(currentState == PlayerState.interact)
        {
            return;
        }

        if(Input.GetButtonDown("attack") && currentState != PlayerState.attack 
           && currentState != PlayerState.stagger)
        {
            swordAttackSound.Play();
            StartCoroutine(AttackCo());
        }
        else if (Input.GetButtonDown("Second Weapon") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
            if (playerInventory.CheckForItem(watergun))
            {
                nembakSound.Play();
                StartCoroutine(SecondAttackCo());
            }
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator SecondAttackCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        MakeWaterball();
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }
        private void MakeWaterball()
    {
        if (playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Waterball waterball = Instantiate(peluru, transform.position, Quaternion.identity).GetComponent<Waterball>();
            waterball.Setup(temp, ChooseWaterballDirection());
            playerInventory.ReduceMagic(waterball.magicCost);
            reduceMagic.Raise();
        }
    }

        Vector3 ChooseWaterballDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX"))* Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                interactSound.Play();
                animator.SetBool("receiveItem", true);
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receiveItem", false);
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
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
    public void Knock(float knockTime, float damage)
    {
        currentState = PlayerState.stagger;
        currentHealth.RuntimeValue -= damage; 
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            damagedSound.Play();
            StartCoroutine(KnockCo(knockTime));
        }else{
            // isDead = true;
            this.gameObject.SetActive(false);
            gameManager.gameOver();
        }
    }
    
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while(temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }
}
