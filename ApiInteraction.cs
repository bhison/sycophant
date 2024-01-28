using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class APIInteraction : MonoBehaviour
{
    public bool isFetching = false;
    
    private JudgeForLaughs judge;

    public void SetJudge(JudgeForLaughs judge)
    {
        this.judge = judge;
    }
    public IEnumerator FetchLaughRating(byte[] audioBytes)
    {
        var url = "http://test.lumi.systems:40500/laughter"; // API endpoint goes here.

        WWWForm form = new WWWForm();
        // Add binary data as a field that can be posted to a server
        form.AddBinaryData("file", audioBytes, "audio.wav", "audio/wave");

        using UnityWebRequest www = UnityWebRequest.Post(url, form);
        {
            www.uploadHandler.contentType = "multipart/form-data";

            Debug.Log("Request Details:");
            Debug.Log("URL: " + www.url);
            Debug.Log("Method: " + www.method);
            if (www.uploadHandler != null)
            {
                Debug.Log("Content-Type: " + www.uploadHandler.contentType);
                Debug.Log("Body Data: " + System.Text.Encoding.UTF8.GetString(www.uploadHandler.data));
            }
            else
            {
                Debug.Log("Upload handler null");
            }

            isFetching = true;
            UnityWebRequestAsyncOperation requestOperation = www.SendWebRequest();

            yield return requestOperation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: Fail");
                Debug.Log(www.error);
                isFetching = false;
            }
            else
            {
                Debug.Log("Response: Success");
                Debug.Log("Response Content: " + www.downloadHandler.text);
                var value = float.Parse(www.downloadHandler.text);
                judge.LaughFactor = value;
                isFetching = false;
            }
        }
    }
}
