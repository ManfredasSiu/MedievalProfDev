using CodeMonkey.Utils;
using System;
using UnityEngine;

public class CustomGrid
{
    protected int m_Width;
    protected int m_Height;
    protected float m_CellSize;
    protected Vector3 m_OriginPos;
    protected Transform m_Parent;

    protected int[,] m_GridArray;
    protected TextMesh[,] m_DebugTextArray;

    public event Action<int, int> OnGridValueChangedEvent;


    public int Width => m_Width;
    public int Height => m_Height;
    public float CellSize => m_CellSize;

    public CustomGrid(int width, int height, float cellSize, Transform parent, Vector3 originPos)
    {
        this.m_Width = width;
        this.m_Height = height;
        this.m_CellSize = cellSize;
        this.m_Parent = parent;
        this.m_OriginPos = originPos;

        m_GridArray = new int[width, height];
        m_DebugTextArray = new TextMesh[width, height];

        for (int x = 0; x < m_GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_GridArray.GetLength(1); y++)
            {
                m_DebugTextArray[x, y] = UtilsClass.CreateWorldText(m_GridArray[x,y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) *0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100, false);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100, false);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100, false);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100, false);

        ParentTextAssets();
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * m_CellSize + m_OriginPos;
    }

    public virtual void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
        {
            m_GridArray[x, y] = value;
            m_DebugTextArray[x, y].text = value.ToString();

            CallValueChangedEvent(x, y);
        }
        else
        {
            Debug.LogError($"Invalid grid range for x = {x}, y = {y} when setting value.");
        }    
    }

    public virtual void SetValue(Vector3 worldPos, int value)
    {
        GetXY(worldPos, out int xValue, out int yValue);

        SetValue(xValue, yValue, value);
    }

    public virtual int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
        {
            return m_GridArray[x, y];
        }
        else
        {
            Debug.LogError($"Invalid grid range for x = {x}, y = {y} when getting value.");
            return -1;
        }
    }

    public int GetValue(Vector3 worldPos)
    {
        GetXY(worldPos, out int xValue, out int yValue);

        return GetValue(xValue, yValue);
    }

    public void ResetGrid()
    {
        for (int x = 0; x < m_GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_GridArray.GetLength(1); y++)
            {
                m_GridArray[x, y] = 0;
                m_DebugTextArray[x, y].text = m_GridArray[x, y].ToString();
            }
        }
    }

    protected void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-m_OriginPos).x / m_CellSize);
        y = Mathf.FloorToInt((worldPosition-m_OriginPos).y / m_CellSize);
    }

    protected void ParentTextAssets()
    {
        foreach (var textAsset in m_DebugTextArray)
        {
            textAsset.transform.SetParent(m_Parent);
        }
    }

    protected void CallValueChangedEvent(int x, int y)
    {
        OnGridValueChangedEvent?.Invoke(x, y);
    }
}
