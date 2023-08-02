# NFSToolHB

<b>NFSLocaleToolHB</b> (Need for Speed Locale Tool Histogram Binary) is a tool designed to decompile and compile localization's binary and histogram chunk files for games based on Frostbite game engine.

## Examples of usage

### [Need for Speed: Unbound — Russian Localization](https://nfsmods.xyz/mod/4749)

The aforementioned game lacks Russian localization since the release. The game also has poor support of Cyrillic characters — only handful of them were encoded into the game for a simple task of displaying songs' track names, rendering the ability to edit the in-game text in [Frosty Tool Suite](https://github.com/CadeEvs/FrostyToolsuite/tree/1.0.7) impossible.
<br>NFSLocaleToolHB solves this issue by editing all of the files outside of Frosty Editor and importing them back afterwards, [bypassing all checks for histogram characters encoding integrity during mod's compilation](https://cdn.discordapp.com/attachments/951527850727600158/1136347084941963404/image.png). Also, having working Ё and ё is a welcome addition, which official localizations lack and overlook during the development.

## Usage

1. Generate a list of characters:
```
  NFSLocaleToolHB -cl chars_list.txt
```
2. Export the game's locale histogram chunk, then compile a new histogram chunk using generated list of characters and game's histogram chunk:
```
  NFSLocaleToolHB -hg histogram.chunk chars_list.txt newhistogram.chunk
```
3. Extract the locale text from game's binary chunk using new histogram chunk:
```
  NFSLocaleToolHB -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt
```
You will be provided with .ids and .txt files that contain hash names and in-game text tied to hashes respectively.

4. Edit whatever you need to edit.
<br>If you're adding a new string into a text, make sure its hash is placed in its appropriate place (and not in the end of file), or else you risk mixing up all of the in-game text and it ending up in inappropriate places.

6. Compile a binary chunk with new edits using new histogram and edited text file:
```
  NFSLocaleToolHB.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk
```
6. All you have to do next is import new chunks into the game's database via Frosty Editor and edit `BinaryChunkSize` in game's `FsUITextDatabase` language asset.
<br>Make sure <b>FsLocalization Loader (FsLocalizationPlugin)</b> and <b>Localized String Editor (LocalizedStringPlugin) plugins</b> are not present in Frosty Editor, or else it crashes during the export.

## Documentation

### Binary chunk file structure

| Name           | Data type      | Size (in bytes)           | Comment                       |
| -------------- | -------------- | ------------------------- | ----------------------------- |
| magic          | uint32         | 4                         | 0x39000                       |
| fileSize       | uint32         | 4                         | file size - 8                 |
| listSize       | uint32         | 4                         | total amount of entries       |
| dataOffset     | uint32         | 4                         | start offset + 8 (hash pairs) |
| stringsOffset  | uint32         | 4                         | string list offset + 8        |
| section        | cstring        | CString (zero terminated) | section name                  |
| 000000         | byte           | 128                       | zero bytes                    |
| hash pair list | uint32, uint32 | 8                         | hash and string list offset   |
| string list    | cstring        | CString list              | collection of all strings     |

### Histogram chunk file structure

| Name           | Data type      | Size (in bytes)           | Comment                       |
| -------------- | -------------- | ------------------------- | ----------------------------- |
| magic          | uint32         | 4                         | 0x39001                       |
| fileSize       | uint32         | 4                         | file size - 8                 |
| dataOffSize    | uint32         | 4                         | 256                           |
| 000000         | byte           | 256                       | zero bytes                    |
| section        | uint16         | 256                       | section chars                 |
|                | byte           |                           | other                         |

## Credits

This repository contains code (which was edited for production purposes) from [NFSFBLocale](https://github.com/LinkOFF7/NFSFBLocale) made by [@LinkOFF7](https://github.com/LinkOFF7).
