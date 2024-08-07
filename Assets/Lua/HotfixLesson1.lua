print("第一个热补丁")


xlua.hotfix(CS.HotfixMain,"Add",function (self,a,b)
    return a + b
end)

xlua.hotfix(CS.HotfixMain,"Speak",function (a)
    print(a)
end)

