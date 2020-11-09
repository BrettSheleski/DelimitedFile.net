DOTNET=dotnet
CONFIG=Release
FRAMEWORK="netstandard2.1;netcoreapp3.1"
RUNTIME="linux-x64"
VERBOSITY="normal"

all: clean restore build package

clean:
	$(DOTNET) clean -v $(VERBOSITY)

restore:
	$(DOTNET) restore -v $(VERBOSITY)

build:
	$(DOTNET) build -c $(CONFIG) -v $(VERBOSITY)

package:
	$(DOTNET) pack -c $(CONFIG) -v $(VERBOSITY)
