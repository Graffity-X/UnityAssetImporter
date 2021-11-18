# TL;DR

AssetImporter for Unity.

## ForJapanese
[こちら日本語ドキュメントです](/README_JP.md)

# **Table of Contents**

- [Introduction](#Introduction)
    - [How to add your project](#installation)
- [How to Use](#usage)


# Introduction

## Installation
1. Open UnityPackageManager Window from tool menu
2. Select `Add packege from Git URL...`
3. Enter `https://github.com/Graffity-X/UnityAssetImporter.git?path=AssetImporterProject/Assets`

# Usage
1. Create ConfigFile from ProjectMenu `Create→Editor→Config→AssetImporter`
<img width="300" alt="01_Create" src="https://user-images.githubusercontent.com/4001760/142399770-f9e8e265-821b-412d-803c-b7162e8d3dee.png">

2. Select "Assets/Editor/CustomAssetImporter/CustomImporter.asset"
<img width="275" alt="02_ScriptableObject" src="https://user-images.githubusercontent.com/4001760/142399780-a56533a3-432e-4a13-b58a-5a50e0aab6f4.png">

3. Add Rules<br>
	3-1. Setup target path<br>
	3-2. Set Preset for custom setting

For adding new rule, preset file is required to attach for each rules.<br>
If you don't know about preset files, please check samples.

## Preset
Preset is new feature from Unity2018.<br>
https://docs.unity3d.com/2018.4/Documentation/Manual/class-PresetManager.html

If only one preset for one category, it is enough to set the preset as a default preset.<br>
If there are more than two presets for one category( i.g. TextureImporterPreset(Sprite), TextureImporterPreset(NormalMap), ...),<br>
this tool is effective for adapting preset separately.



