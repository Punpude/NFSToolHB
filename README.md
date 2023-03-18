# NFSToolHB
Need for Speed Unbound localization tool<br>
Create CharsList: <br>
   -cl outputcharstextfile<br>
Create Histogram:<br>
   -hg inputhistogramchunk inputcharstext outputhistogramchunkfile<br>
Create Text:<br>
   -t inputbinarychunk inputhistogramchunk outputtextfile<br>
Create Binary:<br>
   -b inputtextfile inputhistogramchunk inputidsfile outputbinarychunkfile<br>
FrostbiteTool.exe -cl chars_list.txt<br>
FrostbiteTool.exe -h histogram.chunk chars_list.txt newhistogram.chunk<br>
FrostbiteTool.exe -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt<br>
FrostbiteTool.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk
