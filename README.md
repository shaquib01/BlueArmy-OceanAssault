# Blue Army: Ocean Assault — Unity Prototype Kit v2

This package adds a **primitive-based scene builder** so you can generate a playable greybox prototype inside Unity without downloading art assets.

## Requirements
- Unity 2022.3 LTS or later
- Create a new **3D Core** project
- Copy the `Assets` folder from this kit into your Unity project's root `Assets/` folder
- Wait for scripts to compile
- Select **Tools → Blue Army → Build Prototype Scene**
- Open `Assets/Scenes/BlueArmyPrototype.unity` and press Play

## Prototype includes
- Blue soldier crowd with automatic forward movement and mouse/touch drag steering
- Add and multiply gates
- Automatic soldier firing
- Damageable boss and obstacles
- Basic HUD: crowd count, boss health, coins
- Simple ocean, bridge, gate visuals, camera and lights made from Unity primitives

## Controls
Drag left/right with mouse or touch. The squad moves forward automatically.

## Known prototype limitations
- Greybox visuals only; no final models, animations, sound or polished VFX.
- Combat and boss encounter are a functional starting point, not balanced final gameplay.
- Built-in Unity input API is used; if your project is configured for the new Input System only, set **Active Input Handling** to **Both** in Player Settings.
- Not tested in a Unity editor/runtime in this environment. Please open the project in Unity and check the Console.

## Suggested next milestones
1. Verify scene generation and fix any Unity console errors.
2. Add object pooling for soldiers/projectiles/coins.
3. Improve target selection and make the boss phase begin at the end of the bridge.
4. Replace primitives with original low-poly soldier, ocean, gate and boss assets.
5. Add mobile build profiles and device testing.
