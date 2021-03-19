using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : BaseManager<SwipeManager>, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private PlayerManager _pm;
    private GameManager _gm;
    private Vector3 _offset;
    
    protected override void Awake()
    {
        if (!instance)
            instance = this;
        base.Awake();
    }

    private void Start()
    {
        _pm = PlayerManager.instance;
        _gm = GameManager.instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _gm.StartGame();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_pm.canMove || _gm.CannotPlay() || _gm.Tutorial != 0 || _gm.isTutorial)
            return;
        
        if (eventData.delta.x < -40f)
        {
            _pm.SetDirAndCondition(_pm.xNegativeDir, Vector3.left);
        } if (eventData.delta.x > 40f)
        {
            _pm.SetDirAndCondition(_pm.xDir, Vector3.right);
        } if (eventData.delta.y > 40f)
        {
            _pm.SetDirAndCondition(_pm.zDir, Vector3.forward);
        } if (eventData.delta.y < -40f)
        {
            _pm.SetDirAndCondition(_pm.zNegativeDir, Vector3.back);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {}
}
