/*
  appShot.cs

  > nmake appShot.mak
  > appShot 1 2 3
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace sprite2d {
  public class appShot {
    public appShot()
    {
      // Console.WriteLine("(appShot)");
    }

    public static int Main(params string[] args)
    {
      System.Windows.Forms.Application.Run(new frmMain());
      return 0;
    }
  }
}
