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

	public static bool LinesIntersect(Vector2 a, Vector2 b, Vector2 aDir, Vector2 bDir, out Vector2? intersection)
	{
		//line1 a + t*aDir
		//line2 b + u*bDir

		Vector2 c = (b - a);

		float crossADirBDir = Vector3.Cross(aDir, bDir).magnitude;


		//t(aDir x bDir) = (b - a) x bDir

		// t = (b - a) x bDir / (aDir x bDir)

		//u(bDir x aDir) = (a - b) x aDir

		//u = (a - b) x aDir / (bDir x aDir)    => bDir x aDir = - (aDir x bDir)

		//-u = (a - b) x aDir / (aDir x bDir)

		//u = (b - a) x aDir / (aDir x bDir) 
		if (crossADirBDir == 0)
		{
				intersection = null;
				return false;
		}
		else
		{
			float t = (Vector3.Cross(c, bDir).magnitude / crossADirBDir);
			float u = (Vector3.Cross(c, aDir).magnitude / crossADirBDir);
			if ( 0 <= t && t <= 1 && 0 <= u && u <= 1)
			{
				//a + t * aDir = b + (c x aDir) / (aDir x bDir) * bDir
				


				//aDir * t + a = b * c

				//a + c x bDir / (aDir x bDir) * aDir = b + c x aDir / (aDir x bDir) * bDir | * (aDir x bDir)
				//a + c x bDir / aDir = b + c x aDir / bDir
			}
		}
		



		intersection = null;
		return false;
	}
/*public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
 
		intersection = Vector3.zero;
 
		Vector3 lineVec3 = linePoint2 - linePoint1;
		Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
 
		float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
 
		//Lines are not coplanar. Take into account rounding errors.
		if((planarFactor >= 0.00001f) || (planarFactor <= -0.00001f)){
 
			return false;
		}
 
		//Note: sqrMagnitude does x*x+y*y+z*z on the input vector.
		float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
 
		if((s >= 0.0f) && (s <= 1.0f)){
 
			intersection = linePoint1 + (lineVec1 * s);
			return true;
		}
 
		else{
			return false;       
		}
	}
	*/
	}
