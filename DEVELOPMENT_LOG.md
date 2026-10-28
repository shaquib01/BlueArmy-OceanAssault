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
- Added a Unity 2022.3 project version, package manifest including Unity UI, and GitHub Actions workflow for manual APK builds and tagged releases.
- Updated README with setup and release instructions.
- Added a safe preflight step to check that all required Unity CI secrets are present without printing their values.

### Release pipeline status
- Workflow correction commit: `ce472196d02f880e357e1616f52f70afb808827e` removed the unsupported `androidAppBundle` input, pinned Unity to `2022.3.62f1`, and enabled automatic runs on pushes to `main`.
- Latest workflow improvement commit: `30ac6403bb5b0e2e9de2f0043f7321d5704d35b8` adds a secret-presence preflight. This can distinguish missing secrets from Unity's own license activation failure; it does not expose secret values.
- Required GitHub Actions secrets: `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD`.
- **APK not yet verified as built or released.** The previously observed run logs showed Unity license activation failure. Fixing workflow syntax or confirming secrets exist does not prove the license is accepted by the CI runner.
- GameCI's current documentation confirms that a Personal-license CI build needs the activated `.ulf` file plus the Unity account email and password. If the account uses Google sign-in, ensure the Unity account has a usable password set through Unity ID and store it only as the `UNITY_PASSWORD` GitHub secret; never paste it into chat or source files.
- Official references: [GameCI Builder](https://game.ci/docs/github/builder/) and [GameCI Activation](https://game.ci/docs/github/activation/).
- No Unity Editor/runtime is available in this coding environment, so compilation, scene play testing, APK installation, and device performance remain unverified.

## Known issues / next work
1. Confirm the workflow preflight passes, then resolve any remaining Unity licensing error and get a successful Android build.
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
- [ ] GitHub Actions produces an installable APK as configured.
- [ ] Install and test on real Android hardware.

## Git workflow
Pull the latest source on the Windows computer with `git pull origin main`. Exclude Unity-generated folders such as `Library/`, `Temp/`, `Obj/`, `Logs/`, and local build output from source control.
