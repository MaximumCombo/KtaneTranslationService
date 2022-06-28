
using UnityEngine;

namespace TranslationService
{
    public class OrientationCubeMagnifier: Magnifier
    {
        private static Magnifier vmag = new VectorMagnifier(0.14f, 0.14f);
        public override float GetMagnifier(Vector2 beforeBounds, Vector2 afterBounds, string text, KMBombModule module)
        {
            switch (text) {
                case "TOP":
                case "RIGHT":
                case "LEFT":
                case "BACK":
                case "FRONT":
                    return vmag.GetMagnifier(beforeBounds, afterBounds, text, module);
                default:
                    return base.GetMagnifier(beforeBounds, afterBounds, text, module);
            }

        }
    }
}
