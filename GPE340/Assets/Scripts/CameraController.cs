using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public Transform player;

	public Vector3 offset;

	private void Update()
	{
		if (player != null)
		{
			//Turn
			transform.position = Vector3.Lerp(transform.position,player.transform.position + offset,Time.deltaTime);
		}
	}
}
