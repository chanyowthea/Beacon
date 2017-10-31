using UnityEngine;
using System.Collections;

public class PlayerSaveInfo
{
	public int _curLevel; 
	public int _hp; 
	public int _posIndex; 

	// 游戏数据
	public int _curMeetHint;
	public bool _IsOpenTutorial; 
	public bool _IsShowedOpenDoorTutorial; 
	public int _Step; 

	// 角色相关
	public bool _CanRotateCamera; 
	public bool _HasRotated;
	public bool _isOpenMask; 
	public bool _isLockDoor; 
	public bool _isGrandDaughterRebel; 
	public bool _isGrandDaughterInQueue;  

	// 游戏进程
	public int _plotStatus; 

	// 补充
	public int _rotateAngle; 
	public int _allureCount; 
	public bool _isDarkPrinceStartMove; 
	public int _maxTipCount; 

	public string ToSaveString()
	{
		return string.Format("_curLevel={0}, _hp={1}, _posIndex={2}, " +
			"_curMeetHint={3}, _IsOpenTutorial={4}, _IsShowedOpenDoorTutorial={5}, _Step={6}, " +
			"_CanRotateCamera={7}, _HasRotated={8}, _isOpenMask={9}, " +
			"_isLockDoor={10}, _isGrandDaughterRebel={11}, _isGrandDaughterInQueue={12}, " +
			"_plotStatus={13}, _rotateAngle={14}, _allureCount={15}, " +
			"_isDarkPrinceStartMove={16}, _maxTipCount={17}", 
			_curLevel, _hp, _posIndex, 
			_curMeetHint, _IsOpenTutorial, _IsShowedOpenDoorTutorial, _Step, 
			_CanRotateCamera, _HasRotated, _isOpenMask,
			_isLockDoor, _isGrandDaughterRebel, _isGrandDaughterInQueue, 
			_plotStatus, _rotateAngle, _allureCount, 
			_isDarkPrinceStartMove, _maxTipCount
		); 
	}

	public static PlayerSaveInfo ParseFromSaveString(string s)
	{
		PlayerSaveInfo info = new PlayerSaveInfo(); 
		string[] ss = s.Split(','); 
		if (ss.Length < 18)
		{
			return null; 
		}

		string temp = ss[0].Substring(ss[0].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._curLevel))
		{
			return null; 
		}
		temp = ss[1].Substring(ss[1].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._hp))
		{
			return null; 
		}
		temp = ss[2].Substring(ss[2].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._posIndex))
		{
			return null; 
		}

		temp = ss[3].Substring(ss[3].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._curMeetHint))
		{
			return null; 
		}

		temp = ss[4].Substring(ss[4].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._IsOpenTutorial))
		{
			return null; 
		}

		temp = ss[5].Substring(ss[5].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._IsShowedOpenDoorTutorial))
		{
			return null; 
		}

		temp = ss[6].Substring(ss[6].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._Step))
		{
			return null; 
		}

		temp = ss[7].Substring(ss[7].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._CanRotateCamera))
		{
			return null; 
		}

		temp = ss[8].Substring(ss[8].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._HasRotated))
		{
			return null; 
		}

		temp = ss[9].Substring(ss[9].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._isOpenMask))
		{
			return null; 
		}

		temp = ss[10].Substring(ss[10].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._isLockDoor))
		{
			return null; 
		}

		temp = ss[11].Substring(ss[11].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._isGrandDaughterRebel))
		{
			return null; 
		}

		temp = ss[12].Substring(ss[12].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._isGrandDaughterInQueue))
		{
			return null; 
		}

		temp = ss[13].Substring(ss[13].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._plotStatus))
		{
			return null; 
		}

		temp = ss[14].Substring(ss[14].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._rotateAngle))
		{
			return null; 
		}

		temp = ss[15].Substring(ss[15].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._allureCount))
		{
			return null; 
		}

		temp = ss[16].Substring(ss[16].IndexOf('=') + 1);
		if (!bool.TryParse(temp, out info._isDarkPrinceStartMove))
		{
			return null; 
		}

		temp = ss[17].Substring(ss[17].IndexOf('=') + 1);
		if (!int.TryParse(temp, out info._maxTipCount))
		{
			return null; 
		}
		return info; 
	}
}
