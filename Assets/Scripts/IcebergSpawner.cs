using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcebergSpawner : MonoBehaviour
{
    enum Direction
    {
        Left = -1,
        Right = 1
    }

    public bool hasLeftBuddy = false;
    public bool hasRightBuddy = false;

    private float cameraHorizontalExtend;
    private float spriteWidth;
    private System.Random rand;

    public double minY = -0.4;
    public double maxY = -0.2;
    public double scalingX = 1;
    public float minSpacing = 10;

    // Use this for initialization
    void Start()
    {
        cameraHorizontalExtend = Camera.main.orthographicSize * Screen.width / Screen.height;

        SpriteRenderer sRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x * System.Math.Abs(transform.localScale.x);

        //Debug.Log(spriteWidth);

        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLeftBuddy && transform.position.x > Camera.main.transform.position.x - cameraHorizontalExtend)
        {
            // Debug.Log("About to add an iceberg!");
            AddIceberg(Direction.Left);
            hasLeftBuddy = true;
        }

        if (IsInvisibleOnTheRight())
        {
            Destroy(gameObject);
        }
    }

    private void AddIceberg(Direction direction)
    {
        GameObject newIceberg = Instantiate(gameObject);
        newIceberg.transform.parent = transform.parent;

        float offsetX = (float)(minSpacing + rand.NextDouble() * scalingX);

        double newY = minY + rand.NextDouble() * (maxY - minY);
        // Debug.Log(newY);
        // Debug.Log(offsetX);

        newIceberg.transform.position = new Vector3(transform.position.x + offsetX * (int)direction, (float)(0.3 + newY), transform.position.z);

        if (direction == Direction.Left)
        {
            newIceberg.GetComponent<IcebergSpawner>().hasRightBuddy = true;
        }
        else if (direction == Direction.Right)
        {
            newIceberg.GetComponent<IcebergSpawner>().hasLeftBuddy = true;
        }
        
    }

    private bool IsInvisibleOnTheRight()
    {
        return (transform.position.x - spriteWidth > Camera.main.transform.position.x + 2*cameraHorizontalExtend);
    }
}
