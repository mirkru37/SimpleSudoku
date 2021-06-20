using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCell : Selectable, IPointerClickHandler, ISubmitHandler
{
    //323232
    // Start is called before the first frame update
    private Color colorUsual = new Color(0.87f, 0.87f, 0.87f),
        colorSelected = new Color(1f, 0.95f, 0.61f);
    
    private Color colorHightlight = new Color(0.52f, 0.47f, 0.26f, 0.54f);

    private Color colorWrong = new Color(1f, 0.3f, 0.28f), colorRight = new Color(0.2f, 0.2f, 0.2f);

    private int num = 0;
    private int row, column;

    public GameObject text;
    public GameObject image;
    public GameObject grid;

    private bool selected = false;
    private bool editable = true;
    private bool right = false;

    protected override void Start()
    {
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        
    }

    public bool isRight()
    {
        return right;
    }
    
    public void Deselect()
    {
        selected = false;
        image.GetComponent<Image>().color = colorUsual;
    }

    public void Hightlight()
    {
        image.GetComponent<Image>().color = colorHightlight;
    }
    
    public void Dehightlight()
    {
        image.GetComponent<Image>().color = colorUsual;
    }
    
    public void Select()
    {
        selected = true;
        image.GetComponent<Image>().color = colorSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //selected = true;
        Events.OnCellSelectedMethod(row, column);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
    }

    public void setWrong()
    {
        text.GetComponent<Text>().color = colorWrong;
        right = false;
    }

    public void setRight()
    {
        right = true;
        text.GetComponent<Text>().color = colorRight;
    }
    
    public void displayText()
    {
        if (num > 0)
        {
            text.GetComponent<Text>().text = num.ToString();
        }
    }
    
    // private void OnEnable()
    // {
    //     Events.OnCellSelected += OnCellSelected;
    // }
    //
    // private void OnDisable()
    // {
    //     Events.OnCellSelected -= OnCellSelected;
    // }

    public bool isEditable()
    {
        return editable;
    }

    public void setNum(int num)
    {
        this.num = num;
        displayText();
    }
    
    public void setData(int num, int row, int column, bool editable)
    {
        this.editable = editable;
        this.num = num;
        this.column = column;
        this.row = row;
        text.GetComponent<Text>().fontStyle = FontStyle.Bold;
        displayText();
    }

    
}
