using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private GameObject _doorButton;
    [SerializeField] private DoorController _doorController;
 //   public GameObject InteractionObject;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoorSet(DoorController door)
    {

        _doorController = door;
        _doorButton.gameObject.SetActive(true);
    }
    public void DoorDelete()
    {
        _doorController = null;
        _doorButton.gameObject.SetActive(false);
    }
    public void ClickDoorButton()
    {
        Debug.Log(gameObject.name);
        Debug.Log(_doorController);
        _doorController.DoorInteraction();
    }
}
