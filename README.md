# Blue Army: Ocean Assault

A Unity 3D portrait runner prototype: lead a growing blue-soldier squad across an ocean bridge, choose add/multiply gates, automatically engage targets, and defeat the boss.

## Project requirements

- Unity **2022.3 LTS** with the **Android Build Support** module (Android SDK & NDK + OpenJDK).
- The repository contains the Unity project scaffold, source assets, and an Android build entry point.
- To open locally, clone this repository and open its root folder in Unity Hub.

## Play the prototype in the Editor

1. Open the project in Unity.
2. Wait for script compilation and package import.
3. Select **Tools → Blue Army → Build Prototype Scene**.
4. Open `Assets/Scenes/BlueArmyPrototype.unity` if it did not open automatically.
5. Press **Play**.

The scene builder creates the greybox environment and prefabs. It can also run headlessly in CI before an Android build.

## Gameplay

- Drag left/right with mouse or touch to steer the squad.
- Run through green **+N** and gold **×N** gates to grow the squad.
- Soldiers automatically fire at damageable obstacles and the boss.
- Coins are awarded for defeated targets and coin pickups; the wallet total is saved locally between runs with Unity PlayerPrefs, while the result panel shows the current run's earnings. This is local device storage, not cloud sync or tamper-proof storage.
- Losing all soldiers ends the run; defeating the boss wins it.
- Pause freezes gameplay and offers Resume or Restart; Escape / Android system Back also toggles pause during an active run. Switching away from the app or an Android interruption automatically pauses the run and saves the local wallet; resume manually when you return.

## Android APK build and releases

The GitHub Actions workflow at `.github/workflows/android-apk.yml` builds an APK on pushes to `main`, when manually started, or when a `v*` tag is pushed. Successful runs publish an APK artifact; tagged successful builds also attach `BlueArmy-OceanAssault.apk` to a GitHub Release.

### One-time Unity CI license setup

1. In Unity Hub, sign in to the Unity account that owns the Personal license.
2. Open **Unity Hub → Settings/Preferences → Licenses** and ensure the Personal license is activated. If necessary, use **Add → Get a free Personal license** so Unity creates the license file.
3. On Windows, GameCI documents the license file path as `C:\\ProgramData\\Unity\\Unity_lic.ulf`. Copy the *contents* of that file into the repository Actions secret `UNITY_LICENSE`.
4. Add/update `UNITY_EMAIL` (the Unity account email) and `UNITY_PASSWORD` (the account password) in **Repository → Settings → Secrets and variables → Actions**. Never commit these values or paste them into issues/chat.
5. Open **Actions → Android APK → Run workflow** and inspect the logs if it fails.

Official setup reference: [GameCI GitHub Activation](https://game.ci/docs/github/activation/). If the account uses Google sign-in, confirm the Unity account has a password usable for CI authentication; don't assume the Google login password is automatically available to GameCI.

The workflow configuration was corrected in commit `ce472196d02f880e357e1616f52f70afb808827e` to remove an unsupported action input and pin Unity 2022.3.62f1. The Android APK build and artifact upload succeeded in [GitHub Actions run 40713146565](https://github.com/shaquib01/BlueArmy-OceanAssault/actions/runs/40713146565). The wallet-enabled build succeeded in [run 40715244952](https://github.com/shaquib01/BlueArmy-OceanAssault/actions/runs/40715244952). The latest pause-menu build succeeded in [run 40720613382](https://github.com/shaquib01/BlueArmy-OceanAssault/actions/runs/40720613382) on commit `0ab5172ce4199b892b90cc4167af9e659a72e1c5`; artifact ID `12830357569` (`BlueArmy-OceanAssault-Android-APK`) is available until 2026-11-27. The latest verified build, including the Android background auto-pause change, succeeded in [run 40724732179](https://github.com/shaquib01/BlueArmy-OceanAssault/actions/runs/40724732179) on commit `f749e30ec797c20c5814123b0c0cc032f6aa5d57`; artifact ID `12831619939` (`BlueArmy-OceanAssault-Android-APK`) is available until 2026-11-27. Open the relevant run, then download the artifact from its **Artifacts** section. The artifact expires after 30 days. A successful CI build is not the same as device validation: install and test the APK on physical Android hardware before calling it production-ready.

## Current prototype limitations

- Visuals still use greybox primitives, though bridge rails and ocean markers add depth; final models, animation, sound, effects, onboarding, and device-performance tuning remain future work.
- A persistent local coin wallet is implemented; there is no store or cloud-synced wallet yet.
- Gameplay balance and the Android build still require Unity Editor/runtime validation.
- If local input does not work in a project configured for the New Input System only, set **Active Input Handling** to **Both** in Player Settings.

## Development notes

See [DEVELOPMENT_LOG.md](DEVELOPMENT_LOG.md) for implementation changes, outstanding risks, and the validation checklist.
