using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class leftBttCont : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Player player;
    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData) { player.TurnLeft(); }
    void IPointerUpHandler.OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData) { player.OnLeftButtonUp(); }
}
