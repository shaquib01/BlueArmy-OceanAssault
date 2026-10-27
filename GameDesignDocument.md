# Blue Army: Ocean Assault — Game Design Document

## High concept
A 3D hyper-casual runner. A crowd of small blue toy soldiers automatically runs over a narrow bridge above the ocean. The player steers the crowd horizontally, selects gates that grow the squad, destroys obstacles, and fights a giant boss.

## Main loop
Run → steer → choose +N/×N gates → shoot hazards → reach boss → earn coins → replay.

## Controls
- Mouse: hold left button and drag horizontally.
- Touch: drag horizontally.
- Forward movement is automatic.

## Core systems
- Crowd controller: speed, bounds, formation and soldier count.
- Gate system: one-time additive or multiplicative squad changes.
- Shooter/projectile: automatic fire, collision and damage.
- Damageable: health and defeat event.
- Game manager: run state and coin counter.
- HUD: squad count, boss health and coins.
- Editor scene builder: creates a test scene from Unity primitives.

## Art direction
Bright low-poly toy aesthetic; blue soldiers, turquoise ocean, narrow bridge, contrasting gates, oversized boss. Combat should communicate hits through muzzle flashes, impact sparks, explosions and coin bursts.

## Acceptance criteria
- Crowd moves forward automatically.
- Mouse/touch drag steers horizontally and stays within bounds.
- Gate triggers once per run and changes crowd size.
- Soldiers fire when a valid target is in front.
- Projectile damages targets only once.
- Boss defeat updates HUD/reward once.
- Restart resets the run.

## Performance notes
The first version is for a small prototype crowd. Before increasing the crowd substantially, add pooling and avoid one expensive physics query per soldier per frame.
