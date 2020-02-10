/*
  clsShot.cs
*/

using System.Drawing;

namespace sprite2d {
  public class clsShot : clsSprite {
    public clsShot(PointF p, PointF v, infSprite inf) :
        base(p, v, inf, -1, true, false) {}
  }
}
