using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace TranslationService.ModuleTranslators
{
    class MurderTranslator : ModuleTranslator
    {
        private readonly MethodInfo mUpdateDisplayPostfix;
        public MurderTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mUpdateDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateDisplayPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "ChangeDisplay"), null, new HarmonyMethod(mUpdateDisplayPostfix));
            harmony.Patch(AccessTools.Method(componentType, "ActivateModule"), null, new HarmonyMethod(mUpdateDisplayPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("MurderModule");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => langCode == "ja" ? new Magnifier.VectorMagnifier(0.08f, 0.010f) : new Magnifier.VectorMagnifier(0.1f, 0.012132f);

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            MurderTranslator.translator = translator;
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMesh(texts.First(text => text.text == "ACCUSE"), module, Magnifier.Default);

        }

        public static Translator translator = null;

        public static void UpdateDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMeshes(__instance.GetValue<TextMesh[]>("Display"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }
    }
}