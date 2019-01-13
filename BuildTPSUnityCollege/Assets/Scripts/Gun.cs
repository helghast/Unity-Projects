using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField][Range(0.1f, 1.5f)]	private float fireRate = 0.3f;
	[SerializeField][Range(1, 10)] private int damage = 1;

	/*[SerializeField]
	private Transform firePoint;*/

	[SerializeField] private ParticleSystem muzzleParticle;
	[SerializeField] private AudioSource guneFireSource;

	private float timer;
	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
	}

	// Update is called once per frame
	void Update()
    {
		timer += Time.deltaTime;
		if (timer >= fireRate)
		{
			if (Input.GetButton("Fire1"))
			{
				timer = 0f;

				FireGun();
			}
		}
    }

	private void FireGun()
	{
		//testonly
		//Debug.DrawRay(firePoint.position, firePoint.forward * 100, Color.red, 2f);

		muzzleParticle.Play();
		guneFireSource.Play();

		// v.1 shot from forward
		//Ray ray = new Ray(firePoint.position, firePoint.forward);

		// v.2 shot from reticle / center of screen
		Ray ray = mainCamera.ViewportPointToRay(Vector3.one * 0.5f);

		Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, 100f))
		{
			//testonly
			//Destroy(hitInfo.collider.gameObject); // if we hit something, and this have a collider, then destroy it

			Health health = hitInfo.collider.GetComponent<Health>();
			if (health != null)
			{
				health.TakeDamage(damage);
			}
		}
	}
}
