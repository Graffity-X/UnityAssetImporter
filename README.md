# TL;DR

AssetImporter for Unity.


# Introduction
## UnityPackageManager
1. Open UnityPackageManager Window from tool menu
2. Select `Add packege from Git URL...`
3. Enter `https://github.com/Graffity-X/UnityAssetImporter.git?path=AssetImporterProject/Assets/Editor`

# How to Use
1. Create ConfigFile from ProjectMenu
	Create→Editor→Config→AssetImporter
2. Select "Assets/Editor/CustomAssetImporter/CustomImporter.asset"
3. Add Rules
	3-1. Setup target path
	3-2. Set Preset for custom setting

## Preset
Preset is new feature from Unity2018.
https://docs.unity3d.com/2018.4/Documentation/Manual/class-PresetManager.html

If only one preset for one category, it is enough to set the preset as a default preset.
If there are more than two presets for one category( i.g. TextureImporterPreset(Sprite), TextureImporterPreset(NormalMap), ...),
this tool is effective for adapting preset separately.



