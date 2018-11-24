﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Vector3 centerPosition;
    private float shipHeight;

    private bool sailsUp;

    public SpriteRenderer shipSpriteRenderer;
    public CameraScript cameraScript;
    public GameObject frontWaveParent;
    public Text gameOverText;

    public float forceVerticalScale = 1f;
    public float forceHorizontalScale = 1f;
    public float forceJumpScale = 10f;
    public float floatingLimit = 2f;

    private bool forceUp = false;
    private bool flipped = false;
    private bool gameOver = false;

	// Use this for initialization
	void Start ()
    {
        gameOverText.gameObject.SetActive(false);

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        centerPosition = rb2D.transform.position;
        shipHeight = shipSpriteRenderer.sprite.bounds.size.y * shipSpriteRenderer.gameObject.transform.localScale.y;
        sailsUp = false;

        // initial floating position
        rb2D.transform.SetPositionAndRotation(centerPosition + (Vector3.up * shipHeight / floatingLimit), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Floating();

        if (gameOver == false)
        {
            Sailing();
            Flip();

            // jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float factor = flipped ? -1f : 1f;
                rb2D.AddForce(Vector2.up * forceJumpScale * factor, ForceMode2D.Impulse);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision happened!");

        gameOver = true;
        gameOverText.gameObject.SetActive(true);
    }

    private void Floating()
    {
        Vector3 forceToApply;
        if (forceUp == false)
        {
            float forceScale = -1f * forceVerticalScale * Mathf.Abs((rb2D.transform.position.y - (centerPosition.y - shipHeight / floatingLimit)));
            forceToApply = Vector3.up * forceScale;
            //Debug.Log("Force up: " + forceUp);
            //Debug.Log(forceScale);
        }
        else
        {
            float forceScale = forceVerticalScale * Mathf.Abs((rb2D.transform.position.y - (centerPosition.y + shipHeight / floatingLimit)));
            forceToApply = Vector3.up * forceScale;
            //Debug.Log("Force up: " + forceUp);
            //Debug.Log(forceScale);
        }

        if (forceToApply.y < 0 && rb2D.transform.position.y < centerPosition.y - shipHeight / floatingLimit
            || forceToApply.y > 0 && rb2D.transform.position.y > centerPosition.y + shipHeight / floatingLimit)
        {
            forceUp = !forceUp;
        }

        rb2D.AddForce(forceToApply);
    }

    private void Sailing()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            sailsUp = !sailsUp;
        }

        if (sailsUp)
        {
            rb2D.AddForce(forceHorizontalScale * Vector2.left);
        }
    }

    void Flip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 newScale = rb2D.transform.localScale;
            newScale.y = -newScale.y;
            rb2D.transform.localScale = newScale;

            Vector3 verticalShift;
            Color frontWaveNewColor;
            SpriteRenderer[] frontWaveChildrenSpriteRenderers = frontWaveParent.GetComponentsInChildren<SpriteRenderer>();

            if (flipped)
            {
                verticalShift = (Vector3.up * shipHeight);
                frontWaveNewColor = frontWaveChildrenSpriteRenderers[0].color;
                frontWaveNewColor.a = 1f;
            
            }
            else
            {
                verticalShift = (Vector3.down * shipHeight);
                frontWaveNewColor = frontWaveChildrenSpriteRenderers[0].color;
                frontWaveNewColor.a = 0.3f;
            }

            rb2D.transform.position += verticalShift;
            centerPosition += verticalShift;

            foreach (var childSpriteRenderer in frontWaveChildrenSpriteRenderers)
            {
                childSpriteRenderer.color = frontWaveNewColor;
            }

            cameraScript.VerticalShift(verticalShift);

            flipped = !flipped;
        }
    }
}
