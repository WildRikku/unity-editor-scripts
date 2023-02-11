#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.WildRikku {
	public class CloneAndMove : OdinEditorWindow
	{
		[MenuItem("GameObject/Clone and move")]
		private static void OpenWindow() {
			GetWindow<CloneAndMove>().Show();
		}

		public Vector3 displacement;
		public bool selectNewObject = true;

		[ReadOnly] public Vector3 sizeInWorld;
		[ReadOnly] public Vector3 positionInWorld;

		[Button(ButtonSizes.Gigantic)]
		public void CloneAndMoveSelectedObjects() {
			foreach (Transform transform in Selection.transforms) {
				Undo.RegisterCompleteObjectUndo(transform, transform.name + " cloned");
			
				// Get prefab and clone that or clone directly
				GameObject clonedObject;
				GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(transform.gameObject);
				if (prefabRoot != null) {
					clonedObject = PrefabUtility.InstantiatePrefab(prefabRoot, transform.parent) as GameObject;
					if (clonedObject != null) {
						clonedObject.transform.position = transform.position;
						clonedObject.transform.rotation = transform.rotation;
					}
				}
				else {
					clonedObject = Instantiate(Selection.activeGameObject, transform.position, transform.rotation, transform.parent);
				}
				if (clonedObject != null) {
					clonedObject.name = transform.name;
					clonedObject.transform.Translate(displacement);
					if (selectNewObject) {
						Selection.activeObject = clonedObject;
					}
				}
			}
		}

		void OnSelectionChange() {
			if (Selection.transforms.Length == 1) {
				Renderer renderer = Selection.transforms[0].GetComponent<Renderer>();
				if (renderer != null) {
					sizeInWorld = renderer.bounds.size;
				}
				else {
					renderer = Selection.transforms[0].GetComponentInChildren<Renderer>();
					if (renderer != null) {
						sizeInWorld = renderer.bounds.size;
					}
					else {
						sizeInWorld = Vector3.zero;
					}
				}
				positionInWorld = Selection.transforms[0].position;
			}
			else {
				sizeInWorld = Vector3.zero;
			}
		}
	}
}
#endif
