using UnityEngine;

namespace SpaceScrapper.UserInterface
{
	public sealed class UIWidget : MonoBehaviour
	{

		private UIController controller;

		[SerializeField, Tooltip("The identifier with which the reference will register itself in the parent controller.")]
		private string identifier;


		public string ID => identifier;
		public bool Valid => !string.IsNullOrEmpty(ID);


		private void OnEnable()
		{
			GetController();
			if (controller != null)
			{
				controller.Register(this);
			}
		}

		private void OnDisable()
		{
			if (controller != null)
			{
				controller.Unregister(this);
			}
		}

		private void GetController()
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