using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public delegate void GameObjectHandler(GameObject obj);
public delegate void GameObjectInteractHandler(GameObject obj, GameObject interact);

public class GameObjectController : MonoBehaviour
{
    [SerializeField]
    private float _alpha = 1.0f;
    [SerializeField]
    private bool _touchEnabled = false;
    [SerializeField]
    private bool _ignoreAlpha = false;
    [SerializeField]
    private string _invent;

    public event GameObjectHandler onTouchBegan;
    public event GameObjectHandler onTouchEnded;

    public event GameObjectInteractHandler onTouchMovedWithObject;
    public event GameObjectInteractHandler onTouchEndedWithObject;

    public void Start()
    {

    }

    public void Update()
    {
        UpdateTransparency();
    }

    void UpdateTransparency()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        
        if (renderer != null)
        {
            float alpha = _alpha;
            var parent = gameObject.transform.parent;

            if (parent != null)
            {
                var parentRenderer = parent.GetComponent<SpriteRenderer>();

                if (parentRenderer != null)
                {
                    alpha *= parentRenderer.color.a;
                }
            }

            Color color = renderer.color;
            color.a = alpha;

            renderer.color = color;
        }
    }

    public float alpha
    {
        get{
            return _alpha;
        }

        set{
            _alpha = value;
            UpdateTransparency();
        }
    }

    public float alphaReal
    {
        get
        {
            var renderer = gameObject.GetComponent<SpriteRenderer>();
            if(renderer)
            {
                return renderer.color.a;
            }
            return alpha;
        }
    }

    public Vector3 center
    {
        get {
            var collider = gameObject.GetComponent<Collider2D>();
            var center = gameObject.transform.position;

            if (collider)
            {
                center += collider.bounds.size / 2;
            }

            return center;
        }
    }

    public bool touchEnabled
    {
        get { return _touchEnabled; }
        set { _touchEnabled = value; }
    }

    public bool ignoreAlpha
    {
        get { return _ignoreAlpha; }
        set { _ignoreAlpha = value; }
    }

    public bool aviableForTouch
    {
        get { return touchEnabled && (alphaReal > 0 || ignoreAlpha); }
    }

    public string invent
    {
        get { return _invent; }
        set { _invent = value; }
    }
    
    public bool hasInventoryPair
    {
        get { return _invent.Length > 0; }
    }

    public void OnTouchBegan()
    {
        if(onTouchBegan != null)
        {
            onTouchBegan(gameObject);
        }
    }

    public void OnTouchEnded()
    {
        if(onTouchEnded != null)
        {
            onTouchEnded(gameObject);
        }
    }

    public void OnTouchEndedWithObject(GameObject obj)
    {
        if(onTouchEndedWithObject != null)
        {
            onTouchEndedWithObject(gameObject, obj);
        }
    }

    public void OnTouchMovedWithObject(GameObject obj)
    {
        if(onTouchMovedWithObject != null)
        {
            onTouchMovedWithObject(gameObject, obj);
        }
    }

    public bool IsIntersect(GameObject obj)
    {
        var collider1 = gameObject.GetComponent<BoxCollider2D>();
        var collider2 = obj.GetComponent<BoxCollider2D>();

        if(collider1 && collider2)
        {
            var bounds1 = new Rect(transform.position, collider1.bounds.size);
            var bounds2 = new Rect(obj.transform.position, collider2.bounds.size);

            return bounds1.Overlaps(bounds2);
        }
        return false;
    }
}

