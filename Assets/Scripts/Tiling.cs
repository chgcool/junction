using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour
{

    public bool reverseScale = true;
    private bool hasLeftBuddy = false;
    private bool hasRightBuddy = false;

    public bool allowOverlapping = true;

    private float cameraHorizontalExtend;
    private float spriteWidth;

    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x * System.Math.Abs(transform.localScale.x);

        // orthographicSize is half of height of camera. Thus, horizontal extend can be calculated
        cameraHorizontalExtend = Camera.main.orthographicSize * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
        if (hasRightBuddy == false && transform.position.x + spriteWidth/2 < Camera.main.transform.position.x + cameraHorizontalExtend)
        {
            makeNewBuddy(1);
            hasRightBuddy = true;
        } else if (hasLeftBuddy == false && transform.position.x - spriteWidth/2 > Camera.main.transform.position.x - cameraHorizontalExtend)
        {
            makeNewBuddy(-1);
            hasLeftBuddy = true;
        }
        if (invisible())
        {
            Destroy(gameObject);
        }
	}

    private bool invisible()
    {
        return (transform.position.x - spriteWidth > Camera.main.transform.position.x + cameraHorizontalExtend ||
                transform.position.x + spriteWidth < Camera.main.transform.position.x - cameraHorizontalExtend);
    }

    private void makeNewBuddy(int leftOrRight)
    {
        Transform newBuddy = Instantiate(transform);
        newBuddy.parent = transform.parent;

        newBuddy.position = new Vector3(transform.position.x + spriteWidth * leftOrRight - leftOrRight * (allowOverlapping ? 1 : 0.1f), transform.position.y, transform.position.z);

        // scaling for smooth connection of tiles
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        // set buddy parameters of new object
        if (leftOrRight == 1)
        {
            newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
        }
    }
}
