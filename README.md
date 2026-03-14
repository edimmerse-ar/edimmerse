## EdImmerse - Unity3D Project

**Unity Version:** 2022.3.16f1  
**Project Type:** Educational AR/AI application (Learn / Explore / Create)

---

## Project Overview

EdImmerse is an immersive educational experience that combines:

- **AR visualizations** of electronic components and circuits  
- **AI (AIMLBot)** for conversational guidance and explanations  
- **Arduino-style simulations** and interactive activities  

The app is structured around three major learning modes:

- **Learn**: Guided lessons and explanations  
- **Explore**: Free exploration of AR content and circuits  
- **Create**: DIY, puzzle, and quiz-based activities to apply learning  

---

## Getting Started

- **Requirements**
  - **Unity**: `2022.3.16f1` (LTS recommended)
  - **Platform**: Android (primary target), can be extended to others
  - **Dependencies**: Vuforia / AR Foundation, TextMesh Pro, AIMLBot, LeanTouch, and other plugins already included in the project

- **Opening the Project**
  1. Clone or copy the repository to your local machine.
  2. Open **Unity Hub**, click **Add**, and select the `EdImmerse_Unity3d` folder.
  3. Open the project with **Unity 2022.3.16f1**.

- **Running in Editor**
  1. Open the main entry scene (see **Scene Structure** below – typically `Loading` or `MainMenu` in `Assets/Scenes`).
  2. Click **Play** to run in the editor.

- **Building for Android**
  1. Go to **File > Build Settings…**.
  2. Switch platform to **Android**.
  3. Ensure the scenes listed under **Scenes In Build** follow the flow in **Scene Structure**.
  4. Click **Build** or **Build and Run**.

---

## High‑Level Folder Structure

The Unity project is organized roughly as follows (only key folders listed):

- **`Assets/`**
  - **`Scenes/`**: All gameplay and UI scenes (Loading, menus, AR scenes, etc.).
  - **`Scripts/`**: Main gameplay, UI, and manager scripts (e.g., `LevelManager`, `SettingsManager`, `ProgressBar`, etc.).
  - **`Art Assets/`**
    - **`2D Assets/`**: UI sprites, backgrounds, icons.
    - **`3D Assets/`**: 3D models of sensors, boards, and other hardware (`UltrasonicSensor.fbx`, `Servomotor.fbx`, etc.).
  - **`Model/`**: Additional 3D model variants (some duplicates of `Art Assets/3D Assets`).
  - **`AI Module/AIMLBot/`**
    - **`Scripts/`**: AIML bot controller scripts (`MainAIMLScript.cs`, `ImageOnTypingChange.cs`, etc.).
    - **AIML data** and related assets for the chatbot.
  - **`Sound/`**: Background music and SFX (`BGMU.mp3`, applause, etc.).
  - **`UI/`**: UI sprites and layout images (quiz UI, trophies, confetti, etc.).
  - **`Prefab/`**: Reusable prefab assets (buttons, dropdowns, characters like `Kendrick.prefab`).
  - **`Resources/Plugins/`**: Third‑party plugins and example content (e.g., LeanTouch, TextMesh Pro examples).

- **Project Root**
  - **`.sln` files**: Visual Studio / Rider solution files (prefer `EdImmerse.sln` as the main one).
  - **`APK/`**: Previously built APKs for testing and release.
  - **`QCAR/`**: Vuforia tracking data and configurations.

> **Note:** Some folders include historical or duplicate content that can be cleaned up; see the **Legacy Optimization Report** below for details.

---

## Scene Structure and Flow

The application flow is organized into a top‑level loading/menu layer and three major mode branches.

- **Boot & Main Menus**
  - **`Loading`**  
    - Initial loading / splash scene. Prepares resources and then goes to main menu.
  - **`MainMenu`**  
    - Central hub where the user chooses between Learn, Explore, and Create.
  - **`ModeMenu (Profile)`**  
    - Mode selection with user profile handling (e.g., progress, preferences).

- **Learn Mode**
  - **`LearnLoader`**
    - Handles loading of Learn mode assets and transitions.
  - **`LearnMenu`**
    - Lists available lessons / topics and learning paths.
  - **`Learn`**
    - Core Learn scene where lessons are experienced:
      - AR visualizations of circuits/components
      - AIMLBot guidance and explanations
      - Step‑by‑step learning flow

- **Explore Mode**
  - **`ExploreLoader`**
    - Loads Explore‑specific assets and prepares AR content.
  - **`ExploreMenu (Profile)`**
    - Explore mode landing, including profile‑aware options / unlocked content.
  - **`Explore1` – `Explore4`**
    - Multiple exploration scenarios, typically:
      - **`Explore1`**: Intro / basic components and circuits  
      - **`Explore2`**: Intermediate interactions or different hardware sets  
      - **`Explore3`**: Advanced or combined circuit explorations  
      - **`Explore4`**: Special scenarios / effects / extended content  

- **Create Mode**
  - **`CreateLoader`**
    - Loads assets and data for Create mode.
  - **`CreateMenu (Profile)`**
    - Entry scene for Create mode, with user profile‑based options.
  - **`DIY`**
    - Free‑form “build your own” experiences; users assemble or experiment with circuits/components.
  - **`Puzzle`**
    - Challenge‑based activities where users solve predefined circuit/logic puzzles.
  - **`Quiz`**
    - Question/answer flow to assess understanding; tied into the UI quiz assets.

---

## How It Works (Conceptual)

- **AR & Visual Layer**
  - Uses Vuforia / AR‑related components configured in the scenes to overlay 3D models of electronic components onto markers or surfaces.
  - Scenes like `Explore1–4` and `DIY` place these models in interactive layouts.

- **Game & UI Logic**
  - Manager scripts (e.g., `LevelManager`, `SettingsManager`, `CutSeenManager`, etc.) coordinate:
    - Scene transitions (via loaders and menus)
    - User progress and profile data
    - Tutorial / cut‑scene flows
    - HUD and quiz UI states

- **AI Module (AIMLBot)**
  - AIMLBot scripts (`MainAIMLScript.cs`, `RotationCube.cs`, `ImageOnTypingChange.cs`, etc.) drive:
    - Chatbot conversations and responses
    - Visual feedback (e.g., changing images / animations based on typing or bot state)

- **Learn / Explore / Create Modes**
  - **Learn** focuses on **structured guidance** (lessons, explanations, step‑by‑step flows).
  - **Explore** focuses on **free interaction** with AR content and circuits.
  - **Create** focuses on **application and assessment** (DIY builds, puzzles, quizzes).

---

## Web / Documentation

There is a companion web page (`edimmerse-ar.github.io/index.html`) which can be used as:

- **Landing page / overview** of the EdImmerse concept  
- **Documentation** entry point linking to screenshots, videos, and APK downloads  

Use this README as the primary reference for Unity project structure and the web page for public‑facing information.

---