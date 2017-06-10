using UnityEngine;

public class WeaponInfo : MonoBehaviour {

	[Header("Weapon Setting")]
	public bool HaveAim = true;
	public Vector3 AimPosition = Vector3.zero;
	public Vector3 NotAimPosition = Vector3.zero;
	public float AimDuration = 1f;

	public float BulletDamage = 10f;
	public float BulletSpeed = 100f;
	public float ShootDelay = 0.15f;
	public int MaxBullet = 30;

	[Header("Attachments Setting")]
	public Transform ShootPosition;
	public Transform Muzzle;
	public Transform Sight;
	public Transform Magazine;
}
