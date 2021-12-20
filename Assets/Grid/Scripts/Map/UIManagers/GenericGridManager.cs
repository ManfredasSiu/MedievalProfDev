using Assets.Grid.Scripts.Map.Logic;
using CodeMonkey.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Grid.Scripts.Map.UIManagers
{
    public class GenericGridManager : MonoBehaviour
    {
        [SerializeField]
        private int gridWidth;

        [SerializeField]
        private int gridHeight;

        [SerializeField]
        private float cellSize;

        [SerializeField]
        private Vector3 originPos;

        [SerializeField]
        private HeatMapVisualGeneric heatMapVisualBool;

        private GenericGrid<HeatMapGridObject> m_Grid;

        public GenericGrid<HeatMapGridObject> Grid => m_Grid;

        private void Start()
        {
            m_Grid = new GenericGrid<HeatMapGridObject>(gridWidth, gridHeight, cellSize, originPos, (GenericGrid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

            heatMapVisualBool.SetGrid(m_Grid);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                var position = UtilsClass.GetMouseWorldPosition();

                var heatmapGridObject = m_Grid.GetGridObject(position);

                if(heatmapGridObject != null)
                {
                    heatmapGridObject.AddValue(5);
                }
            //    m_Grid.SetValue(position, true);
            }
        }
    }
}

public class HeatMapGridObject
{
    public readonly int k_HeatMapMax = 100;
    public readonly int k_HeatMapMin = 0;

    private GenericGrid<HeatMapGridObject> grid;
    public int value;
    private int x;
    private int y;

    public HeatMapGridObject(GenericGrid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;

        value =  Mathf.Clamp(value, k_HeatMapMin, k_HeatMapMax);

        grid.CallValueChangedEvent(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / k_HeatMapMax;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}