using UnityEngine;
using System.Collections;

public class PrimaryFire : MonoBehaviour
{
    /***********************************************************************
     * This script is used to fire projectiles that have a degree of power *
     * based on the length of time the left mouse button has been pressed. *
     ***********************************************************************/
    
    public GameObject spriteObj; // The player object from which to fire bolts from
    private SpriteRenderer sprite; // Used to change the player object's sprite for feedback purposes
    private Color defaultColor; // Used internally to keep track of the character's normal color
    private float buttonTimer = 0.0f; // Used to track the length of time a button has been held for
    
    public GameObject target; // Reference to the cursor object to point and shoot at
    
    public bool canFire = true; // Whether or not you can currently fire
    
    public float[] pressTime; // All the different lengths of time to check for when determining the power of the bolt fired
    public Color[] pressColor; // Used to test the pressTime array and distinguish the power of the bolt to fire
    public GameObject[] pressBolt; // Which prefab bolt to fire, based on the length of the press
    
    private float offsetAngle = 90.0f; // Used to spawn the bolts facing the right direction
    
    public float[] boltSpeed; // How fast the bolts move when shot
    public bool additiveVel = false; // Whether or not to add the player's current velocity to the bolt's velocity
    public float[] recoilFactor; // How much recoil the shooter experiences when firing a given bolt
    
    public float projLife = 1.0f; // How long the bolt remains on-screen before imploding
    
    // Initializes various variables
    void Start()
    {
        // Get the player's sprite and default color for later modification and reference
        sprite = spriteObj.GetComponent<SpriteRenderer>();
        defaultColor = sprite.color;
    }
    
    // Call the per-frame behavior each frame
    void Update()
    {
        // While the player holds down the button,
        if (Input.GetButton("LMB") && canFire)
        {
            buttonTimer += Time.deltaTime; // update the timer
            
            // If the current length of the button press matches a length in the array,
            for (int i = 0; i < pressTime.Length; i++)
            {
                if (buttonTimer >= pressTime[i])
                {
                    // Update the player's color to indicate their current firing power
                    sprite.color = pressColor[i];
                }
            }
        }
        // Once the player releases the button,
        else if (Input.GetButtonUp("LMB") && canFire)
        {
            // Return the length of time the button was pressed for
            //Debug.Log("LMB Held for: " + buttonTimer + "s");
            
            sprite.color = defaultColor; // Reset the player's color to normal
            
            FireBolt(); // Call the behavior for actually firing the bolts
            
            buttonTimer = 0.0f; // Reset the timer to check the length of button presses again
        }
    }
    
    // The actual behavior used to fire the projectiles
    void FireBolt()
    {
        // Determine the power of the bolt to be fired
        int i, j;
        for (i = 0, j = 0; i < pressTime.Length; i++)
        {
            // If the button was pressed for at least this length of time
            if (buttonTimer >= pressTime[i])
            {
                j = i; // This is the current power of the bolt
            }
        }
        
        // Find the vector between the character and the cursor
        Vector3 dirVect = target.transform.position - transform.position;
        dirVect.z = 0.0f;
        
        // Spawn the projectile in the right place
        Vector3 boltPos = transform.position;
        
        // Spawn the projectile going the right direction
        Quaternion angle = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dirVect.y, dirVect.x) * Mathf.Rad2Deg - offsetAngle));
        
        // Instantiate the proper bolt object
        GameObject theObject = Instantiate(pressBolt[j], boltPos, angle) as GameObject;
        
        // Find the bullet's rigidbody2D for modification
        Rigidbody2D rigid = theObject.GetComponent<Rigidbody2D>();
        
        if (rigid != null) // Just make sure you're not referencing a null object
        {
            // Make the bullet move
            rigid.velocity = theObject.transform.up * boltSpeed[j];
            
            // If using additive velocity, add the shooter's current velocity to the projectile's
            Vector2 newVel = additiveVel ? gameObject.GetComponent<Rigidbody2D>().velocity : Vector2.zero;
            
            // Apply recoil to the player
            gameObject.GetComponent<Rigidbody2D>().velocity -= rigid.velocity.normalized * recoilFactor[j];
            
            // Apply additive velocity
            rigid.velocity += newVel;
        }
        
        // Find the bullet's destroyer script to modify its lifetime
        AutoDestroyer auto = theObject.GetComponent<AutoDestroyer>();
        
        if (auto != null) // Just make sure you're not referencing a null object
        {
            auto.lifeLimit = projLife; // Set the bullet's lifetime
        }
    }
}
