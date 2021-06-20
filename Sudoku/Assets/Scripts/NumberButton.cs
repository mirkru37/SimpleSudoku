using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEditor.EventSystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public int value = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Events.UpdateCellNumMethod(value);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
    }
    
    
}
