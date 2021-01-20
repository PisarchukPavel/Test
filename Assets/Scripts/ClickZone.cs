using UnityEngine;
using UnityEngine.EventSystems;

public class ClickZone : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] 
    private PointVariable _lastClickedPoint;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        _lastClickedPoint.Value = eventData.position;
    }
}