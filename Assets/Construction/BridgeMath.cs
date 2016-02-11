using UnityEngine;
using System.Collections;

public static class BridgeMath
{
	public static Vector2 ProjectPointOnLine(Vector2 a, Vector2 b, Vector2 point)
	{

		//get vector from point on line to point in space
		Vector2 projectionLine = point - a;

		float t = Vector3.Dot(projectionLine, (b - a));

		return a + (b - a) * t;
	}

	//public static Vector2 ProjectPointOnLineSegment(Vector2 linePoint1, Vector2 linePoint2, Vector2 point)
	//{

	//	Vector3 vector = linePoint2 - linePoint1;

	//	Vector3 projectedPoint = ProjectPointOnLine(linePoint1, vector.normalized, point);

	//	int side = PointOnWhichSideOfLineSegment(linePoint1, linePoint2, projectedPoint);

	//	//The projected point is on the line segment
	//	if (side == 0)
	//	{

	//		return projectedPoint;
	//	}

	//	if (side == 1)
	//	{

	//		return linePoint1;
	//	}

	//	if (side == 2)
	//	{

	//		return linePoint2;
	//	}

	//	//output is invalid
	//	return Vector3.zero;
	//}
}
