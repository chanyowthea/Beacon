using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class CoroutineUtil : MonoBehaviour
{
	#region Main
	static CoroutineUtil _instance; 
	static List<IEnumerator> _routines; 

	void Awake()
	{
		_instance = this; 
		_routines = new List<IEnumerator>(); 
	}

	void OnDestroy()
	{
		StopAll();
		_routines = null; 
		_instance = null; 
	}

	public static void Start(IEnumerator routine)
	{
		if (routine == null)
		{
			return; 
		}
		Add(routine); 
	}

	public static void Stop(IEnumerator routine)
	{
		Remove(routine); 
	}

	public static void StopAll()
	{
		for (int i = _routines.Count - 1; i >= 0; --i)
		{
			Remove(_routines[i]); 
		}
	}
	#endregion


	static void Remove(IEnumerator value)
	{
		if (value == null)
		{
			return; 
		}
		_routines.Remove(value); 
		_instance.StopCoroutine(value); 
	}

	static void Add(IEnumerator value)
	{
		if (value == null)
		{
			return; 
		}
		_routines.Add(value); 
		_instance.StartCoroutine(value); 
	}
}

