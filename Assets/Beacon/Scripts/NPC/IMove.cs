using UnityEngine;
using System.Collections;

public interface IMove
{
	void Init(Pos pos, Transform transform); 
	void Clear(); 
	void Move(int x, int y); 
}

public class RandomMove : IMove
{
	int _x; 
	int _y; 
	IEnumerator _updateRoutine; 
	Transform _transform; 

	public void Init(Pos pos, Transform transform)
	{
		_transform = transform; 
		_x = pos._x; 
		_y = pos._y; 
		if (_updateRoutine != null)
		{
			CoroutineUtil.Stop(_updateRoutine); 
			_updateRoutine = null; 
		}
		_updateRoutine = UpdateRoutine(); 
		CoroutineUtil.Start(_updateRoutine);
	}

	public void Clear()
	{
		if (_updateRoutine != null)
		{
			CoroutineUtil.Stop(_updateRoutine); 
			_updateRoutine = null; 
		}
		_transform = null; 
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
				Move(pos._x, pos._y); 
			}
		}
	}

	public void Move(int x, int y) // 3 Enemy, 2 Pit, 1 Wall, 0 Road, 
	{
		int newX = _x + (int)x; 
		int newY = _y + (int)y; 
		int curIndex = MapManager.CurIndex(newX, newY); 

		if (curIndex >= 0)
		{
			Debug.Log("x, y: " + x + ", " + y); 
			//			Debug.Log("curIndex: " + curIndex + ", curMap: " + MapManager._curMap[curIndex]); 
			if (Player._Instance.GetPos() == new Pos(newX, newY)) // MapManager._curMap[curIndex] == MapCode.PIT
			{
				Player player = Player._Instance; 
				int count = 1; 
				bool rs = player.MinusHP(count); 
				if (rs)
				{
					UIManager._Instance.SetSysMsgInfo(string.Format("你受到{0}点伤害！", count)); // TODO 要设置敌人的攻击力，玩家的防御力
				}
			}
			else if (MapManager._curMap[curIndex] == MapCode.NONE || MapManager._curMap[curIndex] == MapCode.BEFORE_UPSTAIR 
				|| MapManager._curMap[curIndex] == MapCode.BEFORE_DOWNSTAIR)
			{
				int index = MapManager.CurIndex(_x, _y);
				_x = newX; 
				_y = newY; 

				_transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0); 

				if (index >= 0)
				{
					MapManager.ResetMap(curIndex, index);
				}
				else
				{
					Debug.Log("Index is minus! "); 
				}
			}
		}
	}
}