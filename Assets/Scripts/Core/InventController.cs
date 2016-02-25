using UnityEngine;
using System.Collections;

public class InventController : MonoBehaviour {
    [SerializeField]
    string _invent;

    GameObject _objInvent = null;

    public void Start ()
    {
        //var objs = gameobject.findobjectsoftype<gameobject>();

        //gameobject obj = utils.getobjectbyname(objs, _invent);

        //if (obj)
        //{
        //    var controller = obj.getcomponent<gameobjectcontroller>();

        //    if (controller)
        //    {
        //        controller.alpha = 0;
        //    }

        //    controller = gameobject.getcomponent<gameobjectcontroller>();

        //    if(controller)
        //    {
        //        controller.istouchenabled = true;
        //    }

        //    _objinvent = obj;
        //}
    }

    public string invent
    {
        get { return _invent; }
        set { _invent = value; }
    }

    public void Invent()
    {
        //if(_objInvent)
        //{
        //    var controller = _objInvent.GetComponent<GameObjectController>();

        //    if (controller)
        //    {
        //        controller.alpha = 1;
        //    }

        //    controller = gameObject.GetComponent<GameObjectController>();

        //    if (controller)
        //    {
        //        controller.alpha = 0;
        //    }

        //    _objInvent.transform.position = controller.center;
        //}
    }
}