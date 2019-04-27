--统一的消息处理函数
msg,qq,group,id = nil,nil,nil,nil
local handled = false

--发送消息
--自动判断群聊与私聊
function sendMessage(s)
    if group then
        cqSendGroupMessage(group,s)
    else
        cqSendPrivateMessage(qq,s)
    end
end

--对外提供的函数接口
return function (inmsg,inqq,ingroup,inid)
    msg,qq,group,id = inmsg,inqq,ingroup,inid
    if msg:find("#lua") == 1 or msg:find("--lua") == 1 then
        local code
        if msg:find("#lua") == 1 then
            code = cqCqCode_UnTrope(msg:sub(5))
        elseif msg:find("--lua") == 1 then
            code = cqCqCode_UnTrope(msg:sub(6))
        end
        sendMessage(cqCode_At(qq).."\r\n"..apiSandBox(code))
    end
    return handled
end
