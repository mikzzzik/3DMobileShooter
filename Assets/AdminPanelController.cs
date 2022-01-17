using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System;
public class AdminPanelController : MonoBehaviour
{
    [SerializeField] private Button _globalStatisticButton;
    [SerializeField] private Button _playerStatisticButton;
    [SerializeField] private Button _itemsButton;

    [SerializeField] private Text _userCountText;
    [SerializeField] private Text _timePlayedCountText;
    [SerializeField] private Text _itemOnMarketplaceCountText;

    [SerializeField] private RectTransform _playerStatisticContainer;
    [SerializeField] private PlayerStatisticContainer _playerStatisticContainerPrefab;

    [SerializeField] private RectTransform _itemContent;
    [SerializeField] private ItemContainer _itemContainer;

    public static Action<int, string, string, string, string> OnUpdateStatistic;
    public static Action<string, string, string, string, string> OnUpdateItem;
    void OnEnable()
    {
        _globalStatisticButton.interactable = false;
        _playerStatisticButton.interactable = false;
        _itemsButton.interactable = false;

        StartCoroutine(LoadGlobalStat());
        StartCoroutine(LoadPlayerStat());
        StartCoroutine(LoadItems());
        OnUpdateStatistic += SaveStatistic;
        OnUpdateItem += SaveItem;
    }

    private void OnDisable()
    {
        OnUpdateStatistic -= SaveStatistic;
        OnUpdateItem -= SaveItem;
    }

    private IEnumerator LoadGlobalStat()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/GetGlobalStatistic.php", form);

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
            _userCountText.text = webRequest[0];
            _timePlayedCountText.text = webRequest[1];
            _itemOnMarketplaceCountText.text = webRequest[2];
        }
        else
        {
            Debug.Log("Error: " + webRequest[0]);
        }
        _globalStatisticButton.interactable = true;
    }

    private IEnumerator LoadPlayerStat()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/GetPlayerStatistic.php", form);

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
            _playerStatisticContainer.sizeDelta = new Vector2(0, 200 * webRequest.Count / 6);
            for (int i = 0; i < webRequest.Count/6;i++ )
            {
               var container = Instantiate(_playerStatisticContainerPrefab, _playerStatisticContainer) as PlayerStatisticContainer;
                Debug.Log(webRequest[i * 6]);
                container.Init(int.Parse(webRequest[i * 6]),webRequest[i * 6 + 1],webRequest[i * 6 + 2], webRequest[i * 6 + 3],webRequest[i * 6 + 4],webRequest[i * 6 + 5]);
            }
        }
        else
        {
            Debug.Log("Error: " + webRequest[0]);
        }
        _playerStatisticButton.interactable = true;
    }

    private IEnumerator LoadItems()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/GetItems.php", form);

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
            _itemContent.sizeDelta = new Vector2(0, 200 * webRequest.Count / 5);
            for (int i = 0; i < webRequest.Count / 5; i++)
            {
                var container = Instantiate(_itemContainer, _itemContent) as ItemContainer;
                Debug.Log(webRequest[i * 5]);
                container.Init(webRequest[i * 5], webRequest[i * 5 + 1], webRequest[i * 5 + 2], webRequest[i * 5 + 3], webRequest[i * 5 + 4]);
            }
        }
        else
        {
            Debug.Log("Error: " + webRequest[0]);
        }
        _itemsButton.interactable = true;
    }

    private void SaveItem(string objName, string name, string description, string max, string minPrice)
    {
        StartCoroutine(SaveItemTry(objName, name, description, max, minPrice));
    }
    private IEnumerator SaveItemTry(string objName, string name, string description, string max, string minPrice)
    {
        WWWForm form = new WWWForm();
        form.AddField("object_name", objName);
        form.AddField("name", name);
        form.AddField("description", description);
        form.AddField("max_amount", max);
        form.AddField("min_price", minPrice);
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/UpdateItem.php", form);

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
            Debug.Log("true");
        }
        else
        {
            Debug.Log("Error: " + webRequest[0]);
        }
    }

    private void SaveStatistic(int id, string kills, string items, string sell, string money)
    {
        StartCoroutine(SavePlayerStatistic(id, kills, items, sell, money));
    }
    private IEnumerator SavePlayerStatistic(int id, string kills, string items,string sell,string money)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("enemy_kills", kills);
        form.AddField("take_item", items);
        form.AddField("sell_item", sell);
        form.AddField("money_amount", money);
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/UpdatePlayerStatistic.php", form);

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
            Debug.Log("true");
        }
        else
        {
            Debug.Log("Error: " + webRequest[0]);
        }
    }

}
