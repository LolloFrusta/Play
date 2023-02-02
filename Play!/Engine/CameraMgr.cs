using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    struct CameraLimits
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    static class CameraMgr
    {
        private static Dictionary<string, Tuple<Camera, float>> cameras;

        public static Camera MainCamera;

        public static GameObject Target;

        public static float CameraSpeed = 5;

        public static CameraLimits CameraLimits;

        public static float HalfDiagonalSquared { get; private set; }

        public static void Init()
        {
            MainCamera = new Camera(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);

            HalfDiagonalSquared = MainCamera.pivot.LengthSquared;

            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void ResetCamera()
        {
            MainCamera.position = Vector2.Zero;
            MainCamera.pivot = Vector2.Zero;
            Target = null;

            cameras.Clear();
        }



        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return null;
        }

        public static void Update()
        {
            Vector2 oldCameraPos = MainCamera.position;
            MainCamera.position = Vector2.Lerp(MainCamera.position, Target.Position, Game.DeltaTime * CameraSpeed);
            FixPosition();

            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            if(cameraDelta != Vector2.Zero)
            {
                foreach (var item in cameras)
                {
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;//camera.position += delta * cameraSpeed
                }

            }
        }

        private static void FixPosition()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }

    }
}
