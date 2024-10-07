using System;
// Interface for Balloon Movement abstraction
public interface IBalloonMovement
{
    void Initialize(float commonXPoint, float screenMinX, float screenMaxX);
    event Action OnBalloonDestroyed;
}
