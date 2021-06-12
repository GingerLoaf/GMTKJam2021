# Fog Of War

![Fog Of War](Images/fogofwar3.gif)

---

## Description

The fog of war is actually just a big cloudy texture on a flat plane that a mask is applied to in order to create the transparent zones.
There is a camera above the map that watches for objects in the `FogCutout` layer and renders their shape to a renderTexture mask.
Any number of units can be used to act as cutouts as long as they are in the correct layer.
Prefabs have been provided to simplify the setup process.

> The fog is set to render on top of all entities even if they are technically taller than the top of the fog plane.
> This helps conceal the entire level and only reveals what you truly discover.
> It is possible to have tall geometry rendered above the plane if need be but that should be discussed later on.

> The rendering layers `FogCutout` and `FogPlane` are used for this feature and should never be rendered by any other camera outside of the fog system

---

## Feature Prefabs

`Source/Art/GameReady/FogOfWar.prefab`

This prefab contains the fog volume that should be scaled to wrap your entire level.
The fog plane is clear in edit mode but will be fully cleared (visible) when playback begins.
All you need to do is place the prefab in your scene and size it to fit your needs.

![Fog Of War](Images/fogofwar4.gif)

`Source/Art/GameReady/FogCutout.prefab`

This prefab should be placed as a child to any entity that should carve out from the fog.
It uses a texture to carve out shapes on a camera that is set to never clear it's render, causing seen visuals to accumulate in the mask.
Place these as children to your enemies or wildlife and then press play to see them carving out shapes into the fog.

![Fog Of War](Images/fogofwar5.gif)