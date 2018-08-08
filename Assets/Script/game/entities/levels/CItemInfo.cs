using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CItemInfo
{
	// Tipos de items.
	public const int ITEM_BIRD_1 = 0;
	public const int ITEM_BIRD_2 = 1;
	public const int ITEM_LIFE = 2;
	public const int ITEM_CUP_1 = 3;
	public const int ITEM_CUP_2 = 4;
	public const int ITEM_COIN = 5;

	// Array con los enemigos de todas las pantallas.
	private List<SItemData> mItems;

	// Array con los enemigos de cada pantalla.
	public static int[][] mItemsScreen = 
	{
		new int[] {0, 1}, // Pantalla 0
		new int[] {2, 3}, // Pantalla 1
	};

	public struct SItemData
	{
		public int mIndex; // ID: Indice del array de items.
		public int mType; // Tipo de enemigo: CItem.ITEM_BIRD_1, etc..
		public int mCol; // Columna donde sale.
		public int mRow; // Fila donde sale.
		public bool mAlive; // Si esta vivo o no.

		public SItemData(int aIndex, int aType, int aCol, int aRow, bool aAlive)
		{
			mIndex = aIndex;
			mType = aType;
			mCol = aCol;
			mRow = aRow;
			mAlive = aAlive;
		}
	}

	public CItemInfo()
	{
		mItems = new List<SItemData>();
		// Cargar todos los enemigos de todas las pantallas.
		mItems.Add(new SItemData(0, ITEM_BIRD_1, 7, 5, true)); // Pantalla 0
		mItems.Add(new SItemData(1, ITEM_BIRD_2, 8, 5, true));
		mItems.Add(new SItemData(2, ITEM_LIFE, 10, 5, true)); // Pantalla 1
		mItems.Add(new SItemData(3, ITEM_COIN, 11, 5, true));
	}

	public List<SItemData> getEnemyInfoList()
	{
		return mItems;
	}
}
