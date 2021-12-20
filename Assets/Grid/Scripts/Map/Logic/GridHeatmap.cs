using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Map.Logic
{
    public class GridHeatmap : CustomGrid
    {
        public readonly int k_HeatMapMax = 100;
        public readonly int k_HeatMapMin = 0;

        public GridHeatmap(int width, int height, float cellSize, Transform parent, Vector3 originPos) : base(width, height, cellSize, parent, originPos)
        {
        }

        public override void SetValue(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
            {
                m_GridArray[x, y] = Mathf.Clamp(value, k_HeatMapMin, k_HeatMapMax);
                m_DebugTextArray[x, y].text = value.ToString();

                CallValueChangedEvent(x, y);
            }
            else
            {
                Debug.LogError($"Invalid grid range for x = {x}, y = {y} when setting value.");
            }
        }

        public void AddValue(int x, int y, int value)
        {
            SetValue(x, y, GetValue(x, y) + value);
        }

        public override int GetValue(int x, int y)
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

        public void AddValue(Vector3 worldPos, int value, int range)
        {
            GetXY(worldPos, out int originX, out int originY);

            for(int x = 0; x < range; x++)
            {
                for(int y = 0; y < range-x; y++)
                {
                    var currentValue = value - (x + y) * 15;
                    AddValue(originX + x, originY + y, currentValue);

                    if (x != 0)
                    {
                        AddValue(originX - x, originY + y, currentValue);
                    }
                    if (y != 0)
                    {
                        AddValue(originX + x, originY - y, currentValue);
                        if(x != 0)
                        {
                            AddValue(originX - x, originY - y, currentValue);
                        }
                    }

                }
            }
        }
    }
}