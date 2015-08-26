using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Bridger
{
	public class GoalController : MonoBehaviour
	{
		public GoalZone[] goals;
		public bool completed;

		public Animator completedAnimation;

		void Update()
		{
			if(Level.mode == Level.LevelMode.PLAY)
			{
				completed = !Level.completed; //this is to only make the level complete once, if not complete, temporarely set to true
				foreach(GoalZone goal in goals)
				{
					completed &= goal.completed;
				}

				if(completed)
				{
					Level.completed = true;
				}

				if(completedAnimation != null)
				{
					completedAnimation.SetBool("open", Level.completed);
				}
			}

		}
	}
}
