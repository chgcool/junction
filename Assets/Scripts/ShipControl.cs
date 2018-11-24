using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private SpriteRenderer shipSpriteRenderer;
    private Vector3 initialPosition;
    private float shipHeight;

    private bool sailsUp;

    public float forceUpScale = 1f;
    public float forceRightScale = 1f;

	// Use this for initialization
	void Start ()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        shipSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        initialPosition = rb2D.transform.position;
        shipHeight = shipSpriteRenderer.sprite.bounds.size.y * shipSpriteRenderer.gameObject.transform.localScale.y;
        sailsUp = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // floating
        if (rb2D.transform.position.y < initialPosition.y - (shipHeight / 2))
        {
            Debug.Log("We're falling, captain!");
            rb2D.AddForce(forceUpScale * Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            sailsUp = !sailsUp;
        }

        if (sailsUp)
        {
            rb2D.AddForce(forceRightScale * Vector2.right);
        }
	}
}
