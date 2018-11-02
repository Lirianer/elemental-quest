using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class CTile : CSprite
{
	public enum Type : int {
		AIR = 72,
		STONE = 34,
		MUD = 14,
		WATER = 8,
		EARTH = 1,
		ICE = 5,
		SOMETHING,

		ARTIFICIAL_EARTH = 23
	}

	public enum Direction : int {
		TOP,
		LEFT,
		RIGHT,
		BOTTOM
	}

	// Tile index. Starting from 0. 0, 1, 2...
	private int mTileIndex;

	// True = se puede caminar. False = no se puede traspasar.
	private bool mIsWalkable;

	private static GameObject mMapObject;

	private int[] waterIndexes =  new int[] {8, 18, 28};
	private int[] iceIndexes = new int[] {5, 6};
	private int[] stoneIndexes = new int[] {34, 35, 36, 44, 45, 46, 54, 55, 56, 47, 48};
	private int[] earthIndexes = new int[] {1, 2, 3, 4, 7, 9, 10, 11, 12, 13, 17, 19, 20, 22, 23, 24, 27, 29, 30, 31, 32, 33, 37, 38, 39};
	private int[] mudIndexes = new int[] {14, 15, 16, 25, 26};


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
		setVisible(false);
		//hideInUnityHierarchy ();
	}

	public void setTileIndex(int aTileIndex)
	{
		mTileIndex = aTileIndex;


		// Set walkable information.
		if (this.getTileType() == CTile.Type.AIR)
			mIsWalkable = true;
		else if (this.getTileType() == CTile.Type.WATER)
			mIsWalkable = true;
		else
			mIsWalkable = false;
	}

	public void setTileType(Type type)
	{
		this.setTileIndex((int) type);
	}

	public int getTileIndex()
	{
		return mTileIndex;
	}

	public Type getTileType()
	{
		if(Array.IndexOf(this.waterIndexes, this.getTileIndex()) > -1)
		{
			return CTile.Type.WATER;
		}
		else if(Array.IndexOf(this.iceIndexes, this.getTileIndex()) > -1)
		{
			return CTile.Type.ICE;
		}
		else if(Array.IndexOf(this.stoneIndexes, this.getTileIndex()) > -1)
		{
			return CTile.Type.STONE;
		}
		else if(Array.IndexOf(this.earthIndexes, this.getTileIndex()) > -1)
		{
			return CTile.Type.EARTH;
		}
		else if(Array.IndexOf(this.mudIndexes, this.getTileIndex()) > -1)
		{
			return CTile.Type.MUD;
		}

		return (Type)this.getTileIndex();
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

	public CTile getNeighbourTile(Direction direction)
	{
		int xDirection = 0;
		int yDirection = 0;

		switch (direction)
		{
			case Direction.TOP:
				yDirection = -1;
			break;
			case Direction.RIGHT:
				xDirection = 1;
			break;
			case Direction.BOTTOM:
				yDirection = 1;
			break;
			case Direction.LEFT:
				xDirection = -1;
			break;
		}

		return CTileMap.Instance.getTile((int)(this.getX() + CTileMap.Instance.getTileWidth() * xDirection) / CTileMap.Instance.getTileWidth(), (int)(this.getY() + CTileMap.Instance.getTileHeight() * yDirection) / CTileMap.Instance.getTileHeight());
	}
}
