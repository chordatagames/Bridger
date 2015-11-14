using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public abstract class LowPolyMesh : MonoBehaviour //TODO set up pre-runtime generating
{
	protected MeshFilter filter;
	protected MeshRenderer rend;
	protected MeshCollider col;
	
	protected int resolution;
	
	public float mapScale = 1f;
	public float sampleScale = 1f;
	public float height = 60f;
	
	public Gradient colorPallet; //normalized by height
	public float colorRandomness;
	
	protected float mapRelation;
	
	protected Mesh mesh;
	protected Vector3[] verts;
	protected int[] triangles;
	protected Color[] colors;

	protected int index = 0;
	protected Vector2 pos = Vector2.zero;
	
	public virtual void CreateMesh()
	{	
		mesh = new Mesh();
		filter = GetComponent<MeshFilter>();
		filter.mesh = mesh;

		col = GetComponent<MeshCollider>();
		col.sharedMesh = mesh;
		
		triangles = new int[Mathf.RoundToInt(resolution * resolution*6)]; //two triangles per sample
		verts = new Vector3[Mathf.RoundToInt(resolution * resolution*6)];
		colors = new Color[Mathf.RoundToInt(resolution * resolution*6)];

		Generate();
		index = 0;
		pos = Vector2.zero;
	}

	void Generate()
	{
		while(pos.x != resolution)
		{
			while (pos.y != resolution)
			{
				AssignSample(); //[0,0] 0
				
				IteratorMove(0, 1);
				AssignSample(); //[0,1] 1
				
				IteratorMove(1, -1);
				AssignSample(); //[1,0] 2
				
				//NEW TRIANGLE
				IteratorMove(0, 0);
				AssignSample(); //[1,0] 0
				
				IteratorMove(-1, 1);
				AssignSample(); //[0,1] 1
				
				IteratorMove(1, 0);
				AssignSample(); //[1,1] 2
				
				//NEW TRIANGLE
				IteratorMove(-1, 0);
			}
			pos += -Vector2.up*resolution + Vector2.right;
		}
		mesh.vertices = verts;
		mesh.triangles = triangles;
		CompleteMesh();
	}

	public virtual void ReloadColor()
	{
		Vector3 trianglePos = Vector3.zero;
		float highest=0, lowest=float.MaxValue;

		for (int vert = 2; vert < verts.Length; vert+=3)
		{
			trianglePos = (verts[vert-2] + verts[vert-1] + verts[vert])/3;
			
			for(int i=0; i < 3; i++)
			{
				lowest = (verts[vert-i].y < lowest) ? verts[vert-i].y : lowest;
				highest = (verts[vert-i].y > highest) ? verts[vert-i].y : highest;
			}
			colors[vert] = colorPallet.Evaluate((trianglePos.y+Random.value*colorRandomness)/height);
			colors[vert-1] = colors[vert];
			colors[vert-2] = colors[vert];
			
		}
		mesh.colors = colors;
	}

	protected abstract void CompleteMesh();

	protected virtual float GetHeight(int x, int y)
	{
		return 0;
	}

	void AssignSample()
	{
		verts[index] = new Vector3(pos.x - resolution*0.5f, GetHeight((int)pos.x, (int)pos.y), pos.y - resolution*0.5f)*sampleScale*mapScale;
		triangles[index] = index;
	}

	void IteratorMove(int x, int y)
	{
		pos += new Vector2(x, y);
		index++;
	}

//	protected struct VertexTriangle
//	{
//		public static readonly VertexTriangle bottom = new VertexTriangle(0,1,0);
//
//		IntegerPoint2 v0, v1, v2;
//
//		private VertexTriangle(bool lower) : this(( lower ? IntegerPoint2.zero : IntegerPoint2.right), lower)
//		{}
//
//		public VertexTriangle(IntegerPoint2 iteratorPosition, bool lower)
//		{
//			v0 = iteratorPosition;
//			v1 = v0 + (lower ? IntegerPoint2.up : IntegerPoint2.up-IntegerPoint2.right);
//			v2 = v
//		}
//	}

	protected struct IntegerPoint2
	{
		/// <summary>
		/// Zero (1,1)
		/// </summary>
		public static readonly IntegerPoint2 zero = new IntegerPoint2(0,0);
		/// <summary>
		/// One (1,1)
		/// </summary>
		public static readonly IntegerPoint2 one = new IntegerPoint2(1,1);
		/// <summary>
		/// Up (0,1)
		/// </summary>
		public static readonly IntegerPoint2 up = new IntegerPoint2(0,1);
		/// <summary>
		/// Right (1,0)
		/// </summary>
		public static readonly IntegerPoint2 right = new IntegerPoint2(1,0);

		int x, y;

		public IntegerPoint2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public override string ToString ()
		{
			return string.Format("({0}, {1})", x, y);
		}

		public static IntegerPoint2 operator +(IntegerPoint2 a, IntegerPoint2 b)
		{
			return new IntegerPoint2(a.x + b.x, a.y + b.y);
		}

		public static IntegerPoint2 operator - (IntegerPoint2 a, IntegerPoint2 b)
		{
			return new IntegerPoint2(a.x - b.x, a.y - b.y);
		}

		public static IntegerPoint2 operator *(IntegerPoint2 a, float b)
		{
			return new IntegerPoint2(a.x * Mathf.RoundToInt(b), a.y * Mathf.RoundToInt(b));
		}

		public static IntegerPoint2 operator *(IntegerPoint2 a, int b)
		{
			return new IntegerPoint2(a.x * b, a.y * b);
		}
	}

	protected struct IntegerPoint3
	{
		/// <summary>
		/// Zero (0, 0, 0)
		/// </summary>
		public static readonly IntegerPoint3 zero = new IntegerPoint3(0, 0, 0);
		/// <summary>
		/// One (1, 1, 1)
		/// </summary>
		public static readonly IntegerPoint3 one = new IntegerPoint3(1, 1, 1);
		/// <summary>
		/// Up (0, 1, 0)
		/// </summary>
		public static readonly IntegerPoint3 up = new IntegerPoint3(0, 1, 0);
		/// <summary>
		/// Right (1, 0, 0)
		/// </summary>
		public static readonly IntegerPoint3 right = new IntegerPoint3(1, 0, 0);
		/// <summary>
		/// Forward (0, 0, 1)
		/// </summary>
		public static readonly IntegerPoint3 forward = new IntegerPoint3(0, 0, 1);

		int x, y, z;

		public IntegerPoint3(int x, int y) : this(x,y,0)
		{}

		public IntegerPoint3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString ()
		{
			return string.Format("({0}, {1}, {2})", x, y, z);
		}

		public static IntegerPoint3 operator + (IntegerPoint3 a, IntegerPoint3 b)
		{
			return new IntegerPoint3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static IntegerPoint3 operator - (IntegerPoint3 a, IntegerPoint3 b)
		{
			return new IntegerPoint3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
	}
}
