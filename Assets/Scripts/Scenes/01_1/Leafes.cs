using UnityEngine;
using System.Collections;

public class Leafes : MonoBehaviour {

	void Start () {
        Oscillate.Start(gameObject, Utils.Hash("prop", "rotation", "a", new Vector3(0, 0, Random.Range(-10.0f, 10.0f)), "usedelta", true, "rndphase", true, "t", Random.Range(5.0f, 7.0f)));
    }
}
