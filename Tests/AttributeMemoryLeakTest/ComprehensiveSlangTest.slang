struct v2f
{
    int foobar;
}

int foo(int a)
{
    return a + 3;
}

[shader("vertex")]
v2f vert ()
{
    v2f res;
    res.foobar = foo(42);
    return res;
}

[shader("fragment")]
float4 frag (v2f i) : SV_Target
{
    return foo(i.foobar);
}

[shader("vertex")]
v2f vert2()
{
    v2f res;
    res.foobar = foo(42) + 1;
    return res;
}

[shader("fragment")]
float4 frag2(v2f i) : SV_Target
{
    return foo(i.foobar) + 2;
}