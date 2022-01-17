using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.Linq;
public class PlayerStatistic : MonoBehaviour
{

    [SerializeField] private Text _moneyText;
    private int _enemyKills;
    private int _takeItems;
    private int _moneyAmount;

    public static Action<int> OnUpdateMoney;
    public static Action OnUpdateItemTake;
    public static Action OnKillEnemys;


    private void OnEnable()
    {
        StartCoroutine(LoadStatistic());
        OnUpdateMoney += UpdateMoney;
        OnUpdateItemTake += UpdateItemTake;
        OnKillEnemys += UpdateKillEnemys;
    }

    private void OnDisable()
    {
        OnUpdateMoney -= UpdateMoney;
        OnUpdateItemTake -= UpdateItemTake;
        OnKillEnemys -= UpdateKillEnemys;
    }

    private void UpdateMoney(int money)
    {
        _moneyAmount += money;
        _moneyText.text = _moneyAmount.ToString("C", CultureInfo.CreateSpecificCulture("en-US")); ;
    }
    private void UpdateItemTake()
    {
        _takeItems++;
 
    }
    private void UpdateKillEnemys()
    {
        _enemyKills++;
    }

    public IEnumerator SaveStatistic()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));
        form.AddField("enemy_kills", _enemyKills);
        form.AddField("take_item", _takeItems);
        form.AddField("money_amount", _moneyAmount);

        www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/SaveStatistic.php", form);
        yield return www.SendWebRequest();
        List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

        string status = webRequest[0];

        webRequest.RemoveAt(0);
        Debug.Log(status);

        if (bool.Parse(status))
        {
            Debug.Log(webRequest[0]);
        }
        else
        {
            Debug.Log(webRequest[0]);
        }

    }

    IEnumerator LoadStatistic()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));

        www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/LoadingStatistic.php", form);
        yield return www.SendWebRequest();
        List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

        string status = webRequest[0];

        webRequest.RemoveAt(0);
        Debug.Log(status);
        if (bool.Parse(status))
        {
            Debug.Log(webRequest[0]);
            _enemyKills = int.Parse(webRequest[0]);
            _takeItems = int.Parse(webRequest[1]);
            UpdateMoney(int.Parse(webRequest[2]));
        }
        else
        {
            Debug.Log(webRequest[0]);
        }

    }
}
