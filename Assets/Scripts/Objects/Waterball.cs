using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterball : MonoBehaviour
{
    public float speed;
    public Rigidbody2D myRigidbody;
    public float lifetime;
    private float lifetimeCounter;
    public float magicCost;

    // Start is called before the first frame update
    void Start()
    {
        lifetimeCounter = lifetime;
    }

    private void Update()
    {
        lifetimeCounter -= Time.deltaTime;
        // Peluru akan otomatis menghilang dalam periode waktu input Lifetime
        if(lifetimeCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(Vector2 velocity, Vector3  direction)
    {
        // Fungsi agar peluru ditembakkan sesuai arah (tidak mencong)
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Peluru akan otomatis menghilang jika collide dengan Enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
