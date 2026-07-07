# Player Swapper

A [tModLoader](https://github.com/tModLoader/tModLoader) mod that lets you
swap between Terraria player saves in a running world — no rejoining. Swap
through a keybind menu or cycle saves directly, including across Journey/
non-Journey modes.

[Demo clip](https://i.imgur.com/uKPo4OI.mp4)

> **Status:** no longer maintained. Built on request for another modder who
> wanted a faster testing loop; it caught on — **~140k unique visitors** and
> **~50k subscribers** on the Steam Workshop.

It's deliberately small (~650 lines): hot-swapping the live player state and
re-syncing the game's character UI, and nothing else. Most of the work lives
in `PSPlayer.cs` (the swap itself) and `Content/UI/` (a draggable in-game
character picker adapted from the vanilla character-select list).
