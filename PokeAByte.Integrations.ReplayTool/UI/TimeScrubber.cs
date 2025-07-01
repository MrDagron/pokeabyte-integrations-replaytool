using System;
using System.Drawing;
using System.Windows.Forms;

namespace PokeAByte.Integrations.ReplayTool.UI;

public sealed class TimeScrubber : Control
{
    public TimeScrubber()
    {
        DoubleBuffered = true;
    }
    private readonly Color _watchedZoneColor = Color.Aqua;
    private readonly Color _unwatchedZoneColor = Color.Gray;
    public bool IsMouseDown = false;
    
    public event EventHandler<PositionChangedEventArgs> PositionChanged;
    private (int X, int Y) _controlMousePosition = new(0,0);

    public (int X, int Y) ControlMousePosition
    {
        get => _controlMousePosition;
        set
        {
            if (_controlMousePosition != value)
            {
                var oldValue = _controlMousePosition;
                _controlMousePosition = value;
                OnPositionChanged(new PositionChangedEventArgs(oldValue, _controlMousePosition));
            }
        }
    }

    private void OnPositionChanged(PositionChangedEventArgs e)
    {
        PositionChanged?.Invoke(this, e);
    }
    
    private int _totalCount = 0;
    
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        ControlMousePosition = (e.X, e.Y);
        IsMouseDown = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        ControlMousePosition = (e.X, e.Y);
        IsMouseDown = false;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (IsMouseDown)
        {
            ControlMousePosition = (e.X, e.Y);
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_totalCount == 0) return;
        using var watchedZoneBrush = new SolidBrush(_watchedZoneColor);
        using var unwatchedZoneBrush = new SolidBrush(_unwatchedZoneColor);
        e.Graphics.FillRectangle(unwatchedZoneBrush, ClientRectangle);
        var fillWidth = Math.Max(0, Math.Min(_controlMousePosition.X, Width));
        e.Graphics.FillRectangle(watchedZoneBrush, 0, 0, fillWidth, Height);
        using var borderPen = new Pen(Color.Black);
        e.Graphics.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }

    public int GetIndex(int total)
    {
        _totalCount = total;
        var scale = (double)Width / total;
        var index = (int)(_controlMousePosition.X / scale);
        return Math.Max(0, Math.Min(index, total - 1));
    }

    public void UpdatePosition(int index, int total)
    {
        _totalCount = total;
        var scale = (double)Width / total;
        var x = index * scale;
        ControlMousePosition = ((int)x, 0);
        //_controlMousePosition.X = (int)x;
        Invalidate();
    }
}
public class PositionChangedEventArgs : EventArgs
{
    public (int X, int Y) OldPosition { get; }
    public (int X, int Y) NewPosition { get; }
    
    public PositionChangedEventArgs((int X, int Y) oldPosition, (int X, int Y) newPosition)
    {
        OldPosition = oldPosition;
        NewPosition = newPosition;
    }
}
