
--将Json数据读取到Lua中的表进行存储

local txt =ABMgr:LoadRes("json","ItemData",typeof(TextAsset))

local itemList = Json.decode(txt.text)
print(itemList[1].id..itemList[1].name)

ItemData ={}

for _, value in pairs(itemList) do
    ItemData[value.id] = value
end

for key, value in pairs(ItemData) do
    --print(key,value)
end
