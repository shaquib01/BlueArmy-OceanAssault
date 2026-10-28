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
- Added `.github/workflows/unity-activation-file.yml` (commit `0c7aab6769e998c02cd4835c2d8e6986c164c105`) to generate a GitHub-runner-specific Unity activation request file and upload it as a short-lived artifact.

### Release pipeline status
- Android workflow fix commit: `ce472196d02f880e357e1616f52f70afb808827e` removed the unsupported `androidAppBundle` input, pinned Unity to `2022.3.62f1`, and enabled automatic runs on pushes to `main`.
- Secret preflight commit: `30ac6403bb5b0e2e9de2f0043f7321d5704d35b8`.
- **APK not yet verified as built or released.** Earlier logs showed Unity license activation failure.
- Current GameCI documentation notes that manual Personal-license activation is no longer supported through the old route in some cases. The new activation workflow requests an `.alf` file for the GitHub runner; this file must be uploaded to Unity's license portal to obtain the matching `.ulf`, then its contents stored as the `UNITY_LICENSE` GitHub Actions secret. The Unity account email and password must also be configured as `UNITY_EMAIL` and `UNITY_PASSWORD`. Do not paste credentials or license contents into chat or commit them to the repository.
- Official references: [GameCI Activation](https://game.ci/docs/github/activation/) and [GameCI Builder](https://game.ci/docs/github/builder/).
- No Unity Editor/runtime is available in this coding environment, so compilation, scene play testing, APK installation, and device performance remain unverified.

## Known issues / next work
1. Run the new activation workflow, exchange its `.alf` artifact for a `.ulf` license via Unity's official portal, and update the GitHub secret.
2. Run the Android build and fix any licensing, compilation, or Android build errors that remain.
3. Test touch steering, gates, collisions, zero-squad defeat, boss victory, coins, and restart on Android.
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
