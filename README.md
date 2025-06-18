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
12. [Bronnen](#Bronnen)

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

## Bronnen

### Wetenschappelijke en culturele bronnen

- Adjaye, J. K. (1999). *Ghanaian Popular Culture: Cultural Institutions and Oral Narratives*. Indiana University Press.  
- Amoah, A. (2015). Cultural retention and loss among Ghanaian migrant youth in Europe. *Migration and Society Journal*.  
- Arandjelovic, R., et al. (2017). Look, listen and learn: Bridging cultural gaps through digital interaction. *ALT Open Access*.  
- Atlantis-Press. (z.d.). *Learning heritage language through serious games in migrant contexts*. Geraadpleegd van https://www.atlantis-press.com/proceedings/ic…  
- Boakeng, A. (2016). Diaspora identity formation and the role of Ghanaian churches abroad. *African Diaspora Journal of Religion*.  
- de Opleidingen. (z.d.). Vergelijk cursussen Akan Twi bij verschillende aanbieders. Geraadpleegd van https://deopleidingen.be/opleidingen/ak…  
- DBNL. (z.d.). *Ghanese pinkstergemeenten en kosmopolitische identiteiten in …* Geraadpleegd van https://www.dbnl.org/tekst/hovi002c…  
- Fishman, J. A. (1991). *Reversing language shift: Theoretical and empirical foundations of assistance to threatened languages*. Multilingual Matters.  
- Guardado, M. (2002). Loss and maintenance of first language skills: Case studies of Hispanic families in Vancouver. *Canadian Modern Language Review, 59*(4), 499–514.  
- Gyekye, K. (1996). *African Cultural Values: An Introduction*. Sankofa Publishing.  
- Histories. (z.d.). *Project onder de loep: Histories*. Geraadpleegd van https://histories.be/wp-content/upl…  
- ICvzw. (z.d.). Ghana Welfare Association vzw – Leden – Internationaal Comité. Geraadpleegd van https://icvzw.be/verenigingen/o…  
- Khan, K., Gupta, S., & Singh, R. (2015). Enhancing engagement and learning through interactive digital media in education. *Interactive Learning Environments, 23*(4), 457–468.  
- LearnAkanDictionary.com. (z.d.). English–Twi/Voice. Geraadpleegd van https://learnakandictionary.com/english-twi/voice/  
- Leerplein Zwolle. (z.d.). De-25-leukste-apps-waar-je-kind-echt-iets-van-leert. Geraadpleegd van https://leerpleinzwolle.nl/kennisbank/ond…  
- Levitt, P., & Waters, M. C. (2002). *The changing face of home: The transnational lives of the second generation*. Harvard University Press.  
- Lefstein, A., & Snell, J. (2014). *Better than best practice: Developing teaching and learning through dialogue*. Routledge.  
- Ministry of Tourism, Arts and Culture Ghana. (z.d.). Folklore in Ghana. Geraadpleegd van https://www.motac.gov.gh/folklore/  
- Opoku, K. (1978). *West African Traditional Religion*. FEP International.  
- ProQuest OpenView. (2014). *Using interactive 3D simulations in education for increased engagement and learning*. Geraadpleegd van https://pmc.ncbi.nlm.nih.gov/articles/PMC10611935/  
- Sankaa Koepelorganisatie VZW. (z.d.). Vereniging in de kijker: Asanteman Gent. Geraadpleegd van https://sankaa.be/vereniging-in-de-kijker-asanteman-gent  

### 3D-models

- Sketchfab – Znyth Technologies. (2019, mei 1). *Ga Mask* [3D-model]. Geraadpleegd van https://sketchfab.com/3d-models/ga-mask-605603355d714d5eadb2890d33bbf86f  
- Sketchfab – Världskulturmuseerna. (2023, maart 14). *Akuaba Fertility Doll Ghana* [3D-model]. Geraadpleegd van https://sketchfab.com/3d-models/akuaba-fertility-doll-ghana-74b205e048444e1b8388be1dd7584e78  
- Sketchfab – Alienor.org. (2023, april 7). *Figurine de Calao en laiton* [3D-model]. Geraadpleegd van https://sketchfab.com/3d-models/figurine-de-calao-en-laiton-a59452e649ee46d7ace0e2aadf986cbd  
- Sketchfab – Znyth Technologies. (2019, mei 3). *Contemporary Female Figure* [3D-model]. Geraadpleegd van https://sketchfab.com/3d-models/contemporary-female-figure-1157604b35cc44bb94f26cb04a2261c1  
- Sketchfab – Larry3d. (2022, mei 30). *Djembé – African Drum – Scan* [3D-model]. Geraadpleegd van https://sketchfab.com/3d-models/djembe-african-drum-scan-6db07a048621443992a8048dd62c026f  

### Tools & assets

- Unity Asset Store. (z.d.-a). *SurfaceSound Lite – Sound effects system for surface interaction*.  
- Unity Asset Store. (z.d.-b). *Easy Interaction System*.  
- Unity Asset Store. (z.d.-c). *Game Input Controller Icons (free)*.  

### Tutorials & code-snippets

- Alucard-Jay. (2018). *First person controller script* [Code]. GitHub Gist. Geraadpleegd van https://gist.github.com/Alucard-Jay/af368da64e66cc8afc9b939d58b5ec7b  
- Demigiant. (2021). DOTween issue #619 [Software-issue]. GitHub. Geraadpleegd van https://github.com/Demigiant/dotween/issues/619  
- Mike’s Code. (z.d.). *Import Custom Fonts to Unity (for TextMeshPro)* [Tutorial]. Geraadpleegd van https://www.youtube.com/watch?v=lrx-0VsotFM  
- ERIC. (z.d.). *De-25-leukste-apps-waar-je-kind-echt-iets-van-leert* [PDF]. Geraadpleegd van https://files.eric.ed.gov/fulltext/EJ1160637.pdf  

### Zakelijke en maatschappelijke context

- ICvzw. (z.d.). Geraadpleegd van https://icvzw.be/verenigingen/o…  

**Generative AI assistentie:**  
- Documentatie en code-snippets gegenereerd met hulp van ChatGPT (OpenAI).
