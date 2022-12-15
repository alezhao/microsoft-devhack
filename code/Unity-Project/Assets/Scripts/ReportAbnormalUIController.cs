using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class ReportAbnormalUIController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro CallingResult;
    public GridObjectCollection objectCollection;
    public int PlannedItemsCount =6;
    public HistoryButtonItem[] RecentNFCButtons ;
    public HistoryButtonItem template;
    public string UserName
    {
        get
        {

            var username = LoginController.CurrentInstance?.UserSelected;


            username = string.IsNullOrWhiteSpace(username) ? "user1" : username;
            return username;

        }
    }

    public void CallGetHistory()
    {
        gameObject.GetComponent<FunctionCalls>().CallGetCollection(UserName, r =>
        {
            var newitems = r.response.OrderByDescending(x => x.NFTMetadataOwner.metadata.id).Take(PlannedItemsCount).ToList();
            if (RecentNFCButtons!=null)
            {
                foreach (var buttonsToDelete in RecentNFCButtons)
                {
                    buttonsToDelete.transform.SetParent(null);
                    Destroy(buttonsToDelete.gameObject);
                }
            }
            RecentNFCButtons= newitems.Select(item =>
            {
                var ng = Instantiate(template.gameObject, objectCollection.transform);
                var hbtn = ng.GetComponent<HistoryButtonItem>();
                var obt = ng.GetComponent<ButtonConfigHelper>();
                hbtn.AssetUrl = item.NFTMetadataOwner.metadata.image_url;
                obt.MainLabelText = $"Id:{item.NFTMetadataOwner.metadata.id}\r\nOwner:{item.NFTMetadataOwner.owner}";
                ng.SetActive(true);
                return hbtn;
            }
            ).ToArray();
            
            objectCollection.UpdateCollection();
        });


    }


    public async void CallSnapshotReport()
    {
        //Debug.Log(nameof(CallSnapshotReport));
        //var lst = WindTurbineGameEvent.historicalData.ToList().Select(x=>x.windTurbineData).ToArray();
        //var obj = new WindTurbineDataArrayEnvelop() { windTurbineDataArray = lst };
        //var jsonSlzr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(WindTurbineDataArrayEnvelop), new[] { typeof(WindTurbineData) });

        //var ms = new MemoryStream();
        //jsonSlzr.WriteObject(ms, obj);
        //ms.Position= 0;
        //var bytes = ms.ToArray();
        //Debug.Log(Encoding.UTF8.GetString(bytes));
        //gameObject.GetComponent<FunctionCalls>().CallUploadSnapshot(
        //    UserName,
        //    "Abnormal",
        //    $"Abnormal Happend in {DateTime.Now} ({DateTime.UtcNow} UTC)",
        //    $"Abnormal{DateTime.UtcNow:yyyyMMddHHmmss}.json", Convert.ToBase64String(bytes),
        //    r =>
        //    {
        //        CallingResult.text = $"Created: {r.NFTMetadataOwner.metadata.id} , {r.NFTMetadataOwner.owner} ";
        //        CallGetHistory();
        //    }); 
        Debug.Log(nameof(CallSnapshotReport));
        var lst = WindTurbineGameEvent.historicalData.ToList().Select(x => x.windTurbineData).ToArray();
        var obj = new WindTurbineDataArrayEnvelop() { windTurbineDataArray = lst };
       var json=JsonUtility.ToJson(obj);
        Debug.Log(json);
        gameObject.GetComponent<FunctionCalls>().CallUploadSnapshot(
            UserName,
            "Abnormal",
            $"Abnormal Happend in {DateTime.Now} ({DateTime.UtcNow} UTC)",
            $"Abnormal{DateTime.UtcNow:yyyyMMddHHmmss}.json", Convert.ToBase64String(Encoding.UTF8.GetBytes(json)),
            r =>
            {
                CallingResult.text = $"Created: {r.NFTMetadataOwner.metadata.id} , {r.NFTMetadataOwner.owner} ";
                CallGetHistory();
            });


    }
    private void Awake()
    {
        CallGetHistory();
    }


    private void Start()
    {

    }

    

}
public class WindTurbineDataArrayEnvelop
{
   public WindTurbineData[] windTurbineDataArray;

}