# EdImmerse - Unity3D Project Optimization Report

**Unity Version:** 2022.3.16f1  
**Project Type:** Educational AR/AI Application

---

## Project Overview

EdImmerse is an educational AR application that combines AI (AIMLBot), Arduino simulation, and interactive learning experiences.

---

## Critical Issues Found

### 1. APK Build Files (HIGH PRIORITY - Clean Up)

**Total: 17 APK files (~gigabytes of wasted space)**

| File | Recommendation |
|------|----------------|
| `APK/app_v2.apk` | DELETE - Old build |
| `APK/app_v3.apk` | DELETE - Old build |
| `APK/app_v4.apk` | DELETE - Old build |
| `APK/app_v5.apk` | DELETE - Old build |
| `APK/app_v6_50%_done.apk` | DELETE - Incomplete |
| `APK/explore.apk` | DELETE - Old build |
| `APK/final.apk` | DELETE - Duplicate |
| `UpdatedBuild.apk` | DELETE - Old build |
| `UpdatedBuildFinal.apk` | DELETE - Old build |
| `finallll.apk` | DELETE - Duplicate naming |
| `finalBuild27Oct.apk` | DELETE - Dated build |
| `FinalBuild03Nov.apk` | DELETE - Dated build |
| `final04oct.apk` | DELETE - Dated build |
| `Appai.apk` | DELETE - Test build |
| `apk1.apk` | DELETE - Test build |
| `apk2.apk` | DELETE - Test build |
| `apk build123.apk` | DELETE - Test build |
| `build/aaa.apk` | DELETE - Test build |

**Recommended:** Keep only the final production APK.

---

### 2. Duplicate 3D Models (MEDIUM PRIORITY)

| Original | Duplicate | Action |
|----------|-----------|--------|
| `Assets/Art Assets/3D Assets/UltrasonicSensor.fbx` | `Assets/Model/UltrasonicSensor.fbx` | DELETE duplicate |
| `Assets/Art Assets/3D Assets/Tempraturesensor.fbx` | `Assets/Model/Tempraturesensor.fbx` | DELETE duplicate |
| `Assets/Art Assets/3D Assets/Servomotor.fbx` | `Assets/Model/Servomotor.fbx` | DELETE duplicate |
| `Assets/Art Assets/3D Assets/LargePushButton.fbx` | `Assets/Model/LargePushButton.fbx` | DELETE duplicate |
| `Assets/Art Assets/3D Assets/FinalPotentiometer.fbx` | `Assets/Model/FinalPotentiometer.fbx` | DELETE duplicate |
| `Assets/Art Assets/3D Assets/FinalIRSensor.fbx` | `Assets/Model/FinalIRSensor.fbx` | DELETE duplicate |
| `Assets/buzzer.fbx` | `Assets/Model/buzzer.fbx` | DELETE duplicate |
| `Assets/untitled.fbx` | `Assets/Model/untitled.fbx` | DELETE duplicate |
| `Assets/untitled 1.fbx` | `Assets/Model/untitled 1.fbx` | DELETE duplicate |

---

### 3. Duplicate Audio Files (MEDIUM PRIORITY)

| Original | Duplicate | Action |
|----------|-----------|--------|
| `Assets/BGMU.mp3` | `Assets/Sound/BGMU.mp3` | DELETE one |
| `Assets/Scene1/applause-crowd-242638.mp3` | `Assets/Sound/applause-crowd-242638.mp3` | DELETE one |

---

### 4. Duplicate UI Images (MEDIUM PRIORITY)

| Original | Duplicate | Action |
|----------|-----------|--------|
| `Assets/trophy.png` | `Assets/UI/trophy.png` | DELETE one |
| `Assets/Question Screen (3).png` | `Assets/UI/Question Screen (3).png` | DELETE one |
| `Assets/Confetti - Butterfly.png` | `Assets/UI/Confetti - Butterfly.png` | DELETE one |
| `Assets/3333.png` | `Assets/UI/3333.png` | DELETE one |
| `Assets/222.png` | `Assets/UI/222.png` | DELETE one |
| `Assets/Arduino_Uno.001_diffuse.png` | `Assets/UI/Arduino_Uno.001_diffuse.png` | DELETE one |
| `Assets/EDImmerse Quiz UI (5).png` | `Assets/UI/EDImmerse Quiz UI (5).png` | DELETE one |
| `Assets/EDImmerse Quiz UI (6).png` | `Assets/UI/EDImmerse Quiz UI (6).png` | DELETE duplicate |

