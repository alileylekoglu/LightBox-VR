using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class TextToImage : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public Button editButton;
    public RawImage rawImage;
    public XRRayInteractor leftHandRayInteractor;
    public XRRayInteractor rightHandRayInteractor;
    public Auth auth;
    public int eraseSize = 10;
    public Button saveButton;

    private const string Url = "https://api.stability.ai/v1/generation/stable-diffusion-xl-beta-v2-2-2/text-to-image";
    private Texture2D editableTexture;
    private bool isDrawing = false;

    [System.Serializable]
    private class TextPrompt
    {
        public string text;
        public int weight;
    }

    [System.Serializable]
    private class RequestBody
    {
        [Range(1, 1024)]
        public int width = 512;
        [Range(1, 1024)]
        public int height = 512;
        [Range(1, 100)]
        public int steps = 50;
        public int seed = 0;
        [Range(1, 10)]
        public int cfg_scale = 7;
        public int samples = 1;
        public string style_preset = "enhance";
        public TextPrompt[] text_prompts;
    }

    [System.Serializable]
    public class ResponseModel
    {
        public ArtifactModel[] artifacts;
    }

    [System.Serializable]
    public class ArtifactModel
    {
        public string base64;
        public int seed;
    }

    private void Start()
    {
        sendButton.interactable = false;
        editButton.interactable = false;
        sendButton.onClick.AddListener(() => StartCoroutine(PostRequest(inputField.text)));
        editButton.onClick.AddListener(ActivateEditing);
        inputField.onValueChanged.AddListener((value) => {
            bool hasInput = !string.IsNullOrEmpty(value);
            sendButton.interactable = hasInput && !isDrawing;
            editButton.interactable = hasInput && !isDrawing;
        });

        saveButton.onClick.AddListener(SaveImage);

        editableTexture = new Texture2D(512, 512);
        rawImage.texture = editableTexture;
    }

    private void Update()
    {
        if (isDrawing)
        {
            RaycastHit hit;
            if (RaycastHitWithCanvas(leftHandRayInteractor, out hit) || RaycastHitWithCanvas(rightHandRayInteractor, out hit))
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, hit.point, null, out localPoint);

                Vector2 normalizedLocalPoint = new Vector2(
                    (localPoint.x - rawImage.rectTransform.rect.min.x) / rawImage.rectTransform.rect.width,
                    (localPoint.y - rawImage.rectTransform.rect.min.y) / rawImage.rectTransform.rect.height
                );

                int x = Mathf.FloorToInt(normalizedLocalPoint.x * editableTexture.width);
                int y = Mathf.FloorToInt(normalizedLocalPoint.y * editableTexture.height);

                int radius = eraseSize / 2;
                for (int xi = Mathf.Max(0, x - radius); xi < Mathf.Min(editableTexture.width, x + radius); xi++)
                {
                    for (int yi = Mathf.Max(0, y - radius); yi < Mathf.Min(editableTexture.height, y + radius); yi++)
                    {
                        if ((new Vector2(xi, yi) - new Vector2(x, y)).sqrMagnitude <= radius * radius)
                        {
                            editableTexture.SetPixel(xi, yi, Color.clear);
                        }
                    }
                }

                editableTexture.Apply();
            }
        }
    }

    public void SaveImage()
    {
        byte[] bytes = editableTexture.EncodeToPNG();
        // Add the filename to the desired path.
        string path = Path.Combine(Application.dataPath, "Images", "SavedImage.png");
        // Check if the directory exists and if not, create it.
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved image to " + path);
        // Tell Unity that the file has changed.
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void ActivateEditing()
    {
        isDrawing = true;
        inputField.interactable = false;
    }

    private IEnumerator PostRequest(string prompt)
    {
        var body = new RequestBody
        {
            width = 512,
            height = 512,
            steps = 50,
            seed = 0,
            cfg_scale = 7,
            samples = 1,
            style_preset = "enhance",
            text_prompts = new[]
            {
                new TextPrompt
                {
                    text = prompt,
                    weight = 1
                }
            }
        };

        var requestBody = JsonUtility.ToJson(body);
        var request = new UnityWebRequest(Url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(requestBody)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + auth.apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<ResponseModel>(request.downloadHandler.text);
            var image = response.artifacts[0];
            var bytes = System.Convert.FromBase64String(image.base64);
            editableTexture = new Texture2D(2, 2);
            editableTexture.LoadImage(bytes);
            rawImage.texture = editableTexture;

            isDrawing = false;
            inputField.interactable = true;
            editButton.interactable = true;
        }
    }

    private bool RaycastHitWithCanvas(XRRayInteractor rayInteractor, out RaycastHit hit)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
            if (hit.collider.gameObject.CompareTag("RawImage"))
            {
                return true;
            }
        }

        return false;
    }

}
