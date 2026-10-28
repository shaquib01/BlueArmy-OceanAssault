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
- Removed the experimental activation-file workflow because the referenced `game-ci/unity-request-activation-file@v2` action is marked deprecated by its maintainers.

### Release pipeline status
- Android workflow fix commit: `ce472196d02f880e357e1616f52f70afb808827e` removed the unsupported `androidAppBundle` input, pinned Unity to `2022.3.62f1`, and enabled automatic runs on pushes to `main`.
- Secret preflight commit: `30ac6403bb5b0e2e9de2f0043f7321d5704d35b8`.
- Deprecated activation workflow removal: `69942a12c085a7cc43d7a3189d29ff85c9d29595`.
- **APK not yet verified as built or released.** Earlier logs showed Unity license activation failure.
- Current GameCI GitHub activation instructions recommend activating the Personal license in Unity Hub, ensuring the `.ulf` file exists, then storing its contents as `UNITY_LICENSE`, with account email and password in `UNITY_EMAIL` and `UNITY_PASSWORD`. On Windows the documented license path is `C:\\ProgramData\\Unity\\Unity_lic.ulf`. Since the user previously provided a local `Unity_lic.ulf`, the next practical step is to verify the GitHub secret matches the current active file and that the account email/password secrets are correct; do not expose secret values in chat or source control.
- Official references: [Current GameCI Activation](https://game.ci/docs/github/activation/), [GameCI Builder](https://game.ci/docs/github/builder/), and [deprecated activation action notice](https://github.com/game-ci/unity-request-activation-file).
- No Unity Editor/runtime is available in this coding environment, so compilation, scene play testing, APK installation, and device performance remain unverified.

## Known issues / next work
1. Verify the existing Unity Personal license file and GitHub secrets, then run the Android workflow and inspect the latest logs for the exact activation failure.
2. Fix any remaining licensing, compilation, or Android build errors.
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
