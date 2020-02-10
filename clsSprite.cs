/*
  clsSprite.cs
*/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace sprite2d {
  public struct infSprite {
    public float mass, elastic;
    public Image img, crush, explosion;

    public infSprite(float mass, float elastic,
      Image img, Image crush, Image explosion)
    {
      this.mass = mass;
      this.elastic = elastic;
      this.img = img;
      this.crush = crush;
      this.explosion = explosion;
    }
  }

  public class clsSprite {
    protected PointF position;
    protected PointF velocity;
    protected PointF acceleration;
    protected infSprite inf;
    protected long life;
    protected bool vanish;
    protected bool emission;

    public clsSprite(PointF p, PointF v, infSprite inf,
      long life, bool vanish, bool emission)
    {
      position = new PointF(p.X, p.Y);
      velocity = new PointF(v.X, v.Y);
      acceleration = new PointF(0, 0);
      this.inf = inf;
      this.life = life;
      this.vanish = vanish;
      this.emission = emission;
    }

    public virtual void move(PointF d, Rectangle bound)
    {
      if(life > 0) --life;
      position.X += d.X;
      position.Y += d.Y;
      PointF lt = new PointF(bound.Left + inf.img.Width / 2.0f,
        bound.Top + inf.img.Height / 2.0f);
      PointF rb = new PointF(bound.Right - inf.img.Width / 2.0f,
        bound.Bottom - inf.img.Height / 2.0f);
      if(position.X < lt.X) position.X = lt.X;
      else if(position.X > rb.X) position.X = rb.X;
      if(position.Y < lt.Y) position.Y = lt.Y;
      else if(position.Y > rb.Y) position.Y = rb.Y;
    }

    public virtual void move()
    {
      if(life > 0) --life;
      position.X += velocity.X;
      position.Y += velocity.Y;
      velocity.X *= (float)Math.Exp(acceleration.X);
      velocity.Y *= (float)Math.Exp(acceleration.Y);
    }

    public virtual void draw(Graphics gr)
    {
      Image im = inf.img;
      if(life >= 0){
        if(life <= 10) im = inf.explosion;
        else if(life <= 20) im = inf.crush;
      }
      if(alive())
        gr.DrawImage(im, new Point(
          (int)(position.X - im.Width / 2.0f),
          (int)(position.Y - im.Height / 2.0f)));
    }

    public virtual bool intersect(Rectangle border)
    {
      return rect().IntersectsWith(border);
    }

    public virtual Size wh()
    {
      return new Size(inf.img.Width, inf.img.Height);
    }

    public virtual Rectangle rect()
    {
      return new Rectangle(
        (int)(position.X - inf.img.Width / 2.0f),
        (int)(position.Y - inf.img.Height / 2.0f),
        inf.img.Width, inf.img.Height);
    }

    public virtual void collide(clsSprite s)
    {
      if(!live(20)) return;
      // s.collide(this);
      s.live(20);
      // without the moment
      float e = inf.elastic;
      float ee = 1.0f + e;
      float emt = inf.mass - e * s.inf.mass;
      float ems = s.inf.mass - e * inf.mass;
      float m = inf.mass + s.inf.mass;
      float vx = (emt * velocity.X + ee * s.inf.mass * s.velocity.X) / m;
      float vy = (emt * velocity.Y + ee * s.inf.mass * s.velocity.Y) / m;
      s.velocity.X = (ee * inf.mass * velocity.X + ems * s.velocity.X) / m;
      s.velocity.Y = (ee * inf.mass * velocity.Y + ems * s.velocity.Y) / m;
      velocity.X = vx;
      velocity.Y = vy;
      // attenuation
      s.acceleration.X = -0.2f;
      s.acceleration.Y = -0.2f;
      acceleration.X = -0.2f;
      acceleration.Y = -0.2f;
    }

    public virtual bool live(long life)
    {
      if(this.life < 0){ this.life = life; return true; }
      else return false;
    }

    public virtual bool alive(){ return life != 0; }
    public virtual bool isvanish(){ return vanish; }
    public virtual bool isemission(){ return emission; }

    public virtual IEnumerable<clsShot> emit(infSprite inf)
    {
      // throw new IndexOutOfRangeException("emit().next");
      throw new ArgumentOutOfRangeException("emit().next");
      // yield return null;
    }
  }
}
