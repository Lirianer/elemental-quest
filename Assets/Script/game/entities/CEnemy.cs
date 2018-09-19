public class CEnemy : CAnimatedSprite
{
    public const int STATE_STAND = 0;
    // ESTADO PARA LEVANTAR EN EL AIRE
    public const int STATE_ELEVATED = 1;
    public const int STATE_FALLING = 2;
    public const int STATE_WALKING = 3;

    public bool isMovable = false;
    
    protected float velocityBeforeFalling = 0;

    override public void update()
    {
        base.update();
    }

    override public void render()
    {
        base.render();
    }

    override public void destroy()
    {
        base.destroy();
    }

    override public void setState(int aState)
    {
        base.setState(aState);
    }
    
}