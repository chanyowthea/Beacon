using UnityEngine;
using System.Collections;
using System; 

public class GrandDaughter : NPC, IAllurable
{ 
	const int maxAllureCount = 5; 

	int _allureCount; 
	public int allureCount
	{
		set
		{ 
			// 如果已经反叛，诱惑就不再生效
			if (_allureCount >= maxAllureCount)
			{
				return; 
			}
			_allureCount = value; 
			int tempValue = value; 
			Player._Instance.isLockMove = true; 
			Action a = () =>
			{
					Player._Instance.isLockMove = false; 
				if (tempValue >= maxAllureCount)
				{
					Rebel(); 
				}
			}; 
			Debug.LogError("Allure" + (value - 1)); 
			PlotManager.instance.Allure(a, value - 1); 
		}
		get
		{
			return _allureCount; 
		}
	}

	IHP _iHP = new NormalHP(); 

	public override void Init(ERole roleIdent, Pos pos, Sprite[] moveSprites)
	{
		NormalDie iDie = new NormalDie(); 
		iDie.onDie = () =>
		{
			Player._Instance.isLockMove = true; 
			Singleton._npcManager.RemoveNPC(ERole.GrandDaughter);
				PlotManager.instance.GrandDaughterDie(() =>
				{
					Debug.LogError("GrandDaughterDie"); 
					Player._Instance.isLockMove = false; 
				}); 
		}; 
		_iHP.Init(iDie, transform, 2); 
		base.Init(roleIdent, pos, moveSprites);
	}

	public override void Clear()
	{
		base.Clear();
		_iHP.Clear(); 
	}

	public void Hurt(int value)
	{
		_iHP.Hurt(value); 
	}

	void Rebel()
	{
		GameData._isGrandDaughterRebel = true; 
		UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._rebel, 
			GameData._Instance._roleLib.GetRole(_roleIdent)._name)); 
	}
}
