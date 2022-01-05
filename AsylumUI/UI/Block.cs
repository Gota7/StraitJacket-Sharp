using System;
using System.Numerics;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace AsylumUI.UI {

    // Block type.
    [Flags]
    public enum BlockType {
        ConnectsNone = 0b0,
        ConnectsBottom = 0b1,
        ConnectsTop = 0b10,
        ConnectsBoth,
        ConnectsInside = 0b100
    }

    // Block that can be used.
    public class Block {
        public Color Color = Color.GOLD;
        public BlockType BlockType;
        public int NumInsideConnections;
        public Block ConnectedHead = null;
        public Block ConnectedFoot = null;
        public Block[] ConnectedInsides;
        public float X = 50;
        public float Y = 50;
        float Width = 100;
        float Height = 35;
        float[] InsideHeights;
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

        // Init a block.
        public void Init() {
            ConnectedInsides = new Block[NumInsideConnections];
            InsideHeights = new float[NumInsideConnections];
            for (int i = 0; i < NumInsideConnections; i++) {
                ConnectedInsides[i] = null;
                InsideHeights[i] = 10;
            }
        }

        // Draw a tab.
        private void DrawTab(float x, float y, float tabSize, Color color) {
            R.DrawTriangle(
                    new Vector2(x + tabSize / 2, y + tabSize / 2),
                    new Vector2(x + tabSize / 2, y),
                    new Vector2(x, y),
                    color
                );
            R.DrawRectangleV(
                new Vector2(x + tabSize / 2, y),
                new Vector2(tabSize, tabSize / 2),
                color
            );
            R.DrawTriangle(
                new Vector2(x + tabSize * 3 / 2, y),
                new Vector2(x + tabSize * 3 / 2, y + tabSize / 2),
                new Vector2(x + tabSize * 2, y),
                color
            );
            Color outline = new Color(Math.Min(color.r + 50, 255), Math.Min(color.g + 50, 255), Math.Min(color.b + 50, 255), color.a);
            R.DrawLineEx(
                new Vector2(x, y),
                new Vector2(x + tabSize / 2, y + tabSize / 2),
                tabSize / 15,
                outline
            );
            R.DrawLineEx(
                new Vector2(x + tabSize / 2 - tabSize / 30, y + tabSize / 2),
                new Vector2(x + tabSize * 3 / 2 + tabSize / 30, y + tabSize / 2),
                tabSize / 15,
                outline
            );
            R.DrawLineEx(
                new Vector2(x + tabSize * 3 / 2, y + tabSize / 2),
                new Vector2(x + tabSize * 2, y),
                tabSize / 15,
                outline
            );
        }

        // Draw a body.
        private void DrawBody(float x, float y, float width, float height, float tabSize, Color color) {
            Color outline = new Color(Math.Min(color.r + 50, 255), Math.Min(color.g + 50, 255), Math.Min(color.b + 50, 255), color.a);
            R.DrawRectangleRounded(new Rectangle(x, y, width, height), .2f, 1, color);
            R.DrawRectangleRoundedLines(new Rectangle(x, y, width, height), .25f, 1, (int)(tabSize / 10), outline);
        }

        // Draw the block.
        public void Draw() {

            // Draw connected block.
            if (ConnectedBottom) {
                ConnectedFoot.Draw();
            }

            // Draw the main shape.
            float scale = 2.5f;
            DrawBody(X, Y, Width * scale, Height * scale, TabSize * scale, Color);

            // Draw the head tab.
            if ((BlockType & BlockType.ConnectsTop) > 0 && !ConnectedTop) {
                DrawTab(X + HeadOffX * scale, Y + HeadOffY * scale, TabSize * scale, MainWindow.ClearColor);
            }

            // Draw the foot tab.
            if ((BlockType & BlockType.ConnectsBottom) > 0) {
                DrawTab(X + FootOffX * scale, Y + FootOffY * scale, TabSize * scale, Color);
            }
            
        }

        // Update the block.
        public void Update() {

            // Find foot.
            if ((BlockType & BlockType.ConnectsBottom) > 0) {
                FootOffY = Height;
            }

            // Connected block.
            if (ConnectedBottom) {
                ConnectedFoot.X = X;
                ConnectedFoot.Y = Y + FootOffY * 2.5f;
                ConnectedFoot.Update();
            }

        }

    }

}