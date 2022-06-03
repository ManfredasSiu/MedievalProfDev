using CodeMonkey.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Grid.Scripts.Map.Logic
{
    public class GenericGrid<TGridObject>
    {
        protected int m_Width;
        protected int m_Height;
        protected float m_CellSize;
        protected Vector3 m_OriginPos;
        protected Transform m_Parent;

        protected TGridObject[,] m_GridArray;
        protected TextMesh[,] m_DebugTextArray;

        public event Action<int, int> OnGridValueChangedEvent;

        public Vector3 Origin => m_OriginPos;
        
        public int Width => m_Width;
        
        public int Height => m_Height;
        
        public float CellSize => m_CellSize;

        public GenericGrid(int width, int height, float cellSize, Vector3 originPos, Func<GenericGrid<TGridObject>, int, int,TGridObject> createGridObject)
        {
            m_Width = width;
            m_Height = height;
            m_CellSize = cellSize;
            m_OriginPos = originPos;

            m_GridArray = new TGridObject[width, height];
            m_DebugTextArray = new TextMesh[width, height];

            for(int x= 0;x < m_GridArray.GetLength(0);x++)
            {
                for(int y = 0; y < m_GridArray.GetLength(1);y++)
                {
                    m_GridArray[x, y] = createGridObject(this,x,y);
                }
            }

            var showDebug = true;

            if (showDebug == true)
            {
                for (int x = 0; x < m_GridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < m_GridArray.GetLength(1); y++)
                    {
                         /*m_DebugTextArray[x, y] = UtilsClass.CreateWorldText(x + " " + y, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 5, Color.white, TextAnchor.MiddleCenter);*/

                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100, false);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100, false);
                    }
                }


                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100, false);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100, false);
            }

            OnGridValueChangedEvent += (int x, int y) =>
            {
                m_DebugTextArray[x, y].text = m_GridArray[x, y]?.ToString();
            };

        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * m_CellSize + m_OriginPos;
        }

        public virtual void SetGridObject(int x, int y, TGridObject value, bool printError = false)
        {
            if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
            {
                m_GridArray[x, y] = value;
                m_DebugTextArray[x, y].text = value.ToString();

                CallValueChangedEvent(x, y);
            }
            else if(printError)
            {
                Debug.LogError($"Invalid grid range for x = {x}, y = {y} when setting value.");
            }
        }

        public virtual void SetGridObject(Vector3 worldPos, TGridObject value)
        {
            GetXY(worldPos, out int xValue, out int yValue);

            SetGridObject(xValue, yValue, value);
        }

        public virtual TGridObject GetGridObject(int x, int y, bool printError = false)
        {
            if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
            {
                return m_GridArray[x, y];
            }
            else
            {
                if(printError)
                    Debug.LogError($"Invalid grid range for x = {x}, y = {y} when getting value.");
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPos)
        {
            GetXY(worldPos, out int xValue, out int yValue);

            return GetGridObject(xValue, yValue);
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - m_OriginPos).x / m_CellSize);
            y = Mathf.FloorToInt((worldPosition - m_OriginPos).y / m_CellSize);
        }

        public void CallValueChangedEvent(int x, int y)
        {
            OnGridValueChangedEvent?.Invoke(x, y);
        }
    }
}