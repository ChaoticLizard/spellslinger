using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour
{
    public GameObject tether; // Object which this one is tethered to, preventing movement too far from it

    public float minDist; // Minimum distance the object can be from its tether
    public float maxDist; // Maximum distance the object can be from its tether

    public float easeRate = 0.0f; // Used to prevent the cursor from moving too quickly

    // Update is called once per frame
    void Update()
    {
        // Get Mouse Position
        Vector3 mousePosition = Input.mousePosition;           
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        if (tether != null)
        {
            float tetherX = tether.transform.position.x;
            float tetherY = tether.transform.position.y;
            
            Vector2 mouseVect = new Vector2(mousePosition.x - tetherX, mousePosition.y - tetherY);

            if (maxDist >= minDist)
            {
                mouseVect = mouseVect.normalized * Mathf.Clamp(mouseVect.magnitude, minDist, maxDist);
            }
            else
            {
                mouseVect = mouseVect.normalized * Mathf.Max(mouseVect.magnitude, minDist);
            }
            
            mousePosition.x = mouseVect.x + tetherX;
            mousePosition.y = mouseVect.y + tetherY;
        }
        
        mousePosition.z = transform.position.z; // Stay at the same Z value

        // Go to the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, Time.deltaTime / easeRate);
    }
}
