using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float sensitivity = 1f;
	[SerializeField] private float clamp = 10f;

	// wanna change cinemachine virtual camera - aim - tracked object offset - Y value
	private CinemachineComposer composer;

    // Start is called before the first frame update
    private void Start()
    {
		composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

	// Update is called once per frame
	private void Update()
    {
		float vertical = Input.GetAxis("Mouse Y") * sensitivity;
		composer.m_TrackedObjectOffset.y += vertical;
		composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -clamp, clamp);
    }
}
