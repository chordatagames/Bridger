using UnityEngine;
using System;
using System.Collections.Generic;
namespace Bridger
{
	public static class Level
	{
		public static IResetable currentItem{ get{return (levelObjects.Count > 0) ? levelObjects[_levelObjectIndex] : null;} }
		public static IResetable lastItem{ get{return _lastItem;} }

		static int _levelObjectIndex = 0;
		static IResetable _lastItem;
		static List<IResetable> levelObjects = new List<IResetable>();

		
		public static void AddToLevel(IResetable part)
		{
			_lastItem = currentItem;
			levelObjects.Add(part);
			_levelObjectIndex = levelObjects.Count-1;
		}
		
		public static void StartPhysics()
		{
			foreach(IResetable part in levelObjects)
			{
				part.StartPhysics();
			}
		}
		public static void Reload()
		{
			foreach(IResetable part in levelObjects)
			{
				part.Reset();
			}
		}
		public static void Clear()
		{
			foreach(IResetable part in levelObjects)
			{
				Undo(part);
			}
		}

		public static void Undo(IResetable part)
		{
			Debug.Log("Pooling");
			if(part.Pool())
			{
				_lastItem = currentItem;
				_levelObjectIndex--;
			}
		}

		public static void Redo(IResetable part)
		{
			Debug.Log("Unpooling");
			if(part.UnPool())
			{
				_lastItem = currentItem;
				_levelObjectIndex++;
			}
		}

//		static public void Undo(GameObject removeObject)
//		{
//			//FIXME if part is connected by another part (i.e. connectedBody of another part) this will cause problems //FIXED?
//			IResetable<MonoBehaviour> removePart;
//			if(levelObjects.TryGetValue(removeObject, out removePart))
//			{
//				if(removePart.GetType().Equals(typeof(BridgePart)))
//				{
//					foreach (BridgeJoint joint in ((BridgePart)removePart).connectedTo)
//					{
//						((BridgePart)removePart).Detatch(joint);
//					}
//				}
//			}
//
//			levelObjectStack.Remove(removeObject);
//			levelObjects.Remove(removeObject);
//			GameObject.Destroy(removeObject);
//		}
//
//		public static void UndoLast()
//		{
//			if(levelObjectStack.Count > 0)
//			{
//				Undo(levelObjectStack.ToArray()[levelObjectStack.Count-1]);
//			}
//		}
	}
	[System.Serializable]
	public struct TransformData
	{
		public Vector3 localPosition, localRotation, localScale;

		public TransformData(Transform t)
		{
			localPosition 	= t.localPosition;
			localRotation 	= t.localRotation.eulerAngles;
			localScale 		= t.localScale;
		}

		public void Reload(Transform t)
		{
			t.localPosition = this.localPosition;
			t.localRotation	= Quaternion.Euler(localRotation);
			t.localScale	= this.localScale;
		}

	}
	/// <summary>
	/// I resetable.
	/// </summary>
	public interface IResetable
	{
		void Reset();
		void StartPhysics();
		bool UnPool();
		bool Pool();
	}
}

