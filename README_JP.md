# 説明

Unity 用のAssetImporterです.<br>
Import設定の管理/設定適応の自動化ツールです.<br>
Unity の機能で完結しているため、他の外部ライブラリに依存しません.<br>


# 導入
## UnityPackageManager
1. `Window` メニューにある `PackageManager` を選択します
2. ウィンドウ左上にある `Add packege from Git URL...`　を選択
3. 表示されるテキストエリアに `https://github.com/Graffity-X/UnityAssetImporter.git?path=AssetImporterProject/Assets` をコピペ
4. 問題がなければ生成ツールのコードがProjectに追加されます
5. (Optional) サンプルはPackageManager ウィンドウのImport Sample からDLできます

# 使い方
1. ProjectWindowで右クリックメニューにある `Create→Editor→Config→AssetImporter` と選択してください
    <img width="300" alt="01_Create" src="https://user-images.githubusercontent.com/4001760/142399770-f9e8e265-821b-412d-803c-b7162e8d3dee.png">

2. `"Assets/Editor/CustomAssetImporter/CustomImporter.asset"` に設定ファイルのScriptableObjectが生成されます
    <img width="275" alt="02_ScriptableObject" src="https://user-images.githubusercontent.com/4001760/142399780-a56533a3-432e-4a13-b58a-5a50e0aab6f4.png">

3. Inspector からImport設定を編集できます. なおImport設定は `Preset` ファイルが必要になります.<br>
   設定例はSampleをご確認ください

## Preset について
Unity2018から追加された機能でComponentの設定を保存/適応する機能です<br>
https://docs.unity3d.com/2018.4/Documentation/Manual/class-PresetManager.html

例) Image Component のraycast target をデフォルトでOffにする等

Presetが1つしかない場合はデフォルトPresetとして適応されますが、Presetファイルが2つ以上ある場合は<br>
デフォルトに設定されているものが適応されます.<br>
しかし、AudioClipでいえば、BGM/SE/VOICE等同じAudioClipでも設定を変えたい場合がありますし、<br>
Textureでいえば、SpriteなのかFBX用のテクスチャだったり、NormalMap、CubeMapなど複数の使い方があります.

本ツールはこのような複数Presetがある場合にどこにどれを適応するのかをルールベースで管理/適応ができます



# 付属サンプルについて
サンプルにはオーディオファイルや画像ファイルなど複数のアセットがはいっています.<br>
それぞれのアセットは個別のライセンスの下に使用されております.<br>

もしサンプルをそのまま使う場合は[ライセンスファイル](/LICENSE.md) をご確認いただいた上でご利用ください.

## オーディオファイル
全てのオーディオファイルは CC4.0 の下で作成しております.

## Textures

一部アイコンは [defaulticon.com](http://www.defaulticon.com/) より<br>
CC BY-ND 3.0 のライセンスの下で利用しております<br>
