set PATH=c:"\Program Files (x86)\Microsoft Visual Studio\VC98\bin"
set PATH=%PATH%;c:"\Program Files (x86)\Microsoft Visual Studio\VB98"
set PATH=%PATH%;c:"\Program Files\Microsoft Visual Studio\VC98\bin"
set PATH=%PATH%;c:"\Program Files\Microsoft Visual Studio\VB98"
vb6 /m Project1.vbp

set PATH=%PATH%;"D:\dev_works\VB6_works\한글버그_테스트용2\exefix"
editbin Project1.exe /SUBSYSTEM:WINDOWS,5.01