---

### 5. Duplicate Scripts (HIGH PRIORITY)

| Original | Duplicate | Action |
|----------|-----------|--------|
| `Assets/showCoin.cs` | `Assets/Scripts/showCoin.cs` | DELETE one |
| `Assets/initials.cs` | `Assets/Scripts/initials.cs` | DELETE one |
| `Assets/CollisionDetection.cs` | `Assets/Scripts/CollisionDetection.cs` | May be intentional - verify |
| `Assets/LookAtSC.cs` | `Assets/Scripts/LookAtSC.cs` | DELETE one |
| `Assets/ProgressBar.cs` | `Assets/Scripts/ProgressBar.cs` | DELETE one |
| `Assets/LevelManager.cs` | `Assets/Scripts/LevelManager.cs` | DELETE one |
| `Assets/SettingsManager.cs` | `Assets/Scripts/SettingsManager.cs` | DELETE one |
| `Assets/SettingsHandler.cs` | `Assets/Scripts/SettingsHandler.cs` | DELETE one |
| `Assets/GAMEMMAN.cs` | `Assets/Scripts/GAMEMMAN.cs` | DELETE one |
| `Assets/CutSeenManager.cs` | `Assets/Scripts/CutSeenManager.cs` | DELETE one |
| `Assets/AnimationController.cs` | `Assets/Scripts/AnimationController.cs` | DELETE one |
| `Assets/ActivateObjects.cs` | `Assets/Scripts/ActivateObjects.cs` | DELETE one |
| `Assets/AIMLBot/Scripts/RotationCube.cs` | `Assets/AI Module/AIMLBot/Scripts/RotationCube.cs` | DELETE one |
| `Assets/AIMLBot/Scripts/ImageOnTypingChange.cs` | `Assets/AI Module/AIMLBot/Scripts/ImageOnTypingChange.cs` | DELETE one |
| `Assets/AIMLBot/Scripts/MainAIMLScript.cs` | `Assets/AI Module/AIMLBot/Scripts/MainAIMLScript.cs` | DELETE one |

---

### 6. Duplicate Prefabs (LOW PRIORITY)

| Original | Duplicate |
|----------|-----------|
| `Assets/Prefab/Button.prefab` | `Assets/Scenes/Button.prefab` |
| `Assets/Prefab/Dropdown (Legacy).prefab` | `Assets/Dropdown (Legacy).prefab` |
| `Assets/Prefab/Text Popup.prefab` | `Assets/Resources/Plugins/TextMesh Pro/Examples & Extras/Prefabs/Text Popup.prefab` |
| `Assets/Prefab/Kendrick.prefab` | `Assets/Art Assets/2D Assets/AnimationSprites/Talking Kendric/Kendrick.prefab` |

---

### 7. Unused/Duplicate Solution Files

Multiple .sln files exist. Keep only:
- `EdImmerse.sln` (or the main project .sln)

Delete duplicates:
- `Circuit AR.sln`
- `EdImmerse version 1.5_09.sln`
- Any other additional .sln files

---

### 8. Unused Scenes (MEDIUM PRIORITY)

The project has ~45+ scenes. Some appear to be duplicates or unused:

**Potentially unused scenes (verify before deletion):**
- `Assets/Scenes/AR Sceans/Explore 3-old.unity` - Old version
- `Assets/Scenes/AR Sceans/Explore 3--1.unity` - Duplicate
- `Assets/Scenes/AR Sceans/Explore 3-old.unity` - Old version
- `Assets/Scenes/AR Sceans/Explore 4----.unity` - Unclear naming
- `Assets/Scenes/Effect Scene/confetti.unity` - Duplicate (also in Effect Scene folder)
- `Assets/Scenes/SampleScene.unity` - Default Unity scene

