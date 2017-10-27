using UnityEngine;
using System.Collections;

public class TestView : MonoBehaviour
{
	[SerializeField] Animation _anim; 
	void Start()
	{
		_anim.CrossFade("Pit"); 
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			_anim.Stop(); 
			_anim.Play();
		}
	}
}
