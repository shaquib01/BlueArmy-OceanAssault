# Blue Army: Ocean Assault — Development Log

## v0.2 — Greybox scene generation
- Added editor menu action to generate a primitive-based prototype scene.
- Added basic GameManager HUD and boss health wiring.
- Added automatic crowd movement, add/multiply gates, projectile damage, and a boss target.
- Added camera follow and setup documentation.
- Not verified in Unity Editor in this environment; open the scene and check Console before treating it as a tested build.

## Next milestone — First reliable playable loop
The following items are audit findings and planned work, not yet completed or tested:
1. End the run as a defeat when the squad reaches zero.
2. Support trigger and collision projectile impacts through one shared hit method, while ignoring the projectile owner.
3. Improve target acquisition so soldiers select valid damageable targets rather than stopping at unrelated colliders.
4. Ensure the generated UI has a compatible EventSystem/input module for restart buttons.
5. Wire coin pickups to an actual reward path, or remove the unused pickup.
6. Test boss entry, victory, defeat, restart, gates, and mouse/touch steering in Unity.

## Validation checklist
- [ ] Open in Unity 2022.3 LTS.
- [ ] Build the prototype scene from Tools → Blue Army → Build Prototype Scene.
- [ ] Enter Play mode without compile errors.
- [ ] Mouse and touch steering work.
- [ ] Add/multiply gates update squad count.
- [ ] Zero soldiers ends the run as defeat.
- [ ] Projectiles hit valid targets and cannot damage their owner.
- [ ] Boss defeat awards a reward and shows victory.
- [ ] Restart works and restores time scale.
- [ ] Build and test a Windows player.

## Git workflow
Keep source in GitHub and pull updates on the Windows computer using `git pull`. Exclude Unity-generated folders such as `Library/`, `Temp/`, `Obj/`, `Logs/`, and `Build/`.
