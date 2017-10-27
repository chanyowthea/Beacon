using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum EPlotStatus
{
	Start,
	//	Meet_Before,
	Meet,
	Battle_Before,
	Battle,
	End,
}

public class PlotManager : MonoBehaviour
{
	public static PlotManager instance{ private set; get; }

	public static EPlotStatus status;
	//	public static int curProgress;
	void Awake()
	{
		instance = this; 
	}

	public void Clear()
	{
		CoroutineUtil.StopAll(); 
	}

	public void Meet(ERole roleIdent, Action onFinish)
	{
		_onPlotFinish = () =>
		{
			if (onFinish != null)
			{
				onFinish(); 
			}
			UIManager._Instance.SetSysMsgInfo(SystemMessage._openLightMode); 
			_addPlayerRoutine = AddPlayerRoutine(roleIdent);
			StartCoroutine(_addPlayerRoutine); 
			Player._Instance.transform.eulerAngles = Vector3.zero;
			GameData._CanRotateCamera = false; 
			UIManager._Instance.SetMaskEnable(false); 
			PlotManager.status = EPlotStatus.Meet; 

			// 对话结束，销毁地图上的NPC
			var pos = MapManager.GetPos(MapCode.NPC_GRAND_DAUGHTER); 
			MapManager.ResetMap(MapManager.CurIndex(pos._x, pos._y)); 
		}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._Instance._plot_Meet); 
		CoroutineUtil.Start(_plotRoutine); 
	}

	public void Battle(Action onFinish)
	{
		bool hasGrandDaughter = GameData._isGrandDaughterInQueue; 
		_onPlotFinish = () =>
		{
			if (onFinish != null)
			{
				onFinish(); 
			}
				if(hasGrandDaughter)
				{
					UIManager._Instance.SetMaskEnable(true); 
					UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._removePlayer, GameData._Instance._roleLib.GetRole(ERole.GrandDaughter)._name)); 
					GameData._isGrandDaughterInQueue = false; 
					// 孙女离队，老人失去方向感
					MapManager.ResetMap(48, MapCode.NPC_GRAND_DAUGHTER, true); // TODO 写死位置不好吧
				}
				else
				{
					// 如果这里没有孙女那么就去除孙女的NPC信息
					Singleton._npcManager.RemoveNPC(ERole.GrandDaughter);
				}
			GameData._CanRotateCamera = true; 
			PlotManager.status = EPlotStatus.Battle; 
		}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, hasGrandDaughter ? GameData._Instance._plot_Battle :
			GameData._Instance._plot_Battle_NoGrandDaughter); 
		CoroutineUtil.Start(_plotRoutine);
	}

	public void Allure(Action onFinish, int index)
	{
		var allures = GameData._Instance._plot_Allures;
		if (allures.Length <= index)
		{
			return; 
		}
		_onPlotFinish = () =>
		{
			if (onFinish != null)
			{
				onFinish(); 
			}
		}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, allures[index]); 
		CoroutineUtil.Start(_plotRoutine); 
	}

	public void Die_DarkPrince(Action onFinish)
	{
		_onPlotFinish = () =>
		{
			if (onFinish != null)
			{
				onFinish(); 
			}
		}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._Instance._plot_DarkPrinceDie); 
		CoroutineUtil.Start(_plotRoutine); 
	}

	public void GrandDaughterDie(Action onFinish)
	{
		_onPlotFinish = () =>
			{
				if (onFinish != null)
				{
					onFinish(); 
				}
			}; 
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._Instance._plot_GrandDaughterDie); 
		CoroutineUtil.Start(_plotRoutine); 
	}

	public void BattleAfter(Action onFinish)
	{
		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._isGrandDaughterInQueue ? GameData._Instance._plot_BattleAfter : 
			GameData._Instance._plot_BattleAfter_NoGrandDaughter, onFinish); 

//		_plotRoutine = PlotRoutine(ERole.Grandpa, UIManager._Instance.SetTipInfo, GameData._Instance._plot_BattleAfter, onFinish); 
		CoroutineUtil.Start(_plotRoutine); 
	}

	public void _Start(Action onFinish)
	{
		UIManager._Instance.SetMaskEnable(true); 
		_onPlotFinish = () =>
		{
			if (onFinish != null)
			{
				onFinish(); 
			}
			//				UIManager._Instance.SetMaskEnable(false); 
			PlotManager.status = EPlotStatus.Start; 
		}; 
		_plotRoutine = PlotRoutine(ERole.None, UIManager._Instance.SetPlotInfo, GameData._Instance._plot_Start); 
		CoroutineUtil.Start(_plotRoutine); 

		#if TEST
//		_onPlotFinish();
		#endif
	}

	IEnumerator _plotRoutine;
	Action _onPlotFinish;
	IEnumerator _addPlayerRoutine;

	IEnumerator PlotRoutine(ERole ident, Action<string> onPlot, PlotConf conf, Action onFinish = null)
	{
		if (conf._triggerRoleIdent != ident)
		{
			ClearMeetRoutine(); 
			yield break; 
		}

		List<SingleConvers> converses = new List<SingleConvers>(conf.conversList); 
		while (converses.Count > 0)
		{
			RoleConf role = GameData._Instance._roleLib.GetRole(converses[0]._roleIdent); 
			if (role == null)
			{
				converses.RemoveAt(0); 
				continue; 
			}

			string s = role._name + (string.IsNullOrEmpty(role._name) ? "" : "：") + converses[0]._convers; 
			if (onPlot != null)
			{
				onPlot(s); 
			}

			// there is no need to wait if the length of conversation is zero. 
			converses.RemoveAt(0); 
			if (converses.Count == 0)
			{
				if (onFinish != null)
				{
					onFinish();
				} 
				// 必须加下面的语句，不然最后一句无法播放
				yield return new WaitForSeconds(GameData._Instance._conversSpeed); 
				ClearMeetRoutine(); 
				break; 
			}
			yield return new WaitForSeconds(GameData._Instance._conversSpeed); 
		}
	}

	void ClearMeet()
	{
		if (_plotRoutine != null)
		{
			CoroutineUtil.Stop(_plotRoutine); 
			ClearMeetRoutine(); 
		}
		if (_addPlayerRoutine != null)
		{
			CoroutineUtil.Stop(_addPlayerRoutine); 
			ClearAddPlayerRoutine(); 
		}
	}

	void ClearMeetRoutine()
	{
		if (_onPlotFinish != null)
		{
			_onPlotFinish(); 
			_onPlotFinish = null; 
		}
		_plotRoutine = null; 
	}

	IEnumerator AddPlayerRoutine(ERole roleIdent)
	{
		yield return new WaitForSeconds(3); 
		UIManager._Instance.SetSysMsgInfo(string.Format(SystemMessage._addPlayer, 
				GameData._Instance._roleLib.GetRole(roleIdent)._name)); 
		GameData._isGrandDaughterInQueue = true; 
	}

	void ClearAddPlayerRoutine()
	{
		_addPlayerRoutine = null; 
	}
}
