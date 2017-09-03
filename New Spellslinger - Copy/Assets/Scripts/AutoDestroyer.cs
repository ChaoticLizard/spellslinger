using UnityEngine;
using System.Collections;

public class AutoDestroyer : MonoBehaviour
{
    private float lifeTimer = 0.0f; // Used internally to check when to automatically destroy
    public float lifeLimit = 1.0f; // How long this object lives for, in seconds
    public bool dieOnCollision = true; // Whether to self-destruct on collision with another object

    //public bool destroyOnCollision = false; // Whether to automatically destroy on collision with an object

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime; // Increment the life timer

        if (lifeTimer >= lifeLimit) // If your life is over
        {
            Destroy(gameObject); // Destroy yourself, Mortal!!!!
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (dieOnCollision)
        {
            Destroy(gameObject); // Destroy yourself, Mortal!!!!
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (dieOnCollision)
        {
            Destroy(gameObject); // Destroy yourself, Mortal!!!!
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (dieOnCollision)
        {
            Destroy(gameObject); // Destroy yourself, Mortal!!!!
        }
    }
}
