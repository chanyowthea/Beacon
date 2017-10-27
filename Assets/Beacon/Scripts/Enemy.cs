using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	protected IMove _iMove = new RandomMove(); 
	protected IHP _iHP = new NormalHP(); 

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

	IEnumerator UpdateRoutine()
	{
		System.Random random = new System.Random(); 
		Pos[] coordinates = new Pos[4]{new Pos(-1, 0), new Pos(1, 0), new Pos(0, -1), new Pos(0, 1)}; 

		while(true)
		{
			yield return null; 
			if ((int)(random.Next(50)) == 0)
//			if(Input.GetKey(KeyCode.Space))
			{
				Debug.Log("Enemy Move"); 

				int index = random.Next(0, 4); 
				Pos pos = coordinates[index]; 
				_iMove.Move(pos._x, pos._y); 
			}
		}
	}

	void OnDestroy()
	{
		Clear(); 
	}
}
