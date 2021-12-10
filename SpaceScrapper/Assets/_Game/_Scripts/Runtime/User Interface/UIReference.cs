using UnityEngine;

namespace SpaceScrapper.UserInterface
{
	public sealed class UIReference : MonoBehaviour
	{

		private UIController controller;

		[SerializeField, Tooltip ("The identifier with which the reference will register itself in the parent controller.")]
		private string identifier;
		[SerializeField, Tooltip ("If checked, this reference will replace another reference in case there is a duplicate ID.")]
		private bool prioritize;
		[SerializeField, Tooltip ("A reference to the component that will be registered.")]
		private Component reference;


		public string ID => identifier;
		public bool Prioritize => prioritize;
		public Component Reference => reference;
		public bool Valid => Reference != null && !string.IsNullOrEmpty (ID);


		private void OnEnable ()
		{
			GetController ();
			if (controller != null)
			{
				controller.Register (this);
			}
		}

		private void OnDisable ()
		{
			if (controller != null)
			{
				controller.Unregister (this);
			}
		}

		private void GetController ()
		{
			if (controller != null)
				return;

			Transform current = transform;
			while (!current.TryGetComponent(out controller) && current.parent != null)
			{
				current = current.parent;
			}
		}

	}
}