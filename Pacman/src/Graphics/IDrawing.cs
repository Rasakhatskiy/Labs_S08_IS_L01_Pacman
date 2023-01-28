using Microsoft.Xna.Framework;

namespace BebraProject.Graphics;

public interface IDrawing
{
    public void Draw(Vector2 pos, bool flipX = false, bool flipY = false, float rotation = 0f);
    public void Stop();
    public void Start();
    public void Reset();
    public void Update();
}