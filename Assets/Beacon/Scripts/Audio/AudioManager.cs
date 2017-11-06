using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioSource _walk; 
	[SerializeField] AudioSource _attack; 
	[SerializeField] AudioSource _cannotMove; 

	[SerializeField] AudioSource _wind; 
	[SerializeField] AudioSource _title; 
	[SerializeField] AudioSource _boss; 
	[SerializeField] AudioSource _defeat;
	[SerializeField] AudioSource _run; 

	public void Walk()
	{
		_walk.Play(); 
	}

	public void Attack()
	{
		_attack.Play(); 
	}

	public void CannotMove()
	{
		_cannotMove.Play(); 
	}

	public void PlayerTitle(bool value)
	{
		if (value)
		{
			_title.Play();
		}
		else
		{
			_title.Stop(); 
		}
	}

	public void PlayWind(bool value)
	{
		if (value)
		{
			_wind.Play(); 
		}
		else
		{
			_wind.Stop(); 
		}
	}

	public void PlayBoss(bool value)
	{
		if (value)
		{
			_boss.Play(); 
		}
		else
		{
			_boss.Stop(); 
		}
	}

	public void PlayDefeat(bool value)
	{
		if (value)
		{
			_defeat.Play(); 
		}
		else
		{
			_defeat.Stop(); 
		}
	}

	public void PlayRun(bool value)
	{
		if (value)
		{
			_run.Play(); 
		}
		else
		{
			_run.Stop(); 
		}
	}
}
