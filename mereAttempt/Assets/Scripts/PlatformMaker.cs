using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlatformMaker : MonoBehaviour {

    public bool hasLeftBuddy = false;
    public bool hasRightBuddy = false;

    private float cameraHorizontalExtend;

    public double minY = -0.4;
    public double maxY = -0.2;
    public double scalingX = 1;

    // Use this for initialization
    void Start () {
        cameraHorizontalExtend = Camera.main.orthographicSize * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
	    if (!hasRightBuddy && transform.position.x < Camera.main.transform.position.x + cameraHorizontalExtend) {
            addPlatform(1);
            hasRightBuddy = true;
        } else if (!hasLeftBuddy && transform.position.x > Camera.main.transform.position.x - cameraHorizontalExtend)
        {
            addPlatform(-1);
            hasLeftBuddy = true;
        }
	}

    /// <summary>
    /// adds platforms
    /// </summary>
    /// <param name="leftOrRight"> adding left or right platform </param>
    private void addPlatform(int leftOrRight)
    {
        Transform newPlatform = Instantiate(transform);
        newPlatform.parent = transform.parent;

        System.Random rand = new System.Random();

        float offsetX = (float) (0.3 + rand.NextDouble() * scalingX);

        double newY = minY + rand.NextDouble() * (maxY - minY);
        Debug.Log(newY);

        newPlatform.position = new Vector3(transform.position.x + offsetX * leftOrRight, (float)(0.3 + newY), transform.position.z);

        if (leftOrRight == 1)
        {
            newPlatform.GetComponent<PlatformMaker>().hasLeftBuddy = true;
        }
        else
        {
            newPlatform.GetComponent<PlatformMaker>().hasRightBuddy = true;
        }
    }
}
