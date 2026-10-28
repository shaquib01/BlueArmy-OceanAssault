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
- Coins are awarded for defeated targets and coin pickups.
- Losing all soldiers ends the run; defeating the boss wins it.
- Restart returns to a fresh run.

## Android APK build and releases

The GitHub Actions workflow at `.github/workflows/android-apk.yml` builds an APK on pushes to `main`, when manually started, or when a `v*` tag is pushed. Successful runs publish an APK artifact; tagged successful builds also attach `BlueArmy-OceanAssault.apk` to a GitHub Release.

### One-time Unity CI license setup

1. In Unity Hub, sign in to the Unity account that owns the Personal license.
2. Open **Unity Hub → Settings/Preferences → Licenses** and ensure the Personal license is activated. If necessary, use **Add → Get a free Personal license** so Unity creates the license file.
3. On Windows, GameCI documents the license file path as `C:\\ProgramData\\Unity\\Unity_lic.ulf`. Copy the *contents* of that file into the repository Actions secret `UNITY_LICENSE`.
4. Add/update `UNITY_EMAIL` (the Unity account email) and `UNITY_PASSWORD` (the account password) in **Repository → Settings → Secrets and variables → Actions**. Never commit these values or paste them into issues/chat.
5. Open **Actions → Android APK → Run workflow** and inspect the logs if it fails.

Official setup reference: [GameCI GitHub Activation](https://game.ci/docs/github/activation/). If the account uses Google sign-in, confirm the Unity account has a password usable for CI authentication; don't assume the Google login password is automatically available to GameCI.

The workflow configuration was corrected in commit `ce472196d02f880e357e1616f52f70afb808827e` to remove an unsupported action input and pin Unity 2022.3.62f1. A previous run showed a Unity license activation error, so no APK is claimed to exist until a workflow run succeeds. Install and test the APK on a physical Android device before calling it production-ready.

## Current prototype limitations

- Visuals are intentionally greybox primitives; final models, animation, sound, effects, onboarding, settings, save data, and device-performance tuning remain future work.
- Coins currently live for the current run; there is no persistent wallet or store yet.
- Gameplay balance and the Android build still require Unity Editor/runtime validation.
- If local input does not work in a project configured for the New Input System only, set **Active Input Handling** to **Both** in Player Settings.

## Development notes

See [DEVELOPMENT_LOG.md](DEVELOPMENT_LOG.md) for implementation changes, outstanding risks, and the validation checklist.
