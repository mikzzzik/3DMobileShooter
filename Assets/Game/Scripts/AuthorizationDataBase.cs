using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.SceneManagement;

public class AuthorizationDataBase : MonoBehaviour
{
    [SerializeField] private InputField _signInLoginField;
    [SerializeField] private InputField _signInPasswordField;
    [SerializeField] private InputField _signUpLoginField;
    [SerializeField] private InputField _signUpPasswordField;
    [SerializeField] private Text _signUpErrorText;
    [SerializeField] private Text _signInErrorText;
    public void SignInButton()
    {
        _signInErrorText.text = string.Empty;
        _signUpErrorText.text = string.Empty;

        StartCoroutine(SignIn());
    }

    public void SignUpButton()
    {
        _signInErrorText.text = string.Empty;
        _signUpErrorText.text = string.Empty;

        if (_signUpLoginField.text.Length > 4 && _signUpPasswordField.text.Length > 4)
        {
            StartCoroutine(SignUp());
            _signUpLoginField.text = string.Empty;
            _signUpPasswordField.text = string.Empty;
        }
        else
        {
            _signUpPasswordField.text = string.Empty;

            Debug.Log("Error...");

        }
    }

    public void ClearSpace(InputField inputField)
    {
        inputField.text = inputField.text.Replace(" ", "");
    }

    private IEnumerator SignUp()
    {
        WWWForm form = new WWWForm();
        form.AddField("login", _signUpLoginField.text);
        form.AddField("password", _signUpPasswordField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/SignUp.php", form);

        yield return www.SendWebRequest();
        List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

        string status = webRequest[0];
        webRequest.RemoveAt(0);
        Debug.Log(status);
     
        if (www.error != null)
        {
            Debug.Log("Error:" + www.error);
            yield break;
        }

        for (int i = 0; i < webRequest.Count / 2; i++)
        {

            _signUpErrorText.text = webRequest[i];

        }
    }

    private IEnumerator SignIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("login", _signInLoginField.text);
        form.AddField("password", _signInPasswordField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/SignIn.php", form);

        yield return www.SendWebRequest();
        if (www.error != null)
        {
            Debug.Log("Error:" + www.error);
            yield break;
        }

        List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

        string status = webRequest[0];
        webRequest.RemoveAt(0);
        Debug.Log(status);
        if (bool.Parse(status))
        {

                PlayerPrefs.SetInt("PlayerID", int.Parse(webRequest[0]));
            StartCoroutine(LoadScene());
        }

        else
        {
            _signInErrorText.text = webRequest[0];
            Debug.Log("Error: " + webRequest[0]);
        }

    }
    
    private IEnumerator LoadScene()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync("MainMenu");
        while (!loading.isDone)
        {
            Debug.Log(loading.progress);

            yield return null;
        }

    }
}