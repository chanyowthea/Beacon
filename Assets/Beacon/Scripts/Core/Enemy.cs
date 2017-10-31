using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public IMove _iMove = new RandomMove(); 
	public IHP _iHP = new NormalHP(); 

	public void Init(Pos pos)
	{
		_iHP.Init(new NormalDie(), transform, 2); 
		_iMove.Init(pos, transform); 
	}

	void Clear()
	{
		_iMove.Clear(); 
	}

	public void Hurt(int value)
	{
		_iHP.Hurt(value); 
	}

	void OnDestroy()
	{
		Clear(); 
	}
}
