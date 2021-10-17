using UnityEngine;
using UnityEditor;


public class TexturePostProcessor : AssetPostprocessor
{
    private enum SpriteType
    {
        Dot,
        Spine,
    }
    void OnPostprocessTexture(Texture2D texture)
    {
        SpriteType spriteType = SpriteType.Dot;

        if (assetPath.StartsWith("Assets/Resources/Texture/"))
        {
            spriteType = SpriteType.Dot;
        }
        else if (assetPath.StartsWith("Assets/NotLoadResources/Spine/"))
        {
            spriteType = SpriteType.Spine;
        }
        else
        {
            return;
        }


        

        TextureImporter importer = assetImporter as TextureImporter;

        switch(spriteType)
        {
            case SpriteType.Dot :
                {
                    //bool isMonsterImage = (assetPath.StartsWith("Assets/NotLoadResources/Sprite/MonsterImage/") == true);
                    //bool isPixelImage = (assetPath.StartsWith("Assets/NotLoadResources/Sprite/InGame/PixelPlane/") == true);
                
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    //importer.spritePixelsPerUnit = isPixelImage ? 1.0f : 100.0f;

                    importer.alphaIsTransparency = true;
                    //importer.isReadable = isMonsterImage ? true : false;

                    importer.anisoLevel = 1;
                }
                break;

            case SpriteType.Spine :
                {
                    importer.textureType = TextureImporterType.Default;
                    importer.textureShape = TextureImporterShape.Texture2D;

                    importer.alphaIsTransparency = false;
                    importer.isReadable = false;

                    importer.anisoLevel = -1;
                }
                break;
        }
        
        importer.sRGBTexture = true;
        importer.alphaSource = TextureImporterAlphaSource.FromInput;

        importer.wrapMode = TextureWrapMode.Repeat;
        importer.filterMode = FilterMode.Point;
        importer.mipmapEnabled = false;

        importer.textureCompression = TextureImporterCompression.Uncompressed;


        Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
        if (asset)
        {
            EditorUtility.SetDirty(asset);
        }
    }
}