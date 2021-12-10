using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpaceScrapper.UserInterface
{
	/// <summary>
	/// Class for managing <see cref="UIController"/>s
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class UIManager : MonoBehaviour
	{

		private Dictionary<Type, UIController> controllers = new Dictionary<Type, UIController> ();


		private void Awake ()
		{
			// bind this to Game.SceneContext.?
		}


		public bool TryGet<T> (out T controller) where T : UIController
		{
			if (controllers == null)
			{
				controller = null;
				return false;
			}

			if (controllers.TryGetValue (typeof (T), out UIController ctrl))
			{
				controller = ctrl as T;
				return true;
			}

			controller = null;
			return false;
		}


		internal void Register<T> (T controller) where T : UIController
		{
			if (controllers == null)
			{
				controllers = new Dictionary<Type, UIController> ();
			}

			if (controller == null)
				return;


			controllers.TryAdd (typeof (T), controller);
		}

		internal void Unregister<T> (T controller) where T : UIController
		{
			if (controllers == null)
				return;


			if (controllers.TryGetValue (typeof (T), out UIController ctrl) && controller == ctrl)
			{
				controllers.Remove (typeof (T));
			}
		}

	}
}