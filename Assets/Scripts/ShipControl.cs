﻿using System.Collections;
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
    public float floatingLimit = 2f;
    private bool forceUp = false;

	// Use this for initialization
	void Start ()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        shipSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        initialPosition = rb2D.transform.position;
        shipHeight = shipSpriteRenderer.sprite.bounds.size.y * shipSpriteRenderer.gameObject.transform.localScale.y;
        sailsUp = false;

        rb2D.transform.SetPositionAndRotation(initialPosition + (Vector3.up * shipHeight), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // floating
        Vector3 forceToApply;
        if (forceUp == false)
        {
            float forceScale = -1f * forceUpScale * Mathf.Abs((rb2D.transform.position.y - (initialPosition.y - shipHeight / floatingLimit)));
            forceToApply = Vector3.up * forceScale;
            //Debug.Log("Force up: " + forceUp);
            //Debug.Log(forceScale);
        }
        else
        {
            float forceScale = forceUpScale * Mathf.Abs((rb2D.transform.position.y - (initialPosition.y + shipHeight / floatingLimit)));
            forceToApply = Vector3.up * forceScale;
            //Debug.Log("Force up: " + forceUp);
            //Debug.Log(forceScale);
        }

        if (forceToApply.y < 0 && rb2D.transform.position.y < initialPosition.y - shipHeight / floatingLimit
            || forceToApply.y > 0 && rb2D.transform.position.y > initialPosition.y + shipHeight / floatingLimit)
        {
            forceUp = !forceUp;
        }

        rb2D.AddForce(forceToApply);

        // sailing
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
