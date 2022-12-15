using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling.Memory.Experimental;

public class FunctionCalls : MonoBehaviour
{
    public FunctionCalls()
    { 
    }
    // Start is called before the first frame update
    void Start()
    {
        // CallLogin("user2"); Pass


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CallLogin(string userName, Action<string> callback)
    {
        StartCoroutine(PostJsonData<UserNameRequestMessage, string>(
            "https://web3funcs.azurewebsites.net/api/web3funclogin",
            new UserNameRequestMessage { username = userName },
            callback
            ));


    }
    public void CallGetCollection(string userName, Action<NFTMetadataOwnerArrayEnvelop> callback)
    {
        StartCoroutine(PostJsonData<UserNameRequestMessage, NFTMetadataOwnerArrayEnvelop>("https://web3funcs.azurewebsites.net/api/web3funclistnfts",
            new UserNameRequestMessage { username = userName },
            callback
            ));


    }
    public void CallUploadSnapshot(string userName, string nftName, string nftDesc, string fileName, string fileBase64Content, Action<NFTMetadataOwnerItem> callback)
    {
        StartCoroutine(PostJsonData<UploadSapshotMessage, NFTMetadataOwnerItem>("https://web3funcs.azurewebsites.net/api/web3funcmint",
            new UploadSapshotMessage
            {
                username = userName,
                base64stream = fileBase64Content,
                filename = fileName,
                nftdesc = nftDesc,
                nftname = nftName,
            },
            callback
            ));


    }

    public IEnumerator PostJsonData<Tin, Tout>(string url, Tin message, Action<Tout> resultHandler) where Tout : class
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        var jsonToUp = JsonUtility.ToJson(message);
        Debug.Log(jsonToUp);
        var dataBytes = Encoding.UTF8.GetBytes(jsonToUp);
        request.uploadHandler = new UploadHandlerRaw(dataBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        Debug.Log(request.responseCode);

        if (request.isHttpError || request.isNetworkError || request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            var text = request.downloadHandler.text;
            Debug.Log($"httpResponse:{text}");

            if (typeof(Tout) == typeof(string))
            {
                resultHandler.Invoke(text as Tout);
            }
            else
            {
                var json = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Tout));

                var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
                ms.Position = 0;

                var msgout = json.ReadObject(ms) as Tout;
                Debug.Log(msgout);
                resultHandler?.Invoke(msgout);
            }

        }
    }

}

public class UserNameRequestMessage
{
    public string username;
}

public class UploadSapshotMessage
{
    //{"username": "user1 or user2","nftname":"XXX","nftdesc":"XXX"}
    public string username;
    public string nftname;
    public string nftdesc;
    public string filename;
    public string base64stream;

}

public class NFTMetadataOwnerMetadata
{
    public int id;
    public string uri;
    public string nft_name;
    public string nft_description;
    public string image_url;

}

public class NFTMetadataOwner
{
    public NFTMetadataOwnerMetadata metadata;
    public string owner;

}

public class NFTMetadataOwnerItem
{
    public NFTMetadataOwner NFTMetadataOwner;

}

public class NFTMetadataOwnerArrayEnvelop
{
    public List<NFTMetadataOwnerItem> response;
}
