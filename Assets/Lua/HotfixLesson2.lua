print("多函数替换")
print("协程函数替换")
print("属性和索引器")
xlua.hotfix(CS.HotfixMain,{
    Add = function (self,a,b)
        return a+b
    end,
    Speak =  function (a)
        print(a)
    end
})

xlua.hotfix(CS.HotfixTest,{
    --构造函数 热补丁固定写法
    [".ctor"] = function ()
        print("Lua热补丁构造函数")
    end,
    Speak = function (self,a)
        print("lua热更新说"..a)
    end
    --析构函数固定写法
    

})


util = require("xlua.util")
xlua.hotfix(CS.HotfixMain,{
    TestCoroutine = function (self)
        return util.cs_generator(function ()
            while true do
                coroutine.yield(CS.UnityEngine.WaitForSeconds(1))
                print("Lua打补丁后的协程函数")
            end
        end)
    end
})

xlua.hotfix(CS.HotfixMain,{
    
    set_Age = function (self,v)
        print("Lua重定向的属性"..v)
    end,
    get_Age = function (self)
        return 10;
    end
})