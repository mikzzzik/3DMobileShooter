using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SlotItemController : EventTrigger
{


    private bool _nowDrag;
    private void Start()
    {

    }

    private void Update()
    {
        if(_nowDrag)
        {
            this.gameObject.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
      
        _nowDrag = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        this.gameObject.transform.localPosition = Vector3.zero;
        _nowDrag = false;
    }
}
