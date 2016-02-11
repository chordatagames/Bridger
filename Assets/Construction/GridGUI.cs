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
		origin = gridObject.position;
		gridSize = gridObject.rect.size;

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
			
			int lineCountX = 0;
			for (float x = origin.x - gridSize.x * 0.5f; x < origin.x + gridSize.x * 0.5f; x += pass)
			{
				
				int lineCountY = 0;
				for (float y = origin.y + gridSize.y * 0.5f; y > origin.y - gridSize.y * 0.5f; y -= pass)
				{
					// Vertical Lines
					GL.Color( ((lineCountX % strongLineSpace == 0) ? secondaryColor : gridColor) );
					GL.Vertex(new Vector3(x, y) );
					GL.Vertex(new Vector3(x, origin.y - gridSize.y * 0.5f));

					// Horizontal Lines
					GL.Color( ((lineCountY % strongLineSpace == 0) ? secondaryColor : gridColor) );
					GL.Vertex(new Vector3(x, y));
					GL.Vertex(new Vector3(origin.x + gridSize.x * 0.5f, y));

					lineCountY++;
				}
				lineCountX++;
			}


			
		}
        // end grid

        // begin border - i wish there was a better way to do this.
        Rect bounds = gridObject.rect;
        bounds.position += (Vector2)gridObject.position;
		GL.Color(Color.red);
		
		// they have to be done like this because they need to be above the grid. bleh
		GL.Vertex(new Vector3(bounds.position.x, bounds.position.y, -1 ));
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