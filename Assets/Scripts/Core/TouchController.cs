using UnityEngine;
using System.Collections;
using System;

public class TouchController : MonoBehaviour {

    delegate void OnTouchBegan(object sender, EventArgs e);
    delegate void OnTouchMoved(object sender, EventArgs e);
    delegate void OnTouchEnded(object sender, EventArgs e);

    void Start ()
    {
	
	}
	
	void Update ()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Debug.Log("hui123");
            }
        }
    }
}
