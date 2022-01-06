struct SomeOtherStruct {
    pub u16 a;
}

struct TestStruct {
    pub SomeOtherStruct other;
    pub int a;
    pub s8 b;
}

impl TestStruct {
    pub fn TestFunc() {
        println("Hello World!");
    }
}

TestStruct test;
test.other.a = 5;
test.a = 3;
test.b = 7;
printf("%d\n", (int)test.other.a);