using UnityEngine;
using System.Collections;

public class InventController : MonoBehaviour {
    [SerializeField]
    private string          _invent;

    GameObject              _invetnObj = null;

    public void Init(string invent) 
    {
        _invent = invent;
    }

    public void Start ()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();

        _invetnObj = Utils.GetObjectByName(objs, _invent);

        if(_invetnObj)
        {
            var oc = _invetnObj.GetComponent<OpacityController>();

            if(oc)
            {
                oc.alpha = 0;
            }
        }

        var tc = gameObject.AddComponent<TouchController>();
    }

    public void Update ()
    {
	
	}
}
