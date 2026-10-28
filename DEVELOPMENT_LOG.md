# Blue Army: Ocean Assault — Development Log

## v0.3 — Gameplay reliability and Android build pipeline

## v0.4 — Persistent coin wallet

### Implemented and committed
- Added a persistent wallet backed by Unity PlayerPrefs. Coins earned from pickups and defeated targets now increase both the current-run reward and the saved wallet balance.
- Capped the wallet with `System.Math.Min` using integer arithmetic to avoid floating-point precision loss when balances become very large.
- The HUD now labels the saved total as `WALLET`; the end-of-run panel reports coins earned in that run, so the two values are not conflated.
- Wallet writes are saved immediately and flushed again when the app pauses or quits.
- Code commits: `d0484bf9e2d43908b2c03cff8cc025ed00e83feb` (wallet) and `d9f444e1e4ee4596c2324d54ccf7349e4e15844e` (integer-safe balance cap).

### Validation status
- This change has been committed but its new Android CI build and runtime behavior still need verification.
- PlayerPrefs persistence has not yet been tested across app force-stop/relaunch on a physical Android device.


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
- The earlier workflow runs passed the Unity secret preflight but failed during the Unity build. Latest run logs showed `Assets/Editor/BuildAndroid.cs(21,44): error CS0103: The name 'NamedBuildTarget' does not exist in the current context`; this was a C# namespace/import issue, not a licensing failure.
- The ARM64/IL2CPP follow-up run exposed a compile error: `NamedBuildTarget` is declared in `UnityEditor.Build`. Added that namespace import in commit `73032f3cc6aa9dedcaf076ee39f4a4da18a081ca`; the next Actions run must confirm compilation and reveal any subsequent blocker.
- **APK build verified:** workflow run [40713146565](https://github.com/shaquib01/BlueArmy-OceanAssault/actions/runs/40713146565) completed successfully on commit `73032f3cc6aa9dedcaf076ee39f4a4da18a081ca`; Unity build and artifact publish steps both passed. Artifact `BlueArmy-OceanAssault-Android-APK` (artifact ID `12827308230`, about 10.4 MiB) is available until 2026-11-27. The follow-up log-only commit also built successfully in [run 40713158572](https://github.com/shaquib01/BlueArmy-OceanAssault/actions/runs/40713158572), producing artifact ID `12826509479`.
- **Important validation boundary:** CI success verifies that Unity produced and uploaded the APK; it does not prove installability or runtime correctness on a physical device. APK installation, gameplay, touch input, and device performance remain untested.
- GameCI documentation: [Activation](https://game.ci/docs/github/activation/) and [Builder](https://game.ci/docs/github/builder/). Never expose license contents or account credentials in chat or source control.
- No Unity Editor/runtime is available in this coding environment, so gameplay, APK installation, and device performance remain unverified.

## Known issues / next work
1. Install the published APK on Android and smoke-test launch, orientation, touch input, and restart.
2. Test gates, collisions, zero-squad defeat, boss victory, coins, and restart; capture any crash/logcat output.
3. Fix any reproducible runtime issues and verify a new APK artifact.
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
