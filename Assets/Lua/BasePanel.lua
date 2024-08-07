
Object:subClass("BasePanel")

BasePanel.panelObj = nil
--相当于一个字典 键为控件名 值为控件本身
BasePanel.controls = {}
BasePanel.isInitEvent = false

function BasePanel:Init(name)
    if self.panelObj == nil then
        self.panelObj = ABMgr:LoadRes("ui",name,typeof(GameObject))
        self.panelObj.transform:SetParent(Canvas,false)

        local allControls = self.panelObj:GetComponentsInChildren(typeof(UIBehaviour))

        --按照名字规则 去找控件
        for i = 0,allControls.Length-1 do
            local controlName = allControls[i].name
            if  string.find(controlName,"btn") ~= nil or
            string.find(controlName,"tog") ~= nil or
            string.find(controlName,"top") ~= nil or
            string.find(controlName,"img") ~= nil or
            string.find(controlName,"sv") ~= nil or
            string.find(controlName,"Text") ~= nil  then
                --利用反射 Type 得到 控件的类名
                local typeName = allControls[i]:GetType().Name

                --最终存储形式
                --{ btnRole = {Image = 控件, Button =控件}
                --  togItem = {Toggle = 控件}}

                if self.controls[controlName] ~= nil then
                    self.controls[controlName][typeName] = allControls[i]
                else
                    self.controls[controlName] = {[typeName] = allControls[i]}
                end
            end
        end 
    end
end

function BasePanel:GetControl(name,typeName)
    if self.controls[name] ~= nil then
        local sameNameControls = self.controls[name]
        if sameNameControls[typeName] ~= nil then
            return sameNameControls[typeName]
        end
    end
    return nil
end

function BasePanel:ShowMe(name)
    self:Init(name)
    self.panelObj:SetActive(true)
end

function BasePanel:HideMe()
    self.panelObj:SetActive(false)
end


