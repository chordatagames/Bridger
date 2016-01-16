using UnityEngine;
using System.Collections;
using System;

public class GridGUI : MonoBehaviour
{
    public RectTransform gridObject;
    public float pass { get { return Grid.gridSize; } set { pass = value; } }
	public bool grid;
	public Color gridColor, secondaryColor;
	public int strongLineSpace = 5;

	public Material lineMaterial;
	private Camera cam;

	private Vector2 origin;
	private Vector2 gridSize;


	void Start()
	{
		LoadGridInfo();
	}

	void LoadGridInfo()
	{
		cam = GetComponent<Camera>();
		origin = Vector2.one*Grid.gridSize;
		gridSize = new Vector2(4.0f,2.0f) * cam.orthographicSize;
	}

	void OnPostRender()
	{
		GL.PushMatrix();
		lineMaterial.SetPass(0);
		GL.LoadOrtho();
		GL.LoadProjectionMatrix(cam.projectionMatrix);
		GL.modelview = cam.worldToCameraMatrix;
		GL.Begin(GL.LINES);

		// begin grid
		if (grid)
		{
			// Vertical Lines
			int lineCountX = 0;
			for (float i = origin.x - (gridSize.x / 2 + pass); i <= origin.x + (gridSize.x / 2 - pass); i += pass)
			{
				GL.Color( ((lineCountX % strongLineSpace == 0) ? secondaryColor : gridColor) );
				GL.Vertex(new Vector3(i,-gridSize.y / 2, -cam.transform.position.z) + cam.transform.position);
				GL.Vertex(new Vector3(i, gridSize.y / 2, -cam.transform.position.z) + cam.transform.position);
				lineCountX++;
			}

			// Horizontal Lines
			int lineCountY = 0;
			for (float i = origin.y - (gridSize.y / 2 + pass); i <= origin.y + (gridSize.y / 2 - pass); i += pass)
			{
				GL.Color( ((lineCountY % strongLineSpace == 0) ? secondaryColor : gridColor) );
				GL.Vertex(new Vector3(-gridSize.x / 2, i, -cam.transform.position.z) + cam.transform.position);
				GL.Vertex(new Vector3( gridSize.x / 2, i, -cam.transform.position.z) + cam.transform.position);
				lineCountY++;
			}
			
		}
        // end grid

        // begin border - i wish there was a better way to do this.
        Rect bounds = gridObject.rect;
        bounds.position += (Vector2)gridObject.position;
		GL.Color(Color.red);
		
		// they have to be done like this because they need to be above the grid. bleh
		GL.Vertex(new Vector3(bounds.position.x, bounds.position.y, -1));
		GL.Vertex(new Vector3(bounds.position.x, bounds.position.y + bounds.size.y, -1));

		GL.Vertex(new Vector3(bounds.position.x, bounds.position.y + bounds.size.y, -1));
		GL.Vertex(new Vector3(bounds.position.x + bounds.size.x, bounds.position.y + bounds.size.y, -1));

		GL.Vertex(new Vector3(bounds.position.x + bounds.size.x, bounds.position.y + bounds.size.y, -1));
		GL.Vertex(new Vector3(bounds.position.x + bounds.size.x, bounds.position.y, -1));

		GL.Vertex(new Vector3(bounds.position.x + bounds.size.x, bounds.position.y, -1));
		GL.Vertex(new Vector3(bounds.position.x, bounds.position.y, -1));
		// end border

		GL.End();
		GL.PopMatrix();
	}

	//Call this whenever you change the camera after initiating this script
	public void UpdateGrid()
	{
		LoadGridInfo ();
	}
}