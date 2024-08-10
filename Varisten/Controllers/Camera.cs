using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera
{
    public Matrix Transform { get; private set; }
    public Vector2 Position { get; private set; }
    private Vector2 _origin;
    private Viewport _viewport;
    private float _zoom;

    public Camera(Viewport viewport)
    {
        _viewport = viewport;
        _origin = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
        _zoom = 0.7f; // Zoom padrão
        UpdateMatrix();
    }

    public void UpdateMatrix()
    {
        Transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                    Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
                    Matrix.CreateScale(_zoom);
    }

    public void Follow(Vector2 target)
    {
        Position = target - _origin + new Vector2(-20f, 150f) / _zoom;
        UpdateMatrix();
    }

    public void ResetPosition(float maxX, float maxY)
    {
        float posX = Position.X;
        float posY = Position.Y;
        float _width = _viewport.Width / 2;
        float _height = _viewport.Height / 2;
        if (Position.X - _width <= 0) posX = 0;
        if (Position.X + _width >= maxX) posX = maxX;
        if (Position.Y - _height <= 0) posY = 0;
        if (Position.Y + _height >= maxY) posY = maxY;
        Position = new Vector2(posX, posY);
    }

    public void SetZoom(float zoom)
    {
        _zoom = zoom;
        UpdateMatrix();
    }
}
