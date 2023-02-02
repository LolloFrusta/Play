using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    static class Game
    {
        private static List<Controller> controllers;
        private static KeyboardController keyboardController;


        public static Window Window;

        public static Scene CurrentScene { get; private set; }

        public static float DeltaTime { get { return Window.DeltaTime; } }

        public static float UnitSize { get; private set; }
        public static float OptimalScreenHeight { get; private set; }
        public static float OptimalUnitSize { get; private set; }

        public static Vector2 ScreenCenter { get; private set; }
        public static float ScreenDiagonalSquared { get; private set; }

        public static void Init()
        {
            Window = new Window(800, 800, "Heads");
            Window.SetVSync(false);
            Window.SetDefaultViewportOrthographicSize(10);

            OptimalScreenHeight = 1080;

            UnitSize = Window.Height / Window.OrthoHeight;
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight;//108

            ScreenCenter = new Vector2(Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f);
            ScreenDiagonalSquared = ScreenCenter.LengthSquared;

            //TitleScene titleScene = new TitleScene("Assets/aivBG.png");
            PlayScene playScene = new PlayScene();
            //GameOverScene gameOverScene = new GameOverScene();

            //titleScene.NextScene = playScene;
            //playScene.NextScene = gameOverScene;
            //gameOverScene.NextScene = playScene;

            List<KeyCode> keys = new List<KeyCode>();
            keys.Add(KeyCode.W);
            keys.Add(KeyCode.S);
            keys.Add(KeyCode.D);
            keys.Add(KeyCode.A);
            keys.Add(KeyCode.Space);

            KeysList keyList = new KeysList(keys);
            keyboardController = new KeyboardController(0,keyList);

            string[] joystics = Window.Joysticks;
            controllers = new List<Controller>();

            for (int i = 0; i < joystics.Length; i++)
            {
                if(joystics[i]!=null && joystics[i]!="Unmapped Controller")
                {
                    controllers.Add(new JoypadController(i));
                }
            }



            CurrentScene = playScene;
        }

        public static float PixelsToUnits(float pizelsSize)
        {
            return pizelsSize / OptimalUnitSize;
        }

        public static Controller GetController(int index)
        {
            Controller ctrl = keyboardController;

            if (index < controllers.Count)
            {
                ctrl = controllers[index];
            }

            return ctrl;
        }

        

        public static void Play()
        {
            //Counter myCounter = new Counter();

            CurrentScene.Start();

            while (Window.IsOpened)
            {
                //float fps = 1f / Window.DeltaTime;
                //Window.SetTitle($"FPS: {fps}");

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();
                    GC.Collect();//explicit call to Garbage Collector

                    if (nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                //INPUT
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }
               // CurrentScene.Input();

                //UPDATE
                CurrentScene.Update();

                //DRAW
                CurrentScene.Draw();

                Window.Update();
            }
        }
    }

    
}
