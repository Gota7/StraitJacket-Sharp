using System;
using System.Numerics;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace AsylumUI.UI {

    // Block that can be used.
    public class Block {
        public Color Color = Color.GOLD;
        public bool ConnectsHead = true;
        public bool ConnectsFoot = true;
        public Block ConnectedHead = null;
        public Block ConnectedFoot = null;
        public float X = 50;
        public float Y = 50;
        float Width = 100;
        float Height = 35;
        float HeadOffX = 10;
        float HeadOffY = 0;
        float FootOffX = 10;
        float FootOffY = 0;
        float TabSize = 10;
        float CircleHeadSizeX = 30;
        float CircleHeadSizeY = 10;
        float CircleHeadOff = 5;
        public bool ConnectedTop => ConnectedHead != null;
        public bool ConnectedBottom => ConnectedFoot != null;

        // Draw the block.
        public void Draw() {

            // Draw the main shape.
            float scale = 2.5f;
            R.DrawRectangleRounded(new Rectangle(X, Y, Width * scale, Height * scale), .2f, 1, Color);

            // Draw the head tab.
            if (ConnectsHead && !ConnectedTop) {
                R.DrawTriangle(
                    new Vector2(X + (HeadOffX + TabSize / 2) * scale, Y + (HeadOffY + TabSize / 2) * scale),
                    new Vector2(X + (HeadOffX + TabSize / 2) * scale, Y + HeadOffY),
                    new Vector2(X + HeadOffX * scale, Y + HeadOffY * scale),
                    MainWindow.ClearColor
                );
                R.DrawRectangleV(
                    new Vector2(X + (HeadOffX + TabSize / 2) * scale,  Y + HeadOffY * scale),
                    new Vector2(TabSize, TabSize / 2) * scale,
                    MainWindow.ClearColor
                );
                R.DrawTriangle(
                    new Vector2(X + (HeadOffX + TabSize * 3 / 2) * scale, Y + HeadOffY * scale),
                    new Vector2(X + (HeadOffX + TabSize * 3 / 2) * scale, Y + (HeadOffY + TabSize / 2) * scale),
                    new Vector2(X + (HeadOffX + TabSize * 2) * scale, Y + HeadOffY * scale),
                    MainWindow.ClearColor
                );
            }
            
            // Circle tab.
            else if (!ConnectsHead) {
                R.DrawEllipse(
                    (int)(X + (CircleHeadSizeX + CircleHeadOff) * scale),
                    (int)(Y + (CircleHeadSizeY / 3) * scale),
                    CircleHeadSizeX * scale,
                    CircleHeadSizeY * scale,
                    Color
                );
            }

            // Draw the foot tab.
            if (ConnectsFoot) {
                R.DrawTriangle(
                    new Vector2(X + (FootOffX + TabSize / 2) * scale, Y + (FootOffY + TabSize / 2) * scale),
                    new Vector2(X + (FootOffX + TabSize / 2) * scale, Y + FootOffY * scale),
                    new Vector2(X + FootOffX * scale, Y + FootOffY * scale),
                    Color
                );
                R.DrawRectangleV(
                    new Vector2(X + (FootOffX + TabSize / 2) * scale, Y + FootOffY * scale),
                    new Vector2(TabSize * scale, (TabSize / 2) * scale),
                    Color
                );
                R.DrawTriangle(
                    new Vector2(X + (FootOffX + TabSize * 3 / 2) * scale, Y + FootOffY * scale),
                    new Vector2(X + (FootOffX + TabSize * 3 / 2) * scale, Y + (FootOffY + TabSize / 2) * scale),
                    new Vector2(X + (FootOffX + TabSize * 2) * scale, Y + FootOffY * scale),
                    Color
                );
            }
            
        }

        // Update the block.
        public void Update() {

            // Find foot.
            if (ConnectsFoot) {
                FootOffY = Height;
            }

        }

    }

}