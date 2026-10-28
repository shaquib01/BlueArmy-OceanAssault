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
- Added Unity 2022.3 project configuration and a GitHub Actions Android build workflow.
- Added a safe preflight step to check required Unity CI secrets without printing their values.
- Removed the experimental activation-file workflow because `game-ci/unity-request-activation-file@v2` is deprecated.
- Fixed the generated HUD font resource: Unity 2022.3 rejects `Arial.ttf`; scene generation now requests `LegacyRuntime.ttf`.

### Release pipeline status
- Android workflow fix commit: `ce472196d02f880e357e1616f52f70afb808827e` removed the unsupported `androidAppBundle` input, pinned Unity to `2022.3.62f1`, and enabled automatic runs on pushes to `main`.
- Secret preflight commit: `30ac6403bb5b0e2e9de2f0043f7321d5704d35b8`.
- Deprecated activation workflow removal: `69942a12c085a7cc43d7a3189d29ff85c9d29595`.
- CI logs confirmed the initial font blocker (`Arial.ttf`) and the subsequent fix in commit `fce1a93384a6a1f5f1e15e1c25feec8f3e3f8a3c`.
- The next run progressed past scene generation but failed with `UnityException: Target architecture not specified`. Build settings are now applied after prototype scene generation; Android is explicitly set to IL2CPP + ARM64 and checked before building, in commit `bb2098c5b65666e203625f7e2959661d2b45a799`.
- A subsequent workflow run for commit `de591e081d6e1dde11a01d4ef58589c387131c9b` passed the secret preflight and was in progress at the time of inspection. The font fix commit has triggered another run; inspect the Actions page for the latest result.
- **APK not yet verified as built or released.** Wait for a successful workflow and confirm the artifact exists before claiming an APK is available.
- GameCI documentation: [Activation](https://game.ci/docs/github/activation/) and [Builder](https://game.ci/docs/github/builder/). Never expose license contents or account credentials in chat or source control.
- No Unity Editor/runtime is available in this coding environment, so gameplay, APK installation, and device performance remain unverified.

## Known issues / next work
1. Inspect the workflow triggered by the font fix; fix the next concrete error if any.
2. Verify the APK artifact is produced, then install and smoke-test it on Android.
3. Test touch steering, gates, collisions, zero-squad defeat, boss victory, coins, and restart.
4. Add a persistent wallet/save system, pause/settings flow, audio, feedback, and tutorial.
5. Replace primitive visuals with original optimized low-poly models, animations, and VFX; profile memory and frame rate on low/mid-range Android devices.

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
- [ ] GitHub Actions produces an installable APK.
- [ ] Install and test on real Android hardware.

## Git workflow
Pull the latest source on the Windows computer with `git pull origin main`. Exclude Unity-generated folders such as `Library/`, `Temp/`, `Obj/`, `Logs/`, and local build output from source control.
