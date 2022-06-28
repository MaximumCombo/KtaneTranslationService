using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace TranslationService.ModuleTranslators
{
    class AdventureGameTranslator : ModuleTranslator
    {
        private readonly MethodInfo mUpdateStatDisplayPostfix;
        private readonly MethodInfo mUpdateInvDisplayPostfix;
        public AdventureGameTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mUpdateStatDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateStatDisplayPostfix(__instance));
            mUpdateInvDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateInvDisplayPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "UpdateStatDisplay"), null, new HarmonyMethod(mUpdateStatDisplayPostfix));
            harmony.Patch(AccessTools.Method(componentType, "UpdateInvDisplay"), null, new HarmonyMethod(mUpdateInvDisplayPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("AdventureGameModule");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => langCode == "ja" ? new Magnifier.VectorMagnifier(0.07f, 0.018f) : new Magnifier.VectorMagnifier(0.07f, 0.016176f) ;

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            AdventureGameTranslator.translator = translator;
            var text = module.GetComponentsInChildren<TextMesh>().First(text => text.text == "USE");
            translator.SetTranslationToMesh(text, module, Magnifier.Default);

        }

        public override void AwakeTranslation(KMBombModule module, Translator translator)
        {
            var texts = module.GetComponentsInChildren<TextMesh>().Where(text => text.text != "USE").ToArray();
            translator.SetTranslationToMeshes(texts, module, displayMagnifier(translator.langCode));
        }

        public static Translator translator = null;

        public static void UpdateStatDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextStatus"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }
        public static void UpdateInvDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null)
            {
                translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextInventory"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
            }
        }
    }
}
