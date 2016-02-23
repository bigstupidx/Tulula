using UnityEngine;
using System.Collections;

public class Totem : MonoBehaviour {

	void Start () {
        Oscillate.Start(gameObject, Utils.Hash("prop", "position", "a", new Vector3(0, 5.0f / 100.0f, 0), "usedelta", true, "rndphase", true, "t", 3.0));
    }
}
