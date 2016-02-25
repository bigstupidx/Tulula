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

    public bool isTouchEnabled
    {
        get { return _touchEnabled; }
        set { _touchEnabled = value; }
    }

    public bool isIgnoreAlpha
    {
        get { return _ignoreAlpha; }
        set { _ignoreAlpha = value; }
    }

    public bool isAviableForTouch
    {
        get { return isTouchEnabled && (alpha > 0 || isIgnoreAlpha); }
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

