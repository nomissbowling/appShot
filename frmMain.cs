/*
  frmMain.cs
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace sprite2d {
  public partial class frmMain : Form {
    private List<Image> imgRes;
    private infSprite infShot;
    private infSprite infPlayer;
    private infSprite infEnemy;
    private List<clsSprite> sprites;
    private clsPlayer player;
    private Timer timer1;
    private Random rand;

    private static string[] resources = {
      "Resources\\Shot.png",
      "Resources\\Player.png",
      "Resources\\Enemy.png",
      "Resources\\Crush.png",
      "Resources\\Explosion.png"};

    public frmMain()
    {
      InitializeComponent();
      this.DoubleBuffered = true;
      this.Load += new EventHandler(frmMain_Load);
      this.Resize += new EventHandler(frmMain_Resize);
      this.Paint += new PaintEventHandler(frmMain_Paint);
      this.KeyDown += new KeyEventHandler(frmMain_KeyDown);
      timer1 = new Timer();
      timer1.Tick += new EventHandler(timer1_Tick);
      rand = new Random();
    }

    private void InitializeComponent()
    {
      this.AutoScaleMode = AutoScaleMode.Font; // System.Windows.Forms
      this.Text = "appShot";

      this.Name = "appShot";
      // this.AutoScaleDimensions = new SizeF(6F, 12F); // System.Drawing
      // this.AutoScaleBaseSize = new Size(5, 12); // System.Drawing
      this.ClientSize = new Size(800, 600); // System.Drawing
      this.BackColor = Color.FromArgb(128, 248, 192);
      this.StartPosition = FormStartPosition.Manual;
      this.Location = new Point(320, 240);
      // this.MinimumSize = new Size(, );
      // this.MaximumSize = new Size(, );
    }

    protected override void Dispose(bool disposing)
    {
      if(disposing){
        // Console.WriteLine("(dispose frmMain)");
      }
      base.Dispose(disposing);
    }

    private PointF calcpos(infSprite inf, int r, int c)
    {
      float x = ClientRectangle.Width / 2.0f;
      float y = ClientRectangle.Height - (r + 1) * inf.img.Height;
      if(r > 0){
        x += inf.img.Width * (1.2f + 2.2f * c - r * 2.0f / 3.0f);
        y -= 7 * inf.img.Height;
      }
      return new PointF(x, y);
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
      imgRes = new List<Image>();
/*
      imgRes.Add(Properties.Resources.shot);
      imgRes.Add(Properties.Resources.player);
      imgRes.Add(Properties.Resources.enemy);
      imgRes.Add(Properties.Resources.crush);
      imgRes.Add(Properties.Resources.explosion);
*/
      foreach(string fn in resources) imgRes.Add(Image.FromFile(fn));
      infShot = new infSprite(1.0f, 0.2f, imgRes[0], imgRes[3], imgRes[4]);
      infPlayer = new infSprite(10.0f, 0.2f, imgRes[1], imgRes[3], imgRes[4]);
      infEnemy = new infSprite(10.0f, 0.2f, imgRes[2], imgRes[3], imgRes[4]);
      sprites = new List<clsSprite>();
      player = new clsPlayer(calcpos(infPlayer, 0, 0), infPlayer);
      sprites.Add(player);
      for(int j = 0; j < 3; ++j){
        for(int i = 0; i < 7; ++i){
          sprites.Add(new clsEnemy(calcpos(infEnemy, j + 1, i - 3), infEnemy));
        }
      }
      timer1.Interval = 100;
      timer1.Start(); // timer1.Enabled = true;
    }

    private void frmMain_Resize(object sender, EventArgs e)
    {
      ((Form)sender).Invalidate();
      // Console.WriteLine("resize");
    }

    private void frmMain_Paint(object sender, PaintEventArgs e)
    {
      Graphics gr = e.Graphics;
      foreach(clsSprite sprite in sprites){ sprite.draw(gr); }
    }

    private void frmMain_KeyDown(object sender, KeyEventArgs e)
    {
      switch(e.KeyCode){
      case Keys.Left: player.move(new PointF(-5, 0), ClientRectangle); break;
      case Keys.Right: player.move(new PointF(5, 0), ClientRectangle); break;
      case Keys.Up: player.move(new PointF(0, -5), ClientRectangle); break;
      case Keys.Down: player.move(new PointF(0, 5), ClientRectangle); break;
      case Keys.Space:
        foreach(clsShot s in player.emit(infShot)) sprites.Add(s);
        break;
      }
      Invalidate();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      for(int i = sprites.Count - 1; i >= 0; --i){
        clsSprite sprite = sprites[i];
        sprite.move();
        if(!sprite.alive()){ sprites.RemoveAt(i); }
        else if(sprite.isvanish()
             && !sprite.intersect(ClientRectangle)){ sprites.RemoveAt(i); }
        else{
          for(int j = sprites.Count - 1; j >= 0; --j){
            clsSprite s = sprites[j];
            if(j != i && sprite.intersect(s.rect())) sprite.collide(s);
          }
          if(sprite.alive() && sprite.isemission()){
            Byte[] b = new Byte[4];
            rand.NextBytes(b);
            if(b[0] <= 8){
              foreach(clsShot s in sprite.emit(infShot)) sprites.Add(s);
            }
          }
        }
      }
      Invalidate();
    }
  }
}
