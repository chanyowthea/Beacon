using UnityEngine;
using System.Collections;

public static class PositionUtil
{
	static Pos[] _coordinates = new Pos[4]{new Pos(0, 1), // 上，EulerAngle == 0
		new Pos(-1, 0), // 左 90
		new Pos(0, -1), // 下 180 
		new Pos(1, 0)}; // 右 270(-90)


	public static Pos ToPos(this Vector3 pos)
	{
		return new Pos((int)(pos.x - 0.5f), (int)(pos.y - 0.5f)); // 实际位置到Pos偏移0.5f
	}

	public static Vector3 ToVector3(this Pos pos)
	{
		return new Vector3(pos._x + 0.5f, pos._y + 0.5f); // 实际位置到Pos偏移0.5f
	}

	public static Pos GetIndex(Vector3 targetPos, Vector3 curPos)
	{
		int x = Mathf.RoundToInt(Mathf.Clamp(targetPos.x - curPos.x, -1, 1)); 
		int y = Mathf.RoundToInt(Mathf.Clamp(targetPos.y - curPos.y, -1, 1)); 
		for (int i = 0, max = _coordinates.Length; i < max; i++)
		{
			var c = _coordinates[i]; 
			if (c._x == x && c._y == y)
			{
				return _coordinates[i]; 
			}
		}
		return _coordinates[0]; 
	}

	public static int ToEulerAngle(int h, int v)
	{
		Pos p = new Pos(h, v); 
		for (int i = 0, max = _coordinates.Length; i < max; i++)
		{
			if (_coordinates[i] == p)
			{
				return i * 90; 
			}
		}
		return 0 * 90; 
	}
}
