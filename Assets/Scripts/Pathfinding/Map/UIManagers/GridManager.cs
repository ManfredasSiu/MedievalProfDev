using Assets.Scripts.Map.Logic;
using UnityEngine;

public class GridManager : MonoBehaviour
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
    private HeatMapVisual heatmapVisual;

    private GridHeatmap m_Grid;

    public GridHeatmap Grid => m_Grid;

    // Start is called before the first frame update
    void Start()
    {
        m_Grid = new GridHeatmap(gridWidth, gridHeight, cellSize, transform, originPos);

        heatmapVisual.SetGrid(m_Grid);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            m_Grid.AddValue(mousePos, 100,5);
        }
        if(Input.GetMouseButtonDown(1))
        {
            m_Grid.ResetGrid();
        }
    }
}


