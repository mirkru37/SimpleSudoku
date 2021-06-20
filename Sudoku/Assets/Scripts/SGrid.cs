using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SGrid : MonoBehaviour
{
    public float cellOffset = 0;

    private int cellsFilled = 0;
    public int size = 0;
    private int selectedRow = -1, selectedCol = -1;
    private int difficulty = 2;

    public GameObject cell;
    
    private List<List<GameObject>> _cells = new List<List<GameObject>>();

    private List<List<int>> _numbers = new List<List<int>>();
    // {
    //     new List<int>() {0,0,0,0,7,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0},
    //     new List<int>() {0,0,0,0,0,0,0,0,0}
    // };
    
    private bool _isGridCellNull;

    public Vector2 startPos = new Vector2(0.0f, 0.0f);
    

    // Start is called before the first frame update
    void Start()
    {
        _isGridCellNull = cell.GetComponent<GridCell>() == null;
        if (_isGridCellNull)
            Debug.Log("Cell without script");
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GenerateGrid()
    {
        GenerateCells();
        SetCellPos();
        generateNumbers();
        cleanRandom(difficulty);
        cellsFilled = (int)Math.Pow(size, 2) - difficulty;
        setNumbers();
    }

    private void cleanRandom(int i)
    {
        int cleaned = 0;
        while (cleaned < i)
        {
            int r, c;
            r = Random.Range(0, size);
            c = Random.Range(0, size);
            // int rr = Random.Range(0, size * size);
            // r = rr / size;
            // c = rr % size;
            // if (c != 0)
            //     c -= 1;
            if (_numbers[r][c] != 0)
            {
                _numbers[r][c] = 0;
                cleaned++;
            }
        }
    }

    private void generateNumbers()
    {
        _numbers = Enumerable.Repeat(new List<int>(), 9).ToList();
        for (int i = 0; i < size; i++)
        {
            _numbers[i] = Enumerable.Repeat(0, 9).ToList();
        }

        fillDiagonal3x3();
        FillRest(0, 0);
    }

    private bool FillRest(int i, int j)
    {
        if (j == size)
        {
            j = 0;
            i++;
        }

        if (i == size)
            return true;
        
        if (_numbers[i][j] != 0)
            return FillRest(i, ++j);
        
        for (int n = 1; n < 10; n++)
        {
            if (isSafe((int) Math.Sqrt(size), i, j, n))
            {
                _numbers[i][j] = n;
                if(FillRest(i, j+1))
                    return true;
                _numbers[i][j] = 0;
            }
        }

        return false;
    }

    private bool isSafe(int boxSize ,int row, int col, int num)
    {
        return !(isInBox(row, col, boxSize, num) || isInRow(row, num) || isInColumn(col, num));
    }

    private bool isInColumn(int column, int num)
    {
        bool res = false;
        for (int i = 0; i < size; i++)
        {
            res = res || _numbers[i][column] == num;
        }
        return res;
    }

    private bool isInRow(int row, int num)
    {
        bool res = false;
        for (int i = 0; i < size; i++)
        {
            res = res || _numbers[row][i] == num;
        }
        return res;
    }

    private void fillDiagonal3x3()
    {
        for (int i = 0; i < size; i += (int)Math.Sqrt(size))
        {
            fillBoxRnd(i, (int)Math.Sqrt(size));
        }
    }

    private void fillBoxRnd(int startP, int boxSize)
    {
        int num;
        for (int i = startP; i < startP + boxSize; i++)
        {
            for (int j = startP; j < startP + boxSize; j++)
            {
                do
                {
                    num = Random.Range(1, 10);
                } while (isInBox(startP,startP, boxSize, num));

                _numbers[i][j] = num;
                //Debug.Log(_numbers[i][j] + " " + num);
            }
        }
    }

    private bool isInBox(int row, int col, int boxSize, int num)
    {
        bool res = false;
        row = boxSize * (row / boxSize);
        col = boxSize * (col / boxSize);
        for (int i = row; i < row + boxSize; i++)
        {
            for (int j = col; j < col + boxSize; j++)
            {
                res = res || _numbers[i][j] == num;
            }
        }
        return res;
    }

    private void GenerateCells()
    {
        for (int i = 0; i < size; i++)//rows
        {
            _cells.Add(new List<GameObject>());
            for (int j = 0; j < size; j++) //column
            {
                _cells[i].Add(Instantiate(cell, transform.position, Quaternion.identity, transform));
            }
        }
    }

    private void SetCellPos()
    {
        var cellRect = _cells[0][0].GetComponent<RectTransform>();
        var localScale = cellRect.transform.localScale;
        var rect = cellRect.rect;
        Vector2 offset = new Vector2(rect.width * localScale.x + cellOffset, 
            rect.height * localScale.y + cellOffset);


        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var pos_x = offset.x * j;
                var pos_y = offset.y * i;


                _cells[i][j].GetComponent<RectTransform>().anchoredPosition =
                    new Vector3(startPos.x + pos_x, startPos.y - pos_y, 90f);
            }
        }
    }

    private void setNumbers()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _cells[i][j].GetComponent<GridCell>().setData(_numbers[i][j], i, j, _numbers[i][j] == 0);
            }
        }
    }
    
    private void OnEnable()
    {
        Events.OnCellNumUpd += OnSetNum;
        Events.OnCellSelected += OnCellSelected;
    }
    
    private void OnDisable()
    {
        Events.OnCellNumUpd -= OnSetNum;
        Events.OnCellSelected -= OnCellSelected;
    }

    public void OnSetNum(int numb)
    {
        if (selectedCol != -1 && selectedRow != -1 && _cells[selectedRow][selectedCol].GetComponent<GridCell>().isEditable())
        {
            _cells[selectedRow][selectedCol].GetComponent<GridCell>().setNum(numb);
            if (!isSafe((int)Math.Sqrt(size), selectedRow, selectedCol, numb))
            {
                if (_cells[selectedRow][selectedCol].GetComponent<GridCell>().isRight())
                    cellsFilled--;
                _cells[selectedRow][selectedCol].GetComponent<GridCell>().setWrong();
            }
            else
            {
                if (!_cells[selectedRow][selectedCol].GetComponent<GridCell>().isRight())
                    cellsFilled++;
                _cells[selectedRow][selectedCol].GetComponent<GridCell>().setRight();
                if (cellsFilled == Math.Pow(size,2))
                    Debug.Log("You won");
            }
        }
    }

    public void OnCellSelected(int row, int col)
    {
        if (selectedRow != -1 && selectedCol != -1)
        {
            _cells[selectedRow][selectedCol].GetComponent<GridCell>().Deselect();
            DeHighlightGroup();
        }
        selectedCol = col;
        selectedRow = row;
        _cells[selectedRow][selectedCol].GetComponent<GridCell>().Select();
        HighlightGroup();
    }

    
    private void HighlightGroup() //function to highlight group adjacent elements
    {
        int row = (int)Math.Sqrt(size) * (selectedRow / (int)Math.Sqrt(size));
        int col = (int)Math.Sqrt(size) * (selectedCol / (int)Math.Sqrt(size));
        for (int i = row; i < row + (int)Math.Sqrt(size); i++)
        {
            for (int j = col; j < col + (int)Math.Sqrt(size); j++)
            {
                if (i != selectedRow || j != selectedCol)
                    _cells[i][j].GetComponent<GridCell>().Hightlight();
            }
        }
        
        for (int i = 0; i < size; i++)
        {
            if (i != selectedRow)
                _cells[i][selectedCol].GetComponent<GridCell>().Hightlight();
            if (i != selectedCol)
                _cells[selectedRow][i].GetComponent<GridCell>().Hightlight();
        }
    }

    private void DeHighlightGroup()
    {
        int row = (int)Math.Sqrt(size) * (selectedRow / (int)Math.Sqrt(size));
        int col = (int)Math.Sqrt(size) * (selectedCol / (int)Math.Sqrt(size));
        for (int i = row; i < row + (int)Math.Sqrt(size); i++)
        {
            for (int j = col; j < col + (int)Math.Sqrt(size); j++)
            {
                if (i != selectedRow || j != selectedCol)
                    _cells[i][j].GetComponent<GridCell>().Dehightlight();
            }
        }
        for (int i = 0; i < size; i++)
        {
            if (i != selectedRow)
                _cells[i][selectedCol].GetComponent<GridCell>().Dehightlight();
            if (i != selectedCol)
                _cells[selectedRow][i].GetComponent<GridCell>().Dehightlight();
        }
    }
}
