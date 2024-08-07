
require("Object")
require("SplitTools")
Json = require("JsonUtility")

--Unity
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform =CS.UnityEngine.RectTransform
TextAsset = CS.UnityEngine.TextAsset

--图集对象类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas

Vector3 =CS.UnityEngine.Vector3
Vector2 =CS.UnityEngine.Vector2

--UI
UI = CS.UnityEngine.UI
Image = UI.Image
Text = UI.Text
Button =UI.Button
Toggle =UI.Toggle
ScrollRect =UI.ScrollRect
UIBehaviour = CS.UnityEngine.EventSystems.UIBehaviour

Canvas = GameObject.Find("Canvas").transform

--自己写的c#脚本相关
ABMgr = CS.ABMgr.Instance


