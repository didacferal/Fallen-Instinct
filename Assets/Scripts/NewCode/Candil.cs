﻿using UnityEngine;
using System.Collections;

public class Candil : MonoBehaviour 
{
	public float range = 1.0f;
	public GameObject player;
	public GameObject playerLight;
	public GameObject text;
	public static bool candil = false;

	public static int oilCounter = 100;

	void Update () 
	{
		Debug.Log(oilCounter);
		float viewDistance = Vector3.Distance(player.transform.position, transform.position);

		if (viewDistance <= range) 
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				candil = true;
				text.SetActive(true);
				this.gameObject.SetActive(false);
			}
		}

		if (PlayerLightController.enabled == true)
			oilCounter--;

		if(oilCounter <= 0)
		{
			playerLight.SetActive(false);
		}
	}
}