using UnityEngine;
using System.Collections;
using System;

public interface IHP
{
	int _MaxHP { get; set; } 
	int _curHP { get; set; } 
	void Init(IDie iDie, Transform transform, int maxHP); 
	void Reset(int hp); 
	void Clear(); 
	void Cure(int value); 
	void Hurt(int value); 
}

public class NormalHP : IHP
{
	Transform _hpParent; 
	public int _MaxHP{ get; set; } 
	const float _hpDisplayGap = 0.17f; 
	public int _curHP{ get; set; } 
	IDie _iDie; 
	Transform _transform; 

	public void Init(IDie iDie, Transform transform, int maxHP)
	{
		_MaxHP = maxHP; 
		_iDie = iDie; 
		_transform = transform; 
		Reset(_MaxHP); 
	}

	public void Clear()
	{
		GameObject.Destroy(_hpParent.gameObject);
	}

	public void Reset(int hp)
	{
		Debug.Log("Enemy SetHP: " + hp); 
		if (hp > _MaxHP)
		{
			return; 
		}
		if (hp < 0)
		{
			_iDie.Die(_transform.localPosition.ToPos()); 
			return; 
		}
		if (_hpParent != null)
		{
			GameObject.Destroy(_hpParent.gameObject); 
		}
		_hpParent = (new GameObject()).transform; 
		_hpParent.name = "HP Parent"; // TODO need to be optimized
		_hpParent.SetParent(_transform); 
		_hpParent.localPosition = Vector3.zero; 
		_hpParent.localEulerAngles = Vector3.zero; 
		float x = 0 - (int)(_MaxHP / 2f) * _hpDisplayGap; 
		float y = 0.35f; 
		for (int i = 0; i < hp; ++i)
		{
			Transform tf = GameObject.Instantiate(GameData._Instance._hitPointPrefab); 
			tf.SetParent(_hpParent); 
			tf.localPosition = new Vector3(x, y); 
			x += _hpDisplayGap; 
		}
		_curHP = hp; 

		// 先扣血再说，然后判定是否死亡
		if (hp == 0)
		{
			_iDie.Die(_transform.localPosition.ToPos()); 
			return; 
		}
	}

	/// <summary>
	/// 受到多少点伤害
	/// </summary>
	public void Hurt(int value)
	{
		Reset(Mathf.Clamp(_curHP - value, 0, _MaxHP)); 
	}	

	public void Cure(int value)
	{
		Reset(Mathf.Clamp(_curHP + value, 0, _MaxHP)); 
	}
}
