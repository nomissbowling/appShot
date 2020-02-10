/*
  clsPlayer.cs
*/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace sprite2d {
  public class clsPlayer : clsSprite {
    public clsPlayer(PointF p, infSprite inf) :
        base(p, new PointF(0, 0), inf, -1, false, false) {}

    public override IEnumerable<clsShot> emit(infSprite sinf)
    {
      Size swh = new Size(sinf.img.Width, sinf.img.Height);
      Size pwh = wh();
      float x = position.X;
      float y = position.Y - (pwh.Height + swh.Height) / 2.0f;
      PointF p = new PointF(x, y);
      int r = swh.Height; // too short to reject crush with neighbourhood
      int ds = 7; // 1; to 17; // odd number
      int hd = ds / 2; // (i - hd) means (-hd ... 0 ... +hd)
      for(int i = 0; i < ds; ++i){
        double ang = (double)(i - hd) / ds;
        //double th = Math.PI / 2.0 + 2.0 * Math.PI * ang; // circle around
        double th = Math.PI / 2.0 + Math.PI * ang; // half circle
        float dx = (float)(r * Math.Cos(th));
        float dy = (float)(r * Math.Sin(th));
        PointF v = new PointF(dx, -dy);
        yield return new clsShot(p, v, sinf);
      }
    }
  }
}
