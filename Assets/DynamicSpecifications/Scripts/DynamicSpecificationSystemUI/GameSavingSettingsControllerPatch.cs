using System.Reflection;
using HarmonyLib;
using TimberApi.DependencyContainerSystem;
using UnityEngine.UIElements;

namespace DynamicSpecifications
{
    [HarmonyPatch]
    public class SettingsPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GameSavingSettingsController"), "InitializeAutoSavingOnToggle", new []
            {
                typeof(VisualElement)
            });
        }
        
        static void Postfix(ref VisualElement root)
        {
            DependencyContainer.GetInstance<DynamicSpecificationSystemUI>().InitializeSelectorSettings(ref root);
        }
    }
}