---

### 9. Unused Plugins/Examples

**LeanTouch Examples:** Many example scenes in `Assets/Resources/Plugins/CN/LeanTouch/Examples/` are likely unused. Consider removing unused example scenes if not needed.

---

### 10. QCAR Folder

- `QCAR/somedata16` - Vuforia data file (verify if still needed)
- `QCAR/lh` - Possibly left-hand tracking data

---

## Recommended Cleanup Actions

### Step 1: Backup Project
Before cleaning, create a backup or commit to git.

### Step 2: Delete APK Files
```bash
# Delete all APK files except the final one
rm APK/app_v*.apk
rm APK/explore.apk
rm APK/final.apk
rm UpdatedBuild*.apk
rm finallll.apk
rm final*.apk
rm Appai.apk
rm apk*.apk
rm "apk build123.apk"
rm build/aaa.apk
```

### Step 3: Delete Duplicate Scripts
```bash
# Remove duplicate .cs files at root Assets/ folder
rm Assets/showCoin.cs
rm Assets/initials.cs
rm Assets/LookAtSC.cs
rm Assets/ProgressBar.cs
rm Assets/LevelManager.cs
rm Assets/SettingsManager.cs
rm Assets/SettingsHandler.cs
rm Assets/GAMEMMAN.cs
rm Assets/CutSeenManager.cs
rm Assets/AnimationController.cs
rm Assets/ActivateObjects.cs
rm Assets/autoGoScene.cs
```

### Step 4: Delete Duplicate 3D Models
```bash
# Remove duplicate models from Model/ folder
rm Assets/Model/UltrasonicSensor.fbx
rm Assets/Model/Tempraturesensor.fbx
rm Assets/Model/Servomotor.fbx
rm Assets/Model/LargePushButton.fbx
rm Assets/Model/FinalPotentiometer.fbx
rm Assets/Model/FinalIRSensor.fbx
rm Assets/Model/buzzer.fbx
rm Assets/Model/untitled.fbx
rm "Assets/Model/untitled 1.fbx"
```

### Step 5: Delete Duplicate Audio
```bash
rm Assets/Sound/BGMU.mp3
rm Assets/Sound/applause-crowd-242638.mp3
```

### Step 6: Delete Duplicate UI Images
```bash
rm Assets/trophy.png
rm Assets/Question\ Screen\ \(3\).png
rm Assets/Confetti\ -\ Butterfly.png
rm Assets/3333.png
rm Assets/222.png
rm Assets/Arduino_Uno.001_diffuse.png
rm Assets/EDImmerse\ Quiz\ UI\ \(5\).png
rm Assets/EDImmerse\ Quiz\ UI\ \(6\).png
```

### Step 7: Unity Editor Cleanup
After file deletion, open Unity and:
1. **Window > Analysis > Editor Log** - Check for warnings
2. **Right-click in Project > Open C# Project** - Regenerate solution
3. **File > Build Settings** - Remove unused scenes
4. **Edit > Project Settings > Editor > Enter Play Mode Settings** - Enable "Enter Play Mode Options" for faster testing

---

## Additional Optimization Tips

### Asset Compression
- Use texture compression (ASTC for mobile)
- Enable mesh compression
- Use audio compression for sound effects

### Scene Management
- Implement additive scene loading
- Use Addressables for asset management
- Enable scene bundling

### Build Optimization
- Strip unused engine code (Managed Stripping Level: High)
- Enable compression (Brotli/Gzip)
- Use IL2CPP for better performance

---

## Summary

| Category | Files to Delete | Est. Space Savings |
|----------|-----------------|-------------------|
| APK Builds | 18 files | ~GBs |
| Duplicate Models | 9 files | ~10-50MB |
| Duplicate Audio | 2 files | ~5-10MB |
| Duplicate Images | 8 files | ~1-5MB |
| Duplicate Scripts | ~15 files | ~KB |
| Duplicate Solution Files | 2+ files | ~KB |

**Total Potential Savings:** Significant reduction in project size (primarily APK cleanup)

---

*Generated on: 2026-02-14*
