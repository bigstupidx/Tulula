using UnityEngine;
using System.Collections;

public class InventController : MonoBehaviour {
    [SerializeField]
    string _invent;

    GameObject _obj = null;

    public void Init(string invent) 
    {
        _invent = invent;
    }

    public void Start ()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();

        _obj = Utils.GetObjectByName(objs, _invent);

        if(_obj)
        {
            var oc = _obj.GetComponent<GameObjectController>();

            if(oc)
            {
                oc.alpha = 0;
            }
        }
    }
}