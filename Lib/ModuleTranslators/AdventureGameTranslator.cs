﻿using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace TranslationService.ModuleTranslators
{
    class AdventureGameTranslator : ModuleTranslator
    {
        private readonly MethodInfo mUpdateStatDisplayPostfix;
        private readonly MethodInfo mUpdateInvDisplayPostfix;
        private readonly MethodInfo mOnActivatePostfix;
        public AdventureGameTranslator(Harmony harmony)
        {
            mUpdateStatDisplayPostfix = SymbolExtensions.GetMethodInfo((object __instance) => UpdateStatDisplayPostfix(ref __instance));
            mUpdateInvDisplayPostfix = SymbolExtensions.GetMethodInfo((object __instance) => UpdateInvDisplayPostfix(ref __instance));
            mOnActivatePostfix = SymbolExtensions.GetMethodInfo((object __instance) => OnActivatePostfix(ref __instance));
            harmony.Patch(AccessTools.Method(componentType, "UpdateStatDisplay"), null, new HarmonyMethod(mUpdateStatDisplayPostfix));
            harmony.Patch(AccessTools.Method(componentType, "UpdateInvDisplay"), null, new HarmonyMethod(mUpdateInvDisplayPostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnActivate"), null, new HarmonyMethod(mOnActivatePostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("AdventureGameModule");

        public override IEnumerator StartTranslation(KMBombModule module, Translator translator)
        {
            yield return translator;
            AdventureGameTranslator.translator = translator;
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module.ModuleDisplayName, Magnifier.Default);
            while(true)
            {
                Debug.Log(module.GetComponent(componentType).GetValue<TextMesh>("TextStatus").GetComponent<MeshRenderer>().bounds.size.ToString("F6"));
                yield return new WaitForSecondsRealtime(1f);
            }

        }

        public static Translator translator = null;

        public static void UpdateStatDisplayPostfix(ref object __instance)
        {
            if(translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextStatus"), "Adventure Game", new Magnifier.VectorMagnifier(0.07f, 0.025f, 0));
        }
        public static void UpdateInvDisplayPostfix(ref object __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextInventory"), "Adventure Game", new Magnifier.VectorMagnifier(0.07f, 0.025f, 0));
        }
        public static void OnActivatePostfix(ref object __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextEnemy"), "Adventure Game", new Magnifier.VectorMagnifier(0.07f, 0.025f, 0));
        }
    }
}
