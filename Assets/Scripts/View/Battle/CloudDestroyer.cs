using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CloudDestroyer : MonoBehaviour
{
	public List<string> tags;

	public CloudSpawner objectSpawner;

	void OnTriggerEnter(Collider other)
	{
		if(tags.Contains(other.gameObject.tag))
		{
			objectSpawner.Remove(other.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(tags.Contains(other.gameObject.tag))
		{
			objectSpawner.Remove(other.gameObject);
		}
	}
}
