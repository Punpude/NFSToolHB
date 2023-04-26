# NFSToolHB
<br>Need for Speed Unbound localization tool is an application to edit localization binary and histogram chunk files from games built with the Frostbite game engine.</br>
<br>Use cmd</br>
<br>Create CharsList: </br>
<br>   -cl outputcharstextfile</br>
<br>Create Histogram:</br>
<br>   -hg inputhistogramchunk inputcharstext outputhistogramchunkfile</br>
<br>Create Text:</br>
<br>   -t inputbinarychunk inputhistogramchunk outputtextfile</br>
<br>Create Binary:</br>
<br>   -b inputtextfile inputhistogramchunk inputidsfile outputbinarychunkfile</br>
<br>FrostbiteTool.exe -cl chars_list.txt</br>
<br>FrostbiteTool.exe -h histogram.chunk chars_list.txt newhistogram.chunk</br>
<br>FrostbiteTool.exe -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt</br>
<br>FrostbiteTool.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk</br>


## Binary chunk file structure

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

## Histogram chunk file structure

| Name           | Data type      | Size (in bytes)           | Comment                       |
| -------------- | -------------- | ------------------------- | ----------------------------- |
| magic          | uint32         | 4                         | 0x39001                       |
| fileSize       | uint32         | 4                         | file size - 8                 |
| dataOffSize    | uint32         | 4                         | 256                           |
| 000000         | byte           | 256                       | zero bytes                    |
| section        | uint16         | 256                       | section chars                 |
|                | byte           |                           | other                         |
