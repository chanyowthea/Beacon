using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AIMove : IMove
{
	[NonSerialized] public Action<Pos> onMove; 
	[NonSerialized] public Action onAllure; 
	[NonSerialized] public Transform _followTarget; 
	int _x; 
	int _y; 
	Transform _transform; 
	Animator _anim; 

	System.Random random = new System.Random(); 

	public void Init(Pos pos, Transform transform)
	{
		_transform = transform; 
		_anim = _transform.GetComponentInChildren<Animator>(); 
		_x = pos._x; 
		_y = pos._y; 
	}

	public void Clear()
	{
		_transform = null; 
		onMove = null; 
	}

	public void Update()
	{
		if (Player._Instance.isLockMove)
		{
			return; 
		}
		if (
//			Input.GetKeyDown(KeyCode.Space)
			(int)(random.Next(100)) == 0
		)
		{
			//			Debug.Log("DarkPrice Move"); 
			// 为寻路系统注入数据
			List<PathNode> nodes = new List<PathNode>();
			char[] map = MapManager.GetCurMap().ToCharArray(); 
			for (int i = 0, max = map.Length; i < max; i++)
			{
				PathNode n = new PathNode(); 
				var m = map[i]; 
				n._pos = MapManager.Index2Pos(i).ToVector3(); 
				n._isWall = m != MapCode.NONE; 
				n._index = i; 
				nodes.Add(n);
			}
			Singleton._pathFindystem._posLib = nodes; 
			Singleton._pathFindystem._startPos = new PathNode{_pos = _transform.position}; 
			Singleton._pathFindystem._endPos = new PathNode{_pos = _followTarget.position};
			var path = Singleton._pathFindystem.Find(); 
			if (path != null && path.Length > 0)
			{
				Pos pos = PositionUtil.GetIndex(path[0]._pos, _transform.localPosition); 
				Move(pos._x, pos._y); 
				if (onMove != null)
				{
					onMove(pos); 
				}
			}
			// 攻击
			else
			{
				if ((int)(random.Next(20)) == 0)
				{
					// 应该要把人物（Player）的识别信息也写到地图里面啊
					Pos pos = PositionUtil.GetIndex(Player._Instance.GetPos().ToVector3(), _transform.localPosition); 
					Move(pos._x, pos._y); 
				}
			}
		}
		if (
//			Input.GetKeyDown(KeyCode.L) 
			random.Next(600) == 0
		)
		{
			if (onAllure != null)
			{
				onAllure(); 
			}
		}
	}

	/// <summary>
	/// 输入偏移量
	/// </summary>
	public void Move(int x, int y)
	{
		if (_anim != null)
		{
			_anim.SetFloat("h", x); 
			_anim.SetFloat("v", y);
		}
		var pos = _transform.localPosition.ToPos(); 
		MoveUtil.Move(pos, x, y, _transform); 
	} 

	public void SetPos(Pos pos)
	{
		Debug.LogError("ai pos: " + pos); 
		_x = pos._x; 
		_y = pos._y; 
		_transform.position = pos.ToVector3(); 
	}
}
