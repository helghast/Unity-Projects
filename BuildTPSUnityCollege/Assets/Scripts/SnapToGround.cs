using UnityEditor;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
	[MenuItem("Custom/Snap To Ground %g")]
	public static void Ground()
	{
		foreach (Transform transform in Selection.transforms)
		{
			RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 10f);
			foreach (RaycastHit hit in hits)
			{
				if (hit.collider.gameObject == transform.gameObject)
					continue;

				transform.position = new Vector3(hit.point.x, hit.point.y + (transform.gameObject.transform.localScale.y / 2), hit.point.z); //hit.point;
				break;
			}
		}
	}
}
