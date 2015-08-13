using UnityEngine;
using System;
using System.Collections.Generic;
namespace Bridger
{
	public static class Level
	{
		static float timeBendFactor = 1;
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
		public static void Slowmo()
		{
			Time.timeScale *= 0.75f;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}

		public static void UnSlowmo()
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
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
	/// An interface that makes the object able to reset to it's defined original state
	/// </summary>
	public interface IReloadable
	{
		void Reset();
		void StartPhysics();
	}
	/// <summary>
	/// Implements the normal Undo/Redo/Clear functionalities 
	/// </summary>
	public interface IRevertable
	{
		bool Undo();
		bool Redo();
		void Remove();
	}
}

