using UnityEngine;

public class WeaponInfo : MonoBehaviour {
	
	public float BulletDamage = 10f;
	public float BulletSpeed = 100f;
	public float ShootDelay = 0.15f;
	public bool HaveAim = true;
	public int MaxBullet = 30;

	[Header("Attachments Setting")]
	public Transform ShootPosition;
	public Transform Muzzle;
	public Transform Sight;
	public Transform Magazine;
}
