using UnityEngine;
using System.Collections;

public class DarkPrince : NPC
{
	public bool _startMove; 
	IHP _iHP = new NormalHP(); 
	IDie _iDie = new NormalDie(); 
	IMove _iMove = new AIMove(); 
	public void Allure(IAllurable target)
	{
		if (target != null)
		{
			++target.allureCount; 
		}
	}

	public override void Init(ERole roleIdent, Pos pos, Sprite[] moveSprites)
	{
		IDie die = new NormalDie();
		die.onDie = () =>
			{
				Player._Instance.isLockMove = true; 
				PlotManager.instance.Die_DarkPrince(() =>
					{
						GameData._isLockDoor = false; 
						if(GameData._isGrandDaughterInQueue)
						{
							GameData._CanRotateCamera = false; 
							UIManager._Instance.SetMaskEnable(false); 
						}

						PlotManager.instance.BattleAfter(() =>
							{
								// 此处有问题，应该是在上面显示，但是由于这个栈里面下面有一句也是弹出SystemMessage，因此把这个顶掉了
								UIManager._Instance.SetSysMsgInfo(SystemMessage._getKey);
								Player._Instance.isLockMove = false; 

								// 从NPC表中移除NPC
								Singleton._npcManager.RemoveNPC(ERole.DarkPrince); 
							}); 
					}); 
		};
		_iHP.Init(die, transform, 5); 
		var move = (AIMove)_iMove; 
		move.onMove = (Pos dirPos) =>
		{
				// 黑王子移动的时候也要提示
				if(!GameData._isGrandDaughterInQueue && !GameData._isGrandDaughterRebel)
				{
					MoveUtil.ShowDangerTip(Player._Instance.GetPos()); 	
				}
		}; 
		move.onAllure = () =>
		{
			Allure(Singleton._npcManager.GetNPC<GrandDaughter>(ERole.GrandDaughter)); 
		}; 
		_iMove.Init(pos, transform); 
		move._followTarget = Player._Instance.transform; 
		base.Init(roleIdent, pos, moveSprites);
	}

	public override void Clear()
	{
		base.Clear();
		_iHP.Clear();
		_iMove.Clear(); 
	}

	void Update()
	{
		if (_iMove == null || !_startMove)
		{
			return; 
		}
		var move = (AIMove)_iMove; 
		if (move != null)
		{
			move.Update(); 
		}
	}

	public void Hurt(int value)
	{
		_iHP.Hurt(value); 
	}
}
