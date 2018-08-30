public class CEnemy : CAnimatedSprite
{
    public const int STATE_STAND = 0;
    public const int STATE_FALLING = 1;
    public const int STATE_WALKING = 2;

    
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