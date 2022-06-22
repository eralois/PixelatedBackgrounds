using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace PixelatedBackgrounds;

public class PixelatedBackgrounds : Mod {
	// https://github.com/FNA-XNA/FNA/blob/master/src/Graphics/SpriteBatch.cs
	private static readonly FieldInfo _SamplerStateField = typeof(SpriteBatch).GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic);

	public override void Load() {
		if (_SamplerStateField == null)
			throw new Exception("Couldn't get _SamplerStateField");

        On.Terraria.Main.DrawBG += HookDrawBG;
	}

    public override void Unload() {
		On.Terraria.Main.DrawBG -= HookDrawBG;
	}

    private void HookDrawBG(On.Terraria.Main.orig_DrawBG orig, Main self) {
		// Override vanilla's LinearClamp every frame
		_SamplerStateField.SetValue(Main.spriteBatch, SamplerState.PointClamp);
		orig(self);
	}
}