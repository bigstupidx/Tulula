using UnityEngine;
using UnityEditor;

using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;


public class LoadScene
{
    [MenuItem("Editor/Load Scene Objects")]
    static void Load()
    {
        string[] filters = { "XML Files", "xml" };
        string path = EditorUtility.OpenFilePanelWithFilters("Open XML File", Global.kSourcesPath, filters);

        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNode root = doc.DocumentElement.SelectSingleNode("/scene");

        string images = Global.kAssetsPath + root.Attributes["images"].InnerText;

        GameObject sceneRoot = new GameObject("Scene");

        var objects = loadObjectsFromXML(root.SelectSingleNode("objects"), images);

        foreach(var obj in objects)
        {
            if(obj.transform.parent == null)
            {
                obj.transform.parent = sceneRoot.transform;
            }
        }

//            sceneRoot.AddComponent<SceneController>();
    }

    public static GameObject[] loadObjectsFromXML(XmlNode root, string images)
    {
        List<GameObject> objects = new List<GameObject>();

        Dictionary<string, string> parentCfg = new Dictionary<string, string>();

        foreach (XmlNode node in root)
        {
            GameObject obj = new GameObject();

            Vector2 position = new Vector2();
            Vector2 size = new Vector2(10, 10);

            string name = "";
            bool sizeFromTexture = true;

            if (node.Attributes["position"] != null)
            {
                position = Global.VectorFromString(node.Attributes["position"].InnerText, ';');
            }

            if (node.Attributes["size"] != null)
            {
                size = Global.VectorFromString(node.Attributes["size"].InnerText, ';');
                sizeFromTexture = false;
            }

            if (node.Attributes["name"] != null)
            {
                name = node.Attributes["name"].InnerText;
            }

            if (node.Attributes["texture"] != null)
            {
                string texture = node.Attributes["texture"].InnerText;
                string texturePath = images + texture + ".png";

                Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

                if (texture2D != null)
                {
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();

                    if (node.Name == "animation")
                    {
                        EditorCurveBinding curveBinding = new EditorCurveBinding();
                        curveBinding.type = typeof(SpriteRenderer);
                        curveBinding.propertyName = "m_Sprite";

                        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();

                        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[sprites.Length];

                        int i = 0;
                        int fps = 5;

                        foreach (var frame in sprites)
                        {
                            var kf = new ObjectReferenceKeyframe();

                            kf.time = i * (1.0f / fps);
                            kf.value = frame;
                            keyFrames[i++] = kf;

                            if (renderer.sprite == null)
                            {
                                renderer.sprite = frame;

                                if(sizeFromTexture)
                                {
                                    size = frame.bounds.size;
                                }
                            }
                        }

                        AnimationClip clip = new AnimationClip();

                        clip.wrapMode = WrapMode.Loop;
                        clip.frameRate = fps;

                        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);

                        settings.loopTime = true;

                        AnimationUtility.SetAnimationClipSettings(clip, settings);

                        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
                        AssetDatabase.CreateAsset(clip, images + texture + ".anim");

                        var ac = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(images + texture + ".controller");
                        ac.AddMotion(clip);

                        Animator animator = obj.AddComponent<Animator>();
                        animator.runtimeAnimatorController = ac;
                    }
                    else
                    {
                        Vector2 pivot = new Vector2(0, 0);

                        if (node.Attributes["pivot"] != null)
                        {
                            pivot = Global.VectorFromString(node.Attributes["pivot"].InnerText, ';');
                            pivot.y = texture2D.height - pivot.y;
                        }

                        if (node.Attributes["pivotx"] != null)
                        {
                            string pivotx = node.Attributes["pivotx"].InnerText;

                            switch (pivotx)
                            {
                                case "left": pivot.x = 0; break;
                                case "center": pivot.x = texture2D.width / 2; break;
                                case "right": pivot.x = texture2D.width; break;
                            }
                        }

                        if (node.Attributes["pivoty"] != null)
                        {
                            string pivoty = node.Attributes["pivoty"].InnerText;

                            switch (pivoty)
                            {
                                case "top": pivot.y = texture2D.height; break;
                                case "center": pivot.y = texture2D.height / 2; break;
                                case "bottom": pivot.y = 0; break;
                            }
                        }

                        position += pivot;

                        pivot.x /= texture2D.width;
                        pivot.y /= texture2D.height;

                        Rect bounds = new Rect(0, 0, texture2D.width, texture2D.height);

                        renderer.sprite = Sprite.Create(texture2D, bounds, pivot);

                        if(sizeFromTexture)
                        {
                            size = bounds.size;
                        }
                    }

                    if (name.Length == 0)
                    {
                        name = texture;
                    }
                }
            }

            // var controller = obj.AddComponent<GameObjectController>();

            if (node.Attributes["alpha"] != null)
            {
                float alpha = float.Parse(node.Attributes["alpha"].InnerText);

                // controller.alpha = alpha;
            }

            if(node.Attributes["touched"] != null)
            {
                bool touched = bool.Parse(node.Attributes["touched"].InnerText);

                // controller.isTouchEnabled = touched;
            }

            if(node.Attributes["ignorealpha"] != null)
            {
                bool ignore = bool.Parse(node.Attributes["ignorealpha"].InnerText);

                //  controller.isIgnoreAlpha = ignore;
            }

            if (node.Attributes["parent"] != null)
            {
                parentCfg.Add(name, node.Attributes["parent"].InnerText);
            }

            if (node.Attributes["script"] != null)
            {
                var script = node.Attributes["script"].InnerText + ", " + "Assembly-CSharp";
                obj.AddComponent(Type.GetType(script));
            }

            if (node.Attributes["invent"] != null)
            {
                var inv = obj.AddComponent<InventController>();
                inv.invent = node.Attributes["invent"].InnerText;
            }

            if (node.Name == "compound")
            {
                string broken = node.Attributes["open"].InnerText;
                string full = node.Attributes["full"].InnerText;
                string config = node.Attributes["objects"].InnerText;

                var cc = obj.AddComponent<CompoundController>();
                cc.Init(broken, full, config);
            }

            var collider = obj.AddComponent<BoxCollider2D>();
            collider.size = size / Global.kPixelsPerUnit;

            position -= (Global.kDefaultWinSize / 2);

            obj.name = name;
            obj.transform.position = new Vector3(position.x / Global.kPixelsPerUnit, position.y / Global.kPixelsPerUnit, 0);

            objects.Add(obj);
        }

