using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper.UserInterface
{
	/// <summary>
	/// Base class for managing <see cref="UIReference"/>s and providing functionality to UI elements
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class UIController : MonoBehaviour
	{
		private Dictionary<string, Component> elements = new Dictionary<string, Component> ();


		private void Start ()
		{
			if (transform.root.TryGetComponent (out UIManager uiManager))
			{
				uiManager.Register (this);
			}
		}


		public bool TryGet<T> (string identifier, out T reference) where T : Component
		{
			if (elements.TryGetValue (identifier, out Component c))
			{
				reference = c as T;
				return reference != null;
			}

			reference = null;
			return false;
		}


		internal void Register (UIReference reference)
		{
			if (elements == null)
			{
				elements = new Dictionary<string, Component> ();
			}

			if (reference == null || !reference.Valid)
				return;


			if (reference.Prioritize)
			{
				elements[reference.ID] = reference.Reference;
			}
			else
			{
				elements.TryAdd (reference.ID, reference.Reference);
			}
		}

		internal void Unregister (UIReference reference)
		{
			if (reference == null || !reference.Valid)
				return;


			if (elements.TryGetValue (reference.ID, out Component comp) && reference.Reference == comp)
			{
				elements.Remove (reference.ID);
			}
		}

	}
}