using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class rightBttCont : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Player player;
    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData) { player.TurnRight(); }
    void IPointerUpHandler.OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData) { player.OnRightButtonUp(); }
}
