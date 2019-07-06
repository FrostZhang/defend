using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeShow : MonoBehaviour {
    Animator animal;
    private void Awake()
    {
        animal = GetComponent<Animator>();
        animal.SetBool("idel", true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
