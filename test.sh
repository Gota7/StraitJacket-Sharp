mkdir --parents Tests/obj
mkdir --parents Tests/bin
rm -f Tests/obj/$1.bc
rm -f Tests/bin/$1.elf
rm -f Tests/obj/$1.ll
dotnet run --project StraitJacket/StraitJacket.csproj $1.asy
llvm-dis Tests/obj/$1.bc -o Tests/obj/$1.ll
clang -O2 Tests/obj/$1.bc -o Tests/bin/$1.elf
Tests/bin/$1.elf