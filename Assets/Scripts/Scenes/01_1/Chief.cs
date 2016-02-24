using UnityEngine;
using System.Collections;

public class Chief : MonoBehaviour {

	public void Start () {
        Oscillate.Start(gameObject, Utils.Hash("prop", "scale", "a", new Vector3(0.1f, 0.1f, 0), "usedelta", true, "rndphase", true, "t", 5.0f));
    }  
}
