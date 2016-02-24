using UnityEngine;
using System.Collections;

public class InventController : MonoBehaviour {
    [SerializeField]
    string _invent;

    public void Start ()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();
        var obj = Utils.GetObjectByName(objs, _invent);

        if(obj)
        {
            var controller = gameObject.GetComponent<GameObjectController>();

            if(controller)
            {
                controller.invent = obj;
            }
        }
    }

    public string invent
    {
        get { return _invent; }
        set { _invent = value; }
    }
}