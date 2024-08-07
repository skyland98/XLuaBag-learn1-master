BasePanel:subClass("MainPanel")



function MainPanel:Init(name)
    self.base.Init(self,name)

    --为了只添加一次事件监听
    if self.isInitEvent == false then
        print( self:GetControl("btnBag","Image"))
    self:GetControl("btnBag","Button").onClick:AddListener(self.BtnBagClick)

    self.isInitEvent = true
    end


end

function MainPanel:BtnBagClick()
    BagPanel:ShowMe("BagPanel")
end

