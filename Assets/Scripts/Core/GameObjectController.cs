using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public delegate void GameObjectEvent(GameObject obj);

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

    public event GameObjectEvent onTouchBegan;
    public event GameObjectEvent onTouchEnded;

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
}

