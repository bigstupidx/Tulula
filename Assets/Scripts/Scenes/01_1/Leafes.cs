using UnityEngine;
using System.Collections;

public class Leafes : MonoBehaviour {

	void Start () {
        var r = new System.Random();

        Oscillate.Start(gameObject, Utils.Hash("prop", "rotation", "a", new Vector3(0, 0, r.Next(-10, 10)), "usedelta", true, "rndphase", true, "t", Utils.RandRange(3.0, 5.0)));
    }
}
