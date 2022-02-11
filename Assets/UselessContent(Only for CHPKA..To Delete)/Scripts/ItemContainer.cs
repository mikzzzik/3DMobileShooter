using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemContainer : MonoBehaviour
{
    [SerializeField] private InputField _nameField;
    [SerializeField] private InputField _descriptionField;
    [SerializeField] private InputField _maxAmountField;
    [SerializeField] private InputField _minPriceField;
    [SerializeField] private Text _objectName;

    private bool _active = true;

    public void Init(string objName,string name, string description, string max, string minPrice)
    {
        _objectName.text = objName;
        _nameField.text = name;
        _descriptionField.text = description;
        _maxAmountField.text = max;
        _minPriceField.text = minPrice;

        EditButton();
    }
    public void EditButton()
    {
        _active = !_active;
        _nameField.targetGraphic.enabled = _active;
        _descriptionField.targetGraphic.enabled = _active;
        _maxAmountField.targetGraphic.enabled = _active;
        _minPriceField.targetGraphic.enabled = _active;

        _nameField.interactable = _active;
        _descriptionField.interactable = _active;
        _maxAmountField.interactable = _active;
        _minPriceField.interactable = _active;
    }

    public void Save()
    {
        AdminPanelController.OnUpdateItem(_objectName.text, _nameField.text, _descriptionField.text, _maxAmountField.text, _minPriceField.text);
        if (_active)
            EditButton();
    }
}
