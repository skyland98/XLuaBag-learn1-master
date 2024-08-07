

Object:subClass("ItemGird")

ItemGird.obj = nil
ItemGird.imgIcon = nil
ItemGird.Text = nil


--实例化对象
function ItemGird:Init(father,posX,posY)
    self.obj = ABMgr:LoadRes("ui","ItemGird")
    self.obj.transform:SetParent(father,false)
    self.obj.transform.localPosition = Vector3(posX,posY,0)

    self.imgIcon = self.obj.transform:Find("imgIcon"):GetComponent(typeof(Image))
    self.Text = self.obj.transform:Find("Text"):GetComponent(typeof(Text))
end


--初始化格子信息
--data指的是玩家拥有物品的信息
--itemData指的是物品信息
function ItemGird:InitData(data)
    local itemData = ItemData[data.id]
    local strs = string.split(itemData.icon,"+")

    local spriteAtlas = ABMgr:LoadRes("ui",strs[1],typeof(SpriteAtlas))
    self.imgIcon.sprite = spriteAtlas:GetSprite(strs[2])

    self.Text.text = data.num

    
end


function ItemGird:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end


