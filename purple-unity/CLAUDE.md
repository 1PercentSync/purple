# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 6000.2.2f1 project called "purple-unity" using the Universal Render Pipeline (URP). It includes Unity's Starter Assets for first-person and third-person controllers, providing a foundation for character-based games.

## Project Structure

The project follows Unity's standard folder structure:
- `Assets/` - All game assets and scripts
  - `Assets/Starter Assets/` - Unity's official starter assets for character controllers
    - `Runtime/` - Core runtime scripts and components
      - `FirstPersonController/` - First-person character controller system
      - `ThirdPersonController/` - Third-person character controller system  
      - `Common/` - Shared utilities and components
      - `InputSystem/` - Input handling components
      - `Mobile/` - Mobile-specific UI and controls
      - `Settings/` - Configuration assets
  - `Assets/Scenes/` - Unity scene files (currently contains SampleScene.unity)
  - `Assets/Settings/` - Project rendering and quality settings
- `ProjectSettings/` - Unity project configuration
- `Packages/` - Package Manager dependencies and manifest

## Key Dependencies

The project uses these major Unity packages:
- **Universal Render Pipeline (URP) 17.2.0** - Modern rendering pipeline
- **Cinemachine 3.1.2** - Advanced camera system
- **Input System 1.14.2** - New Unity input handling
- **Collections 2.5.7** - High-performance data structures

## Development Commands

Unity projects are typically built and run through the Unity Editor, not command line tools. However, for CI/CD or automation:

### Build Commands
```bash
# Build for Windows (requires Unity Editor in PATH)
Unity.exe -batchmode -quit -projectPath . -buildWindows64Player ./Build/purple-unity.exe

# Build for other platforms
Unity.exe -batchmode -quit -projectPath . -buildTarget <target> -buildPath ./Build/
```

### Unity Editor
- Open project in Unity 6000.2.2f1 or later
- Main scene: `Assets/Scenes/SampleScene.unity`
- Play mode testing available through Unity Editor

## Architecture Notes

### Character Controllers
The project includes both first-person and third-person character controller systems from Unity's Starter Assets:
- Character movement, jumping, and ground detection
- Camera follow and look controls
- Input handling through Unity's new Input System
- Mobile touch controls for mobile builds

### Input System
Uses Unity's modern Input System with:
- `Assets/InputSystem_Actions.inputactions` - Central input action definitions
- Support for keyboard, mouse, gamepad, and touch input
- Event-driven input handling

### Rendering Pipeline
Configured for Universal Render Pipeline (URP):
- Modern, performant rendering suitable for multiple platforms
- Mobile-optimized rendering settings
- Post-processing and lighting configured for URP

## Visual Studio Integration

The project includes `.vsconfig` specifying the "ManagedGame" workload for optimal C# development in Visual Studio.

## Git Branch Structure

- `main` - Primary development branch
- `first_person` - Current working branch (likely focused on first-person features)

## Important Files

- `Assets/InputSystem_Actions.inputactions` - Input action definitions
- `Assets/Starter Assets/Runtime/Unity.StarterAssets.asmdef` - Assembly definition for starter assets
- `Packages/manifest.json` - Package dependencies
- `ProjectSettings/ProjectSettings.asset` - Unity project configuration