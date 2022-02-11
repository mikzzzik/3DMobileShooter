using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class AdminAuthorizationDataBase : MonoBehaviour
{
    [SerializeField] private InputField _loginField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private GameObject _menuPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearSpace(InputField inputField)
    {
        inputField.text = inputField.text.Replace(" ", "");
    }

    public void SignInButton()
    {
        if (_loginField.text.Length > 4 && _passwordField.text.Length > 4)
        {
            StartCoroutine(SignIn());
            _passwordField.text = string.Empty;
        }
        else
        {
            _passwordField.text = string.Empty;
        }
    }
    private IEnumerator SignIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("login", _loginField.text);
        form.AddField("password", _passwordField.text);
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

            Debug.Log("right");
            int id = int.Parse(webRequest[0]);
            PlayerPrefs.SetInt("PlayerID", id);
            int admin = int.Parse(webRequest[2]);
            PlayerPrefs.SetInt("AdminLevel", admin);
            if (admin == 0) yield break;
            _menuPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

        else
        {

            Debug.Log("Error: " + webRequest[0]);
        }

    }
}
