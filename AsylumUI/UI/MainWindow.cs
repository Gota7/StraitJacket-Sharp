using System;
using ImGuiNET;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace AsylumUI.UI {
    
    // Main window.
    public static class MainWindow {
        public const int BaseWidth = 1024;
        public const int BaseHeight = 576;
        public static Color ClearColor = Color.BLACK;
        static ImguiController Controller = new ImguiController();
        static Block Block = new Block();
        static float SlideScale = 2f;
        static Camera2D Camera;

        // Constructor.
        static MainWindow() {
            R.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            R.InitWindow(BaseWidth, BaseHeight, "Asylum UI");
            Controller.Load(BaseWidth, BaseHeight);
            Camera.zoom = 1f;
            Block.BlockType = BlockType.ConnectsBottom;
            Block.ConnectedFoot = new Block();
            Block.ConnectedFoot.BlockType = BlockType.ConnectsBoth;
            //Block.ConnectedFoot.Color = new Color(255, 0, 0, 150);
        }

        // Run the window.
        public static void Run() {
            
            // Run until should close.
            while (!R.WindowShouldClose()) {

                // Update frame.
                Controller.Update(R.GetFrameTime());
                DrawUI();

                // Draw stuff.
                R.BeginDrawing();
                R.ClearBackground(ClearColor);
                R.BeginMode2D(Camera);
                DrawGraphics();
                R.EndMode2D();
                Controller.Draw();
                R.EndDrawing();

                // Update.
                Update();

            }

            // Cleanup.
            R.CloseWindow();
            Controller.Dispose();

        }

        // Draw the window UI.
        private static void DrawUI() {
            //ImGui.ShowDemoWindow();
            ImGui.Begin("Test");
            ImGui.DragFloat("Size", ref Camera.zoom, .1f, 0.5f, 4f);
            ImGui.End();
        }

        // Draw graphics.
        private static void DrawGraphics() {
            Block.Draw();
        }

        // Update the window.
        private static void Update() {

            // Check for resize.
            if (R.IsWindowResized()) {
                Controller.Resize(R.GetScreenWidth(), R.GetScreenHeight());
            }
            Block.Update();

        }

    }

}