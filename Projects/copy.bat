set filePath=%1
set copyTo=%2
set workingDir=%3

cd %workingDir%

set PDB2MDB=%workingDir%\..\..\..\pdb2mdb.exe

%PDB2MDB% %filePath%.dll

if not exist %copyTo% mkdir %copyTo%
if exist %filePath%.dll copy %filePath%.dll %copyTo%
if exist %filePath%.pdb copy %filePath%.pdb %copyTo%
if exist %filePath%.dll.mdb copy %filePath%.dll.mdb %copyTo%