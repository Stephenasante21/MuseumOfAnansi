# Museum van Anansi

Een interactieve, educatieve 3D-ervaring in Unity waarin spelers de Ghanese taal en cultuur ontdekken aan de hand van vier minigames, elk gekoppeld aan een bekende Ghanees-Belgische held of locatie.

---

## Inhoud

1. [Beschrijving](#beschrijving)  
2. [Features](#features)  
3. [Systeemvereisten](#systeemvereisten)  
4. [Installatie & Setup](#installatie--setup)  
5. [Projectstructuur](#projectstructuur)  
6. [Scenes & Flow](#scenes--flow)  
7. [Besturing & UI](#besturing--ui)  
8. [Audio Management](#audio-management)  
9. [Data & Save-systeem](#data--save-systeem)  
10. [Contributing](#contributing)  
11. [Licence](#licence)  

---

## Beschrijving

“Museum van Anansi” is een Unity-project dat jongeren met Ghanese roots in België spelenderwijs Twi-woordenschat, folklore en uitspraak aanleert. Elke minigame correspondeert met een virtueel museumexhibit en levert stukjes van het Anansi-standbeeld op.

---

## Features

- **4 Minigames**  
  - **Okomfo Anokye Quiz** (easy & hard)  
  - **Yaa Asantewaa Zoekt Schat** (easy & hard)  
- **Interactieve schilderijen** met hotspots  
- **Woordenboek-panel** met audio-voorbeelden uit `StreamingAssets/clips`  
- **Instellingenpaneel** (ESC) voor volume & quit  
- **AudioManager**: BGM per scene, SFX-one-shots  
- **MouseManager**: cursor lock/unlock per context  

---

## Systeemvereisten

- Unity 2020.3 LTS of hoger  
- Windows (PC build)  
- Min. 4 GB RAM, GPU met DirectX 11 ondersteuning  

---

## Installatie & Setup

1. Clone de repository:  
   ```bash
   git clone https://github.com/Stephenasante21/MuseumOfAnansi


## projectstructuur
Assets/
├── Adaptive Performance/ # (Unity package)
├── Audio/ # al je BGM en SFX-bestanden (.mp3, .wav, etc.)
│ ├── Archive.zip
│ ├── ASANTEWAA.mp3
│ ├── correct.mp3
│ ├── incorrect.mp3
│ ├── main menu.mp3
│ ├── okomfo.mp3
│ └── okomfo fast.mp3
├── Dictionary/ # je JSON-data voor woorden en zinnen
│ ├── words.json
│ └── sentences.json
├── Editor/ # editor-scripts (custom inspectors, etc.)
├── EndlessExistence/ # 3e-party asset “Endless Existence”
├── Fungus/ # 3e-party asset “Fungus”
├── FungusExamples/ # bijbehorende voorbeeldscènes
├── GameInputControllerIconsFree/ # icon-pack voor controls
├── models/ # al je 3D-model-mappen
│ ├── Akan drum/
│ ├── akrafena/
│ ├── Anansi/
│ ├── Asantewaa/
│ ├── calao/
│ ├── doll/
│ └── Okomfo/
├── Plugins/ # eventueel extra plug-ins
├── PolygonAncientEmpire/ # 3e-party asset “Polygon Ancient Empire”
├── Prefabs/ # al je prefab-collecties
│ ├── Paintings/ # schilderijen-prefabs voor Asantewaa
│ ├── Button.prefab
│ ├── DictionaryEntryItem.prefab
│ ├── InspectCamera.prefab
│ ├── Interactive.prefab
│ └── Settings.prefab
├── Resources/ # Resources-map (Json, sprites, etc.)
│ └── DictionaryData.json
├── Scenes/ # al je Unity-scènes
│ ├── MainMenu.unity
│ ├── LobbyScene.unity
│ ├── MuseumHub.unity
│ ├── Dictionary.unity
│ ├── Okomfo.unity
│ ├── OkomfoHard.unity
│ ├── Asantewaa.unity
│ └── AsantewaaHard.unity
├── scripts/ # al je C#-scripts
│ ├── AsantewaaGameController.cs
│ ├── AsantewaaHardGameController.cs
│ ├── AudioManager.cs
│ ├── AudioPlayer.cs
│ ├── DictionaryManager.cs
│ ├── DictionaryOpnener.cs
│ ├── FirstpersonController.cs
│ ├── FoundWordsManager.cs
│ ├── GameSettings.cs
│ ├── GameState.cs
│ ├── GameStatePublisher.cs
│ ├── HardOkomfoGameController.cs
│ ├── MainMenu.cs
│ ├── MouseManager.cs
│ ├── PaintingData.cs
│ ├── PlayerManager.cs
│ ├── PlayGame.cs
│ ├── SettingsManager.cs
│ ├── StatuePieceController.cs
│ └── WordChoiceGameController.cs
├── Settings/ # UI-layout, icons, sprites voor settings
├── SlimUI/ # 3e-party UI-asset “Slim UI”
├── StreamingAssets/ # audio-clips voor woordenboek (runtime load)
│ └── clips/
│ ├── sen1.wav
│ ├── sen2.mp3
│ └── …
├── TextMesh Pro/ # TextMeshPro-asset
├── TutorialInfo/ # eventuele documentatie in-editor
└── UI/ # overige UI-prefabs en sprites

## scenes--flow
MainMenu
- Play > MuseumHub

MuseumHub
- ESC > Settings / Quit
- Kies exhibit > laad minigame

Minigame (easy/hard)
- Speel quiz/puzzel/race > BGM overschakelen > timer start
- Winnen > piece vrijgeven, tijd opslaan > terug naar Hub

Dictionary (X)
- Pauzeert tijd, toont audio-woordenlijst, hervat BGM op sluiten

## besturing--ui
- ESC: open/sluit settings
- X: open/sluit woordenboek
- WASD / pijltjestoetsen: verkennen in Hub
- Linkermuisknop: hotspot/knop selecteren
- Cursor: lock/unlock via MouseManager

## audio-management
- AudioManager (singleton, DontDestroyOnLoad)
- bgmSettings: koppel per scene een AudioClip, volume & loop
- PlayBGMForScene(sceneName): automatisch call op sceneLoaded
- PauseBGM() / ResumeBGM() in DictionaryOpener

## data--save-systeem
- PlayerPrefs:
-- OkomfoTime, HardOkomfoTime, AsantewaaTime, HardAsantewaaTime

## contributing
- Fork de repo
- Maak een feature-branch: git checkout -b feature/Naam
- Commit je changes: git commit -am 'Voeg feature X toe'
- Push naar branch: git push origin feature/Naam
- Open een Pull Request

## licence
MIT License © Stephen Asante    