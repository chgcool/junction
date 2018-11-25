using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform target;
    public float damping = 100;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float offsetZ;
    private Vector3 lastTargetPosition;
    private Vector3 currentVelocity;
    private Vector3 lookAheadPos;

    private Vector3 offset;

    private Vector3 cameraCenter = Vector3.zero;

    // Use this for initialization
    private void Start()
    {
        lastTargetPosition = target.position;
        offsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        /*
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - lastTargetPosition).x;

        if (Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold)
        {
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, cameraCenter, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = lookAheadPos + Vector3.forward * offsetZ;
        aheadTargetPos.x += target.position.x;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

        transform.position = newPos;
        lastTargetPosition = target.position;
        */

        float xMoveDelta = lastTargetPosition.x - target.position.x;
        lastTargetPosition = target.position;

        Vector3 aheadTargetPos = target.position;
        aheadTargetPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);


        //transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }

    public void VerticalShift(Vector3 shiftVector)
    {
        cameraCenter += shiftVector;
        lookAheadPos += shiftVector;
    }
}
