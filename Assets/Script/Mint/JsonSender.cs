using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // Add this to access UI components

public class JsonSender : MonoBehaviour
{
    private const string URL = "http://localhost:3000/json-data";

    // Sample JSON payload
    private class SampleData
    {
        public string mintAddress;
    }

    public Button sendButton; // Reference to your button

    private void Start()
    {
        // Assign the SendData function to the button's onClick event
        sendButton.onClick.AddListener(OnSendButtonClick);
    }

    // Function that gets triggered when the button is clicked
    private void OnSendButtonClick()
    {
        // Create sample data
        SampleData data = new SampleData { mintAddress = "HRupGSLGgbTjuABZZdcNaFeLLmQywubmk3bVCCPBfQqj" };

        // Send data
        StartCoroutine(SendData(data));
    }

    private IEnumerator SendData(SampleData data)
    {
        // Convert data to JSON
        string json = JsonUtility.ToJson(data);

        // Create a POST request with JSON payload
        UnityWebRequest www = new UnityWebRequest(URL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Handle the result
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Successfully sent JSON! Response: " + www.downloadHandler.text);
        }
    }
}
