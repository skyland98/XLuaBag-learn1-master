
BasePanel:subClass("BagPanel")

BagPanel.Content =nil

--格子
BagPanel.items={}

BagPanel.nowType = -1

function BagPanel:Init(name)

    self.base.Init(self,name)
    if self.isInitEvent == false then

        self.Content = self:GetControl("svBag","ScrollRect").transform:Find("Viewport"):Find("Content")

        self:GetControl("btnClose","Button").onClick:AddListener(function ()
            self:HideMe()
        end)

        self:GetControl("togEquip","Toggle").onValueChanged:AddListener(function (value)
            if value == true then
                self:ChangeType(1)
            end
        end)
        self:GetControl("topItem","Toggle").onValueChanged:AddListener(function (value)
            if value == true then
                self:ChangeType(2)
            end
        end)
        self:GetControl("topGem","Toggle").onValueChanged:AddListener(function (value)
            if value == true then
                self:ChangeType(3)
            end
        end)

        self.isInitEvent = true
    end

end

function BagPanel:ShowMe(name)
    self.base.ShowMe(self,name)
    
    if self.nowType==-1 then
        self:ChangeType(1)
    end
end



--type 1.装备 2. 3.
function BagPanel:ChangeType(type)

    if self.nowType == type then
        return
    end

    --销毁格子
    for i = 1, #self.items do
        self.items[i]:Destroy()
    end
    self.items = {}


    local nowItems = nil
    if type==1 then
        nowItems = PlayerData.equips

    elseif type==2 then
        nowItems = PlayerData.shengs

    else
        nowItems = PlayerData.items
        
    end

    --生成格子
    for i = 1, #nowItems do
        local grid = ItemGird:new()

        grid:Init(self.Content,(i-1)%4*175,math.floor((i-1)/4)*175)

        grid:InitData(nowItems[i])

        table.insert(self.items,grid)
    end


end






