publish:
	rm -rf ~/chilly
	dotnet publish -c Release -r linux-x64 --self-contained true ChillyCgi.csproj 
	mv bin/Release/net5.0/linux-x64/publish ~/chilly
