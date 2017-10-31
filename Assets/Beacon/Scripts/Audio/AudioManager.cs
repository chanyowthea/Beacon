using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioSource _walk; 
	public void Walk()
	{
		_walk.Play(); 
	}
}
