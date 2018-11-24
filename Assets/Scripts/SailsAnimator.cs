using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailsAnimator : MonoBehaviour {

    public Animator animator;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () { 
        animator.SetBool("ButtonPushed", Input.GetKeyDown(KeyCode.S));     
    }
}
