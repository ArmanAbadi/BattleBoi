using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Username;
    public TMP_InputField Password;

    public TMP_InputField Username2;
    public TMP_InputField Password2;

    public GameObject LoadingVeil;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.GetInt("LoginFullUser") == 1)
        {
            string email = PlayerPrefs.GetString("Email");
            string username = PlayerPrefs.GetString("Username"); ;
            string password = PlayerPrefs.GetString("Password"); ;


            var request = new LoginWithPlayFabRequest { Username = username, Password = password };

            PlayFabClientAPI.LoginWithPlayFab(request, OnAutoLoginSuccessUsernamePassword, OnLoginFailure);
            LoadingVeil.SetActive(true);
        }
    }
    public void LoginWithUsername()
    {
        string username = Username2.text;
        string password = Password2.text;


        var request = new LoginWithPlayFabRequest { Username = username, Password = password };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccessUsernamePassword, OnLoginFailure);
        LoadingVeil.SetActive(true);
    }
    public void CreateAccount()
    {
        /* if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
         {
             *//*
             Please change the titleId below to your own titleId from PlayFab Game Manager.
             If you have already set the value in the Editor Extensions, this can be skipped.
             *//*
             PlayFabSettings.staticSettings.DeveloperSecretKey = "X1MAPBOBY8T1H3UFJXRRONH5QR9E3WCT37TB3PTQC1O3SWHD5W";
             PlayFabSettings.staticSettings.TitleId = "BattleBoi";
         }
         var request = new LoginWithCustomIDRequest { CustomId = "asd", CreateAccount = true };
         PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);*/

        var request = new RegisterPlayFabUserRequest { Email = Email.text, Username = Username.text, Password = Password.text};
        PlayFabClientAPI.RegisterPlayFabUser(request, OnLoginSuccess, OnLoginFailure);
        LoadingVeil.SetActive(true);
    }
    public void PlayAsGuest()
    {
        string RandomId = PlayerPrefs.GetString("CustomId");
        if (string.IsNullOrEmpty(RandomId))
        {
            RandomId = Random.Range(0, 100000000).ToString();

            PlayerPrefs.SetString("CustomId", RandomId);
        }
        var request = new LoginWithCustomIDRequest { CustomId = RandomId, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessCustomId, OnLoginFailure);
    }

    private void OnLoginSuccess(RegisterPlayFabUserResult result)
    {
        SaveSignupDetails();
        LetsPlay();
    }
    void SaveSignupDetails()
    {

        PlayerPrefs.SetInt("LoginFullUser", 1);

        PlayerPrefs.SetString("Email", Email.text);
        PlayerPrefs.SetString("Username", Username.text);
        PlayerPrefs.SetString("Password", Password.text);
    }
    void SaveLoginDetails()
    {
        PlayerPrefs.SetInt("LoginFullUser", 1);

        PlayerPrefs.SetString("Username", Username2.text);
        PlayerPrefs.SetString("Password", Password2.text);
    }
    private void OnLoginSuccessCustomId(LoginResult result)
    {
        LetsPlay();
    }
    private void OnAutoLoginSuccessUsernamePassword(LoginResult result)
    {
        LetsPlay();
    }
    private void OnLoginSuccessUsernamePassword(LoginResult result)
    {
        SaveLoginDetails();
        LetsPlay();
    }


    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        LoadingVeil.SetActive(false);
    }

    void LetsPlay()
    {
        NetworkManager.Instance.StartGameHost();
    }
}