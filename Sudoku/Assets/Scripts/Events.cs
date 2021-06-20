using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public delegate void UpdateCellNumber(int number);
    
    public static event UpdateCellNumber OnCellNumUpd;

    public static void UpdateCellNumMethod(int num)
    {
        if (OnCellNumUpd != null)
        {
            OnCellNumUpd(num);
        }
    }

    public delegate void CellSelected(int row, int col);

    public static event CellSelected OnCellSelected;

    public static void OnCellSelectedMethod(int row, int col)
    {
        if (OnCellSelected != null)
        {
            OnCellSelected(row, col);
        }
    }
}
