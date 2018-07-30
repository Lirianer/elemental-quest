using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEnemyInfo
{
	// Tipos de enemigos.
	public const int ENEMY_SCORPION = 0;
	public const int ENEMY_SNAKE = 1;

	// Array con los enemigos de todas las pantallas.
	private List<SEnemyData> mEnemies;

	// Array con los enemigos de cada pantalla.
	public static int[][] mEnemiesScreen = 
	{
		new int[] {0, 1}, // Pantalla 0
		new int[] {2, 3}, // Pantalla 1
	};

	public struct SEnemyData
	{
		public int mIndex; // ID: Indice del array de enemigos.
		public int mType; // Tipo de enemigo: CEnemy.SCORPION, etc..
		public int mCol; // Columna donde sale.
		public int mRow; // Fila donde sale.
		public bool mAlive; // Si esta vivo o no.

		public SEnemyData(int aIndex, int aType, int aCol, int aRow, bool aAlive)
		{
			mIndex = aIndex;
			mType = aType;
			mCol = aCol;
			mRow = aRow;
			mAlive = aAlive;
		}
	}

	public CEnemyInfo()
	{
		mEnemies = new List<SEnemyData>();
		// Cargar todos los enemigos de todas las pantallas.
		mEnemies.Add(new SEnemyData(0, ENEMY_SCORPION, 7, 5, true));
		mEnemies.Add(new SEnemyData(1, ENEMY_SCORPION, 8, 5, true));
		mEnemies.Add(new SEnemyData(2, ENEMY_SCORPION, 10, 5, true));
		mEnemies.Add(new SEnemyData(3, ENEMY_SCORPION, 11, 5, true));
	}

	public List<SEnemyData> getEnemyInfoList()
	{
		return mEnemies;
	}
}
