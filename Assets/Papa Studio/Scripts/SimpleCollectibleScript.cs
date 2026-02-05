using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePolygon;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, Coin, Immortal, FreezeTime, Type4, Type5};

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	[Header("Coin Item Data")]
	[SerializeField] int numberCoinCollect = 10;

	[Header("Immortal Item Data")]
	[SerializeField] float immortalTime = 2f;

	[Header("Freeze Item Data")]
	[SerializeField] float freezeTime = 2f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Stick") {
			Collect ();
		}
	}

	public void Collect()
	{
		// if(collectSound)
		// 	AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		if (CollectibleType == CollectibleTypes.NoType) {

			

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Coin) {

            
			SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
            CoinManager.Instance.AddCoins(numberCoinCollect);
		}
		if (CollectibleType == CollectibleTypes.Immortal) {

			SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
			GameManager.instance.OnCollectImmortalItem(immortalTime);
		}
		if (CollectibleType == CollectibleTypes.FreezeTime) {

			SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
			GameManager.instance.OnCollectFreezeItem(freezeTime);
		}
		if (CollectibleType == CollectibleTypes.Type4) {

			

		}
		if (CollectibleType == CollectibleTypes.Type5) {

			

		}

		Destroy (gameObject);
	}
}
