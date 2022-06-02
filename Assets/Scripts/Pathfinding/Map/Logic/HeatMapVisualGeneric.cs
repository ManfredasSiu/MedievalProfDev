using Assets.Grid.Scripts.Map.Logic;
using Assets.Scripts.Map.Logic;
using UnityEngine;

public class HeatMapVisualGeneric : MonoBehaviour 
{
    private GenericGrid<HeatMapGridObject> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake() 
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(GenericGrid<HeatMapGridObject> grid) 
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChangedEvent += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(int x, int y)
    {
        //UpdateHeatMapVisual();
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.Width * grid.Height, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.Width; x++) 
        {
            for (int y = 0; y < grid.Height; y++) 
            {
                int index = x * grid.Height + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.CellSize;

                var gridValue = grid.GetGridObject(x, y);
                float gridValueNormalized = gridValue.GetValueNormalized();
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}