        foreach (var pair in parentCfg)
        {
            GameObject obj = Utils.GetObjectByName(objects.ToArray(), pair.Key);
            GameObject parent = Utils.GetObjectByName(objects.ToArray(), pair.Value);

            if (parent)
            {
                obj.transform.parent = parent.transform;
            }
        }

        foreach (GameObject obj in objects)
        {
            // var controller = obj.GetComponent<GameObjectController>();
            //controller.Update();
        }

        return objects.ToArray();
    }

    [MenuItem("Editor/Load Global Objects")]
    static void LoadGlobals()
    {
        string[] filters = { "XML Files", "xml" };
        string path = EditorUtility.OpenFilePanelWithFilters("Open XML File", Global.kSourcesPath, filters);

        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNode root = doc.DocumentElement.SelectSingleNode("/level");

        loadObjectsFromXML(root.SelectSingleNode("inventory"), Global.kInventoryPath);
    }

    [MenuItem("Editor/Reorder Scene Objects")]
    static void ReorderSceneObjects()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();

        int z = 0;

        foreach(var obj in objs)
        {
            Vector3 position = obj.transform.position;

            position.z = z++;
            obj.transform.position = position;
        }
    }

    [MenuItem("Editor/Setup camera")]
    static void SetupCamera()
    {
        var camera = Camera.main;

        camera.orthographic = true;
        camera.orthographicSize = Global.kDefaultWinSize.y / 2 / 100;
        camera.transform.position = new Vector3(0, 0, -100);
    }
}




