using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatisticContainer : MonoBehaviour
{
    [SerializeField] private InputField _enemyKillsField;
    [SerializeField] private InputField _takeItemsField;
    [SerializeField] private InputField _sellItemsField;
    [SerializeField] private InputField _amountMoneyField;
    [SerializeField] private Text _loginText;

    private int _id;

    private bool _active = true;
    public void Init(int id,string login, string kills, string take, string sell, string money)
    {
        _id = id;


        _loginText.text = login;
        _enemyKillsField.text = kills;
        _takeItemsField.text = take;
        _sellItemsField.text = sell;
        _amountMoneyField.text = money;

        EditButton();

    }

    public void EditButton()
    {
        _active = !_active;
        _enemyKillsField.targetGraphic.enabled = _active;
        _takeItemsField.targetGraphic.enabled = _active;
        _sellItemsField.targetGraphic.enabled = _active;
        _amountMoneyField.targetGraphic.enabled = _active;

        _enemyKillsField.interactable = _active;
        _takeItemsField.interactable = _active;
        _sellItemsField.interactable = _active;
        _amountMoneyField.interactable = _active;
    }

    public void Save()
    {
        AdminPanelController.OnUpdateStatistic(_id, _enemyKillsField.text, _takeItemsField.text, _sellItemsField.text, _amountMoneyField.text);
        if(_active)
        EditButton();
    }
}
