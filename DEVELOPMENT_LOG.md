# Blue Army: Ocean Assault — Development Log

## v0.3 — Gameplay reliability and Android build pipeline

### Implemented and committed
- Improved soldier target acquisition: soldiers now search raycast hits for the nearest valid `Damageable`, instead of stopping at the first unrelated collider.
- Paused-time guard prevents auto-shooters from continuing to fire when the run has ended.
- Added a shared coin reward API in `GameManager`; coin pickups now award their configured value once, and defeated targets award coins through the same API.
- Added a defeat result when the squad reaches zero (from the existing crowd-controller change).
- Added an EventSystem with a legacy input module to the generated HUD so the restart button can receive UI events.
- Added a headless-safe scene-generation entry point for CI.
- Added `Assets/Editor/BuildAndroid.cs`, a Unity build method that generates the prototype scene and produces an ARM64 Android APK.
- Added a Unity 2022.3 project version, package manifest, and GitHub Actions workflow for manual APK builds and tagged releases.
- Updated README with setup and release instructions.

### Release pipeline status
- Workflow and build entry point are committed and the file contents were re-fetched from GitHub for verification.
- **APK not yet built or released.** A licensed Unity Editor/Android build runner is required. Configure the repository Actions secret `UNITY_LICENSE`, run the Android APK workflow, and inspect the build logs. A `v*` tag publishes the APK as a GitHub Release after a successful build.
- No Unity Editor/runtime is available in this coding environment, so compilation, scene play testing, APK installation, and device performance remain unverified.

## Known issues / next work
1. Run a licensed CI build and fix all Unity compilation/build errors it reports.
2. Test touch steering, gates, collisions, zero-squad defeat, boss victory, coins, and restart on Android.
3. Add a persistent wallet/save system, pause/settings flow, audio, feedback, and tutorial.
4. Replace primitive visuals with original optimized low-poly models, animations, and VFX.
5. Profile memory and frame rate on low/mid-range Android devices before release.

## Validation checklist
- [ ] Unity 2022.3 LTS opens project without package or compiler errors.
- [ ] Build the prototype scene from Tools → Blue Army → Build Prototype Scene.
- [ ] Enter Play mode without compile errors.
- [ ] Mouse and touch steering work.
- [ ] Add/multiply gates update squad count.
- [ ] Zero soldiers ends the run as defeat.
- [ ] Projectiles hit valid targets and cannot damage their owner.
- [ ] Boss defeat awards coins and shows victory.
- [ ] Coin pickup adds its value once.
- [ ] Restart works and restores time scale.
- [ ] GitHub Actions produces a signed/unsigned installable APK as configured.
- [ ] Install and test on real Android hardware.

## Git workflow
Pull the latest source on the Windows computer with `git pull origin main`. Exclude Unity-generated folders such as `Library/`, `Temp/`, `Obj/`, `Logs/`, and local build output from source control.
