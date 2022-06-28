#nullable enable

using System.Linq;
using UnityEngine;

namespace TranslationService
{
    public class Magnifier
    {
        public virtual float GetMagnifier(Vector2 beforeBounds, Vector2 afterBounds, string text, KMBombModule module)
        {
            return new float[] { beforeBounds.x / afterBounds.x, beforeBounds.y / afterBounds.y, 1 }.Min();
        }

        public class StaticMagnifier: Magnifier
        {
            public override float GetMagnifier(Vector2 beforeBounds, Vector2 afterBounds, string text, KMBombModule module)
            {
                return 1f;
            }
        }

        public class SizeLimitMagnifier: Magnifier
        {
            float maxHeight;
            float maxWidth;
            public SizeLimitMagnifier(float maxWidth, float maxHeight) { 
                this.maxWidth = maxWidth;
                this.maxHeight = maxHeight;
            }

            public override float GetMagnifier(Vector2 b, Vector2 afterBounds, string text, KMBombModule module)
            {
                return base.GetMagnifier(new Vector2(b.x * maxWidth, b.y * maxHeight), afterBounds, text, module);
            }
        }

        public class VectorMagnifier : Magnifier
        {
            Vector3 vec;
            public VectorMagnifier(Vector2 vec) { this.vec = vec; }
            public VectorMagnifier(float x, float y) { vec = new Vector2(x, y); }
            public override float GetMagnifier(Vector2 _, Vector2 afterBounds, string text, KMBombModule module)
            {
                return new float[] { vec.x / afterBounds.x, vec.y / afterBounds.y }.Min();
            }
        }



        public static Magnifier Default = new Magnifier();
        public static Magnifier Static = new StaticMagnifier();
    }

}
