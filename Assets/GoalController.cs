using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Bridger
{
	public class GoalController : MonoBehaviour
	{
		public GoalZone[] goals;
		private bool _completed;
		public bool completed{ get{return _completed;} }

		public Animator completedAnimation;

		void Update()
		{
			if(Level.mode == Level.LevelMode.PLAY)
			{
				_completed = !Level.completed; //this is to only make the level complete once, if not complete, temporarely set to true
				foreach(GoalZone goal in goals)
				{
					_completed &= goal.completed;
				}

				if(_completed)
				{
					CompleteLevel();
				}
			}
		}

		public void CompleteLevel()
		{
			Level.Complete();
			if(completedAnimation != null)
			{
				completedAnimation.SetBool("open", _completed);
			}

		}
	}
}
