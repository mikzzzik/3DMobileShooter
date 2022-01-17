using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
public class ItemDataBaseLoader : MonoBehaviour
{
    [SerializeField] private List<Item> _itemList;
    void Awake()
    {
        StartCoroutine(LoadItemData());
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadItemData()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;

        for (int i = 0; i < _itemList.Count; i++)
        {

            form.AddField("item_obj_name", _itemList[i].name);
            form.AddField("name", _itemList[i].Name);
            form.AddField("description", _itemList[i].Description);
            form.AddField("max_amount", _itemList[i].MaxAmount);
            form.AddField("min_price", _itemList[i].MinPrice);


            www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/ItemLoader.php", form);
            yield return www.SendWebRequest();
            List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

            string status = webRequest[0];

            webRequest.RemoveAt(0);
            Debug.Log(status);
            if (bool.Parse(status))
            {
                _itemList[i].Name = webRequest[0];
                _itemList[i].Description = webRequest[1];
                _itemList[i].MaxAmount = int.Parse(webRequest[2]);
                _itemList[i].MinPrice = int.Parse(webRequest[3]);
                _itemList[i].ItemId = int.Parse(webRequest[4]);
                Debug.Log(int.Parse(webRequest[4]));
            }
            else
            {
                Debug.Log(webRequest[0]);
            }
        }
    }
}
