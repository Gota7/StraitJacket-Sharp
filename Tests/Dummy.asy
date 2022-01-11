struct SomeOtherStruct {
    pub u16 a;
}

struct TestStruct {
    pub SomeOtherStruct other;
    pub int a;
    pub s8 b;
}

impl TestStruct {
    pub fn TestFunc(This tmp) {
        printf("%d\n", (int)this.other.a);
    }
}

TestStruct test;
test.other.a = 7;
test.TestFunc(test);