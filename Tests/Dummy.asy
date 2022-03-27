fn add(int a, int b) -> int {
    llvm("add", a, b)
}

fn add(int a, int b, int c) -> int {
    llvm("add", llvm("add", a, b), c)
}

printf("%d\n", add((int)5, (int)7));
printf("%d\n", add((int)2, (int)2, (int)3));
println("Test!");