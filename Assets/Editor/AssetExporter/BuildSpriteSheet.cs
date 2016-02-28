using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using PlistCS;

class BuildSpriteSheet
{
    [MenuItem("Editor/Build SpriteSheet")]
    static void LoadSpriteSheet()
    {
        string[] filters = { "Plist Files", "plist" };
        string path = EditorUtility.OpenFilePanelWithFilters("Open Plist File", Global.kAssetsPath, filters);

        var plist = Plist.readPlist(path) as Dictionary<string, object>;
        var metadata = plist["metadata"] as Dictionary<string, object>;
        var frames = plist["frames"] as Dictionary<string, object>;

        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Global.kAssetsPath + metadata["textureFileName"]);

        if (!texture)
        {
            Debug.Log(string.Format("Cant load texture {0}", metadata["textureFileName"]));
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(texture);

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        importer.isReadable = true;

        List<SpriteMetaData> list = new List<SpriteMetaData>();

        foreach (var frame in frames)
        {
            var data = frame.Value as Dictionary<string, object>;
            SpriteMetaData meta = new SpriteMetaData();

            Rect bounds = Global.RectFromString(data["textureRect"] as string);
            bounds.y = texture.height - bounds.y - bounds.height;

            meta.pivot = new Vector2(0, 0);

            meta.alignment = (int)SpriteAlignment.Custom;
            meta.name = frame.Key;
            meta.rect = bounds;

            list.Add(meta);
        }

        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritesheet = list.ToArray();

        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }
}
