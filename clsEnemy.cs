/*
  clsEnemy.cs
*/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace sprite2d {
  public class clsEnemy : clsSprite {
    public clsEnemy(PointF p, infSprite inf) :
        base(p, new PointF(0, 0), inf, -1, false, true) {}

    public override IEnumerable<clsShot> emit(infSprite sinf)
    {
      Size swh = new Size(sinf.img.Width, sinf.img.Height);
      Size ewh = wh();
      float x = position.X;
      float y = position.Y + (ewh.Height + swh.Height) / 2.0f;
      PointF p = new PointF(x, y);
      float dx = 0;
      float dy = swh.Height;
      PointF v = new PointF(dx, dy);
      yield return new clsShot(p, v, sinf);
    }
  }
}
