using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using UnityEngine;
[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace FPFCToggle
{
    internal class PluginConfig
    {
        public virtual KeyCode ToggleKey { get; set; } = KeyCode.F;

        public virtual void OnReload()
        {
            FPFCController.ToggleKey = ToggleKey;
        }

        public virtual void Changed()
        {
            FPFCController.ToggleKey = ToggleKey;
        }
    }
}
