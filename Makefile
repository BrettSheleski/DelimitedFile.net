DOTNET=dotnet
CONFIG=Release

all: package build

build:
	$(DOTNET) build -c $(CONFIG)

package:
	$(DOTNET) pack -c $(CONFIG)