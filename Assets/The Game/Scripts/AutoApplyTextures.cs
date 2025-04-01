using System.IO;
using UnityEditor;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;
using File = UnityEngine.Windows.File;

public class AutoApplyTextures : MonoBehaviour
{
    public string texturesFolder; // Путь внутри "Assets"

    private void Start()
    {
        Renderer component = GetComponent<Renderer>();
        if (component == null)
            return;

        Material[] materials = component.sharedMaterials;
        string path = Path.Combine("Assets", texturesFolder);

        if (!Directory.Exists(path))
        {
            Debug.LogError("Папка с текстурами не найдена: " + path);
            return;
        }

        foreach (Material mat in materials)
        {
            string texturePath = FindTextureByName(path, mat.name);
            if (!string.IsNullOrEmpty(texturePath))
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                
                if (texture == null) 
                    continue;
                
                mat.mainTexture = texture;
                Debug.Log($"✅ Применена текстура {texture.name} к материалу {mat.name}");
            }
            else
            {
                Debug.LogWarning($"⚠ Текстура не найдена для материала {mat.name}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private string FindTextureByName(string folderPath, string materialName)
    {
        string[] validExtensions = {".png", ".jpg", ".jpeg", ".tga"};

        foreach (string ext in validExtensions)
        {
            string potentialPath = Path.Combine(folderPath, $"{materialName}{ext}");
            if (File.Exists(potentialPath))
                return potentialPath;
        }

        return null;
    }
}