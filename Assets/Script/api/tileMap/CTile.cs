﻿using UnityEngine;
using System.Collections;

public class CTile : CSprite
{
	// Tile index. Starting from 0. 0, 1, 2...
	private int mTileIndex;

	// True = se puede caminar. False = no se puede traspasar.
	private bool mIsWalkable;

	private static GameObject mMapObject;

	// Parametros: coordenada del tile (x, y) y el indice del tile.
	public CTile(int aX, int aY, int aTileIndex, Sprite aSprite, int aScale = 1)
	{
		if (mMapObject == null)
        {
            CTile.mMapObject = new GameObject();
            mMapObject.name = "Map";
        }

		setXY (aX, aY);
		setTileIndex(aTileIndex);

		setImage (aSprite);
		setSortingLayerName ("TileMap");

		setScale (aScale);

		setName ("Tile - " + (aY / this.getHeight()) + "/" + (aX / this.getWidth()) );
		setParent(mMapObject);
		//hideInUnityHierarchy ();
	}

	public void setTileIndex(int aTileIndex)
	{
		mTileIndex = aTileIndex;

		// Set walkable information.
		if (aTileIndex == 2)
			mIsWalkable = false;
		else
			mIsWalkable = true;
	}

	public int getTileIndex()
	{
		return mTileIndex;
	}

	override public void render()
	{
		base.render ();
	}

	override public void update()
	{
		base.update ();
	}

	override public void destroy()
	{
		base.destroy ();
	}

	// Invocada desde CTileMap cuando se clickea el mouse en este tile.
	public void clicked()
	{
		//setColor(Color.red);
	}

	public bool isWalkable()
	{
		return mIsWalkable;
	}

	public void setWalkable(bool aIsWalkable)
	{
		mIsWalkable = aIsWalkable;
	}
}
