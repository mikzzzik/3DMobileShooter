using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LocationExitController : MonoBehaviour
{
    [SerializeField] private GameObject _exitPanel;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory.OnRaidExit();
        _exitPanel.SetActive(true);
    }
}
