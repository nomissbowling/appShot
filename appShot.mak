# nmake
all: appShot.exe

appShot.exe : appShot.cs frmMain.dll
	csc -reference:frmMain.dll appShot.cs

frmMain.dll : frmMain.cs clsSprites.dll
	csc -target:library -reference:clsSprites.dll frmMain.cs

clsSprites.dll : clsSprite.cs clsShot.cs clsPlayer.cs clsEnemy.cs
	csc -target:library -out:clsSprites.dll clsSprite.cs clsShot.cs clsPlayer.cs clsEnemy.cs
