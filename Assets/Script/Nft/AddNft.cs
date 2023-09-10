using UnityEngine;
using System.IO;

public class AddNft : MonoBehaviour
{
    public string imagePath; // This is the relative path of the image from the Assets folder.

    void Start()
    {
        LoadImage();
    }

    void LoadImage()
    {
        // Combine the data path with the image path to get the full path
        string fullPath = Path.Combine(Application.dataPath, imagePath);
        if (File.Exists(fullPath))
        {
            // Load the image into a byte array
            byte[] imageData = File.ReadAllBytes(fullPath);

            // Create a new Texture2D
            Texture2D texture = new Texture2D(2, 2);

            // Load the image data into the texture
            if (texture.LoadImage(imageData))
            {
                // If the data loads successfully, apply the texture to the renderer
                GetComponent<Renderer>().material.mainTexture = texture;
            }
            else
            {
                Debug.LogError("Could not load image as texture");
            }
        }
        else
        {
            Debug.LogError("Image file not found at path: " + fullPath);
        }
    }
}
