# Project Overview
- Game Title: UniPix Compatibility Fix
- High-Level Concept: Fix Unity 6 compilation errors in UniPix.cs so the editor script compiles and the menu item appears under Window > UniPix.
- Players: N/A (Editor Tool)
- Inspiration / Reference Games: N/A
- Tone / Art Direction: N/A
- Target Platform: Unity Editor (Unity 6 / 6000.6.0f1)
- Screen Orientation / Resolution: N/A
- Render Pipeline: UniversalRP

# Game Mechanics
## Core Gameplay Loop
N/A (Editor Tool)

## Controls and Input Methods
Menu item: `Window > UniPix`.

# UI
EditorWindow UI provided by UniPix.

# Key Asset & Context
- Target file: `Assets/UniPix/Editor/UniPix.cs`
- Line 1222:
  ```csharp
  // Current:
  textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
  // Needs to be updated to Unity 6 modern API:
  textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
  ```

# Implementation Steps
1. Modify `Assets/UniPix/Editor/UniPix.cs` at line 1222 to replace obsolete `TextureImporterFormat.AutomaticTruecolor` and `textureImporter.textureFormat` with `textureImporter.textureCompression = TextureImporterCompression.Uncompressed;`.
2. Allow Unity to recompile the editor scripts.
3. Verify that the compilation error disappears from the console.

# Verification & Testing
1. Check console logs to ensure 0 compilation errors exist.
2. Confirm `Window > UniPix` is compiled and available in the Unity Editor menu.
