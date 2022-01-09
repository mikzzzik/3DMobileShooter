using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class SlotDrag : EventTrigger
{

	private Vector3 _position;
    private bool _drag;
    [SerializeField] private SlotContainer _slotContainer;

    public static Action<SlotContainer> OnSetTargetItem;
    public static Action<Vector3> OnDragitem;
    public static Action<SlotContainer> OnSetNewSlot;

    private void Awake()
    {
        if (_slotContainer == null)
            _slotContainer = GetComponentInParent<SlotContainer>();
		_position = transform.localPosition;    
    }
    public override void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag called.");
        OnSetTargetItem(_slotContainer);
        OnDragitem(data.delta);
        _slotContainer.UpdateSlot(null);
    }

    public override void OnDrag(PointerEventData data)
	{
    
        OnDragitem(data.position);

    }



    public override void OnEndDrag(PointerEventData data)
	{
        _drag = false;
        OnSetTargetItem(null);
        Debug.Log("OnDrop called.");

	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnSetNewSlot(_slotContainer);
        Debug.Log("Collision enter");
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        OnSetNewSlot(null);
        Debug.Log("Collision exit");
    }
}
