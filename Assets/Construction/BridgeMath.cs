using UnityEngine;

public static class BridgeMath
{
	public static bool LineIntersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
	{
		intersection = Vector2.zero;

		Vector2 b = a2 - a1;
		Vector2 d = b2 - b1;
		float bDotDPerp = b.x * d.y - b.y * d.x;

		// if b dot d == 0, it means the lines are parallel so have infinite intersection points
		if(bDotDPerp == 0)
			return false;

		Vector2 c = b1 - a1;
		float t = ( c.x * d.y - c.y * d.x ) / bDotDPerp;
		if(t < 0 || t > 1)
			return false;

		float u = ( c.x * b.y - c.y * b.x ) / bDotDPerp;
		if(u < 0 || u > 1)
			return false;

		intersection = a1 + t * b;

		return true;
	}

	public static bool RectangleIntersects(Vector2 a1, Vector2 a2, Rect rect, out Vector2 intersection)
	{
		intersection = Vector2.zero;
		
		Vector2 leftBottom = new Vector2(rect.xMin, rect.yMin);
		Vector2 leftTop = new Vector2(rect.xMin, rect.yMax);
		Vector2 rightBottom = new Vector2(rect.xMax, rect.yMin);
		Vector2 rightTop = new Vector2(rect.xMax, rect.yMax);
		//left side
		if(LineIntersects(a1, a2, leftBottom, leftTop, out intersection))
		{ return true; }
		//Top side
		if(LineIntersects(a1, a2, rightTop, leftTop, out intersection))
		{ return true; }
		//Right side
		if(LineIntersects(a1, a2, rightTop, rightBottom, out intersection))
		{ return true; }
		//Bottom side
		if(LineIntersects(a1, a2, rightBottom, leftBottom, out intersection))
		{ return true; }

		return false;
	}
}