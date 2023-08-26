using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
	[SerializeField] float moveSpeed;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] NoiseSampler bopNoise;
	[SerializeField] NoiseSampler rotationNoise;
	void Start()
	{
		
	}


	void FixedUpdate()
	{
		Vector2 playerPos = PlayerInfo.GetPlayerPosition();
		transform.position = Vector3.Lerp(transform.position, new Vector3(playerPos.x, transform.position.y, transform.position.z), moveSpeed*Time.fixedDeltaTime);

		float delta = playerPos.x - transform.position.x;
		spriteRenderer.flipX = delta > 0;

		transform.position = new Vector3(transform.position.x, bopNoise.SampleAt(transform.position), transform.position.z);
		transform.rotation = Quaternion.Euler(0,0,rotationNoise.SampleAt(transform.position));
	}
}
