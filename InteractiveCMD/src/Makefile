# .net 2 path
CSC =Microsoft.Net.Compilers.2.9.0/tools/csc.exe
FILES = *.cs 
OUT= /out:debug/InteractiveCMD.exe
FLAGS = -optimize  /target:winexe
compile : 
	@mkdir -p debug
	$(CSC) $(OUT) $(FLAGS) $(FILES)

prerequests: 
	nuget.exe install Microsoft.Net.Compilers