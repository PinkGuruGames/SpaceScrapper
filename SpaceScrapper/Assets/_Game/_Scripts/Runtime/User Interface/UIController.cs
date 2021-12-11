using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper.UserInterface
{
	/// <summary>
	/// Base class for managing <see cref="UIIndex"/>s and providing functionality to UI elements
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class UIController : MonoBehaviour
	{
		private Dictionary<string, UIIndex> elements = new Dictionary<string, UIIndex> ();


		private void Start ()
		{
			if (transform.root.TryGetComponent (out UIManager uiManager))
			{
				uiManager.Register (this);
			}
		}


		public bool TryGetByID<T> (string identifier, out T reference) where T : Component
		{
			if (elements.TryGetValue (identifier, out UIIndex uiRef))
			{
				reference = uiRef.GetComponent<T> ();
				return reference != null;
			}

			reference = null;
			return false;
		}

		public T GetByID<T> (string identifier) where T : Component
		{
			if (TryGetByID(identifier, out T value)) {
				return value;
			}
			return null;
		}


		internal void Register (UIIndex reference)
		{
			if (elements == null)
			{
				elements = new Dictionary<string, UIIndex> ();
			}

			if (reference == null || !reference.Valid)
				return;

			if (!elements.TryAdd (reference.ID, reference))
			{
				Debug.LogError ($"Duplicate UI ID: {reference.ID}");
			}
		}

		internal void Unregister (UIIndex reference)
		{
			if (reference == null || !reference.Valid)
				return;


			if (elements.TryGetValue (reference.ID, out UIIndex comp) && reference == comp)
			{
				elements.Remove (reference.ID);
			}
		}

	}
}