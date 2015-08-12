using UnityEngine;
using System;
using System.Collections.Generic;
namespace Bridger
{
	public static class Level
	{
		static List<IReloadable> levelObjects = new List<IReloadable>();

		public static IRevertable currentItem{ get{return undoStack.Peek();} }

		static Stack<IRevertable> undoStack = new Stack<IRevertable>();
		static Stack<IRevertable> redoStack = new Stack<IRevertable>();

		public static void AddToLevel(IReloadable part)
		{
			levelObjects.Add(part);
			if(part is IRevertable)
			{
				redoStack.Clear();
				undoStack.Push((IRevertable)part);
			}
		}
		
		public static void StartPhysics()
		{
			foreach(IReloadable part in levelObjects)
			{
				part.StartPhysics();
			}
		}
		public static void Reload()
		{
			foreach(IReloadable part in levelObjects)
			{
				part.Reset();
			}
		}
		public static void Clear()
		{
			foreach(IRevertable part in undoStack)
			{
				part.Remove();
				levelObjects.Remove((IReloadable)part);
			}
			undoStack.Clear();
			redoStack.Clear();
		}

		public static void Undo()
		{
			if(undoStack.Count > 0)
			{
				if(undoStack.Peek().Undo())//Sucessfully undo
				{
					redoStack.Push(undoStack.Pop());
				}
			}
		}

		public static void Redo()
		{
			if(redoStack.Count > 0)
			{
				if(redoStack.Peek().Redo())//Sucessfully redo
				{
					undoStack.Push(redoStack.Pop());
				}
			}
		}

//		static public void Undo(GameObject removeObject)
//		{
//			//FIXME if part is connected by another part (i.e. connectedBody of another part) this will cause problems //FIXED?
//			IReloadable<MonoBehaviour> removePart;
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
	public interface IReloadable
	{
		void Reset();
		void StartPhysics();

	}
	public interface IRevertable
	{
		bool Undo();
		bool Redo();
		void Remove();
	}
}

