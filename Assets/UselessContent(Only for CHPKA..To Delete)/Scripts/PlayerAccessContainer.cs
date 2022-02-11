using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAccessContainer : MonoBehaviour
{
   

    [SerializeField] private InputField _acessLevelField;
    [SerializeField] private Text _passwordText;
    [SerializeField] private Text _loginText;

    private int _id;

    private bool _active = true;
    public void Init(int id, string login, string password, string accessLevel)
    {

        _id = id;
        _loginText.text = login;
        _acessLevelField.text = accessLevel;
        _passwordText.text = password;

        EditButton();

    }

    public void EditButton()
    {
        _active = !_active;
        _acessLevelField.targetGraphic.enabled = _active;

        _acessLevelField.interactable = _active;

    }

    public void Save()
    {
        AdminPanelController.OnUpdateAccess(_id, _acessLevelField.text);
        if (_active)
            EditButton();
    }
}
