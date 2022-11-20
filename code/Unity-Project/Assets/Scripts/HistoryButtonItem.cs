using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryButtonItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string AssetUrl;
    public void ShowAsset()
    {
        Application.OpenURL(AssetUrl);
    }
}
