build:
	dotnet build

build-release:
	dotnet build -c Release

pack:
	dotnet pack -c Release

test:
	# Test, normal verbositry, collect cov, output as opencover
	dotnet test ./kin-sdk-tests -p:CollectCoverage=true -p:CoverletOutputFormat=opencover

upload:
	@dotnet nuget push kin-sdk/bin/Release/kin-sdk*.nupkg -k $(KEY) -s https://api.nuget.org/v3/index.json

clean:
	rm -rf */bin */obj

