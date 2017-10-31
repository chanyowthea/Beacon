using UnityEngine;
using System.Collections;

public class MapNode : MonoBehaviour
{
	public int _id; 
	public SpriteRenderer _sprite; 
	public Animator _anim; 
	public int _posIndex; 

	public void Init()
	{
		
	}

	public void Clear()
	{
		_id = 0; 
	}
}
