PlayerData = {}
--玩家数据，只做了背包功能

PlayerData.equips ={}
PlayerData.shengs ={}
PlayerData.items = {}

function PlayerData:Init()
    --道具信息 不管存本地还是服务器 都不会把道具所有信息存进去
    --道具ID和道具数量
    table.insert(self.equips,{id=1,num="Lv.10"})
    table.insert(self.equips,{id=2,num="Lv.20"})

    table.insert(self.shengs,{id=3,num="+8"})
    table.insert(self.shengs,{id=4,num="+0"})

    table.insert(self.items,{id=5,num=10})
    table.insert(self.items,{id=6,num=20})

end

PlayerData:Init()


