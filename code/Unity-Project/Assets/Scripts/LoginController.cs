using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoginController : MonoBehaviour
{
    public static LoginController CurrentInstance { get; private set; }

    public LoginController()
    {
        CurrentInstance = this;
    }


    public TextMeshPro UserSelectionMessage;
    public TextMeshPro ErrorMessage;
    public PressableButtonHoloLens2 buttonLogin;
    public string UserSelected;
    public string Error;
    public void SelectUser1()
    {
        UserSelected = "user1";
        RefreshStatus();
    }

    public void SelectUser2()
    {
        UserSelected = "user2";
        RefreshStatus();
    }

    public void RefreshStatus()
    {
        if (string.IsNullOrWhiteSpace(UserSelected))
        {
            UserSelectionMessage.text = "You have not select any user yet.";
            buttonLogin.gameObject.SetActive(false);

        }
        else
        {

            UserSelectionMessage.text = $"{UserSelected} selected";
            buttonLogin.gameObject.SetActive(true);
        }
        ErrorMessage.text = Error;
     
    }
    public void Login()
    {
        gameObject.GetComponent<FunctionCalls>().CallLogin(UserSelected, r =>
         {
             Error = r;
             if (Error.Contains("successfully"))
             {
                 SceneManager.LoadScene("CompletedScene");
             }
             RefreshStatus();
         });
    }
    private void Start()
    {
        RefreshStatus();

    }
}
