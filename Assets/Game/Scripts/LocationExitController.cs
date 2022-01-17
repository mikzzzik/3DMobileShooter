using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LocationExitController : MonoBehaviour
{
    [SerializeField] private GameObject _exitPanel;
    [SerializeField] private PlayerStatistic _playerStatistic;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory.OnRaidExit();
        _exitPanel.SetActive(true);
        StartCoroutine(_playerStatistic.SaveStatistic());
    }
